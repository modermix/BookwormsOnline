using BookwormsOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NanoidDotNet;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookwormsOnline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(MyDbContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly MyDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("register")]
        public IActionResult Register([FromForm] RegisterRequest request, [FromForm] IFormFile? file)
        {
            // Trim string values
            request.FirstName = request.FirstName.Trim();
            request.LastName = request.LastName.Trim();
            request.Email = request.Email.Trim().ToLower();
            request.Password = request.Password.Trim();
            request.CreditCardNo = request.CreditCardNo.Trim();
            request.MobileNo = request.MobileNo.Trim();
            request.BillingAddress = request.BillingAddress.Trim();
            request.ShippingAddress = request.ShippingAddress.Trim();

            string fullName = request.FirstName + " " + request.LastName;

            // Check email
            var foundUser = _context.Users.Where(x => x.Email == request.Email).FirstOrDefault();
            if (foundUser != null)
            {
                string message = "Email already exists.";
                return BadRequest(new { message });
            }

            // Handle file upload if file is provided
            string? imageFile = null;
            if (file != null)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".jpg")
                {
                    return BadRequest(new { message = "Only .jpg files are allowed." });
                }

                if (file.Length > 1024 * 1024) // Max file size 1MB
                {
                    return BadRequest(new { message = "Maximum file size is 1MB" });
                }

                // Using your existing FileController's logic
                var id = Nanoid.Generate(size: 10);
                imageFile = id + Path.GetExtension(file.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploads", imageFile);
                using var fileStream = new FileStream(imagePath, FileMode.Create);
                file.CopyTo(fileStream);
            }

            // Create user object
            var now = DateTime.Now;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            string creditcardHash = BCrypt.Net.BCrypt.HashPassword(request.CreditCardNo);
            var user = new User()
            {
                Name = fullName,
                Email = request.Email,
                Password = passwordHash,
                CreatedAt = now,
                UpdatedAt = now,
                ProfileImage = imageFile, // Store the profile image filename
                CreditCardNo = creditcardHash,
                MobileNo = request.MobileNo,
                BillingAddress = request.BillingAddress,
                ShippingAddress = request.ShippingAddress
            };

            // Add user
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // Trim string values
            request.Email = request.Email.Trim().ToLower();
            request.Password = request.Password.Trim();

            // Check email and password
            string message = "Email or password is not correct.";
            var foundUser = _context.Users.Where(x => x.Email == request.Email).FirstOrDefault();
            if (foundUser == null)
            {
                return BadRequest(new { message });
            }

            if (foundUser.LockoutEnd > DateTime.Now)
            {
                return BadRequest(new { message = "Account is locked. Please try again later." });
            }

            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, foundUser.Password);
            if (!verified)
            {
                foundUser.FailedLoginAttempts++;
                if (foundUser.FailedLoginAttempts >= 3)
                {
                    foundUser.LockoutEnd = DateTime.Now.AddMinutes(5); // Lock account for 15 minutes
                }
                _context.SaveChanges();
                return BadRequest(new { message });
            }

            foundUser.FailedLoginAttempts = 0;
            foundUser.LockoutEnd = null;
            _context.SaveChanges();


            // Return user info
            var user = new
            {
                foundUser.Id,
                foundUser.Email,
                foundUser.Name,
                foundUser.MobileNo,          // Include mobile number
                foundUser.BillingAddress,    // Include billing address
                foundUser.ShippingAddress,   // Include shipping address
                foundUser.ProfileImage,
                foundUser.Password,
                foundUser.CreditCardNo
            };
            string accessToken = CreateToken(foundUser);
            return Ok(new { user, accessToken });
        }

        [HttpGet("auth"), Authorize]
        public IActionResult Auth()
        {
            var id = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault());
            var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).SingleOrDefault();

            var foundUser = _context.Users.FirstOrDefault(u => u.Id == id);

            if (foundUser != null)
            {
                var user = new
                {
                    id = foundUser.Id,
                    name = foundUser.Name,
                    email = foundUser.Email,
                    mobileNo = foundUser.MobileNo,
                    billingAddress = foundUser.BillingAddress,
                    shippingAddress = foundUser.ShippingAddress,
                    profileImage = foundUser.ProfileImage, // Include the profile image filename
                    password = foundUser.Password,
                    creditCardNo = foundUser.CreditCardNo
                };
                return Ok(new { user });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string CreateToken(User user)
        {
            string? secret = _configuration.GetValue<string>("Authentication:Secret");
            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("Secret is required for JWT authentication.");
            }

            int tokenExpiresDays = _configuration.GetValue<int>("Authentication:TokenExpiresDays");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII    .GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                ]),
                Expires = DateTime.UtcNow.AddDays(tokenExpiresDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
