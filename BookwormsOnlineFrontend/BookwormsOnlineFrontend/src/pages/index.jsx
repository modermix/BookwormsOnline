import React, { useState, useEffect } from 'react';
import http from '../http'; // Assuming you have a configured HTTP client for API calls (axios, for example)

const Index = () => {
  const [user, setUser] = useState(null); // Store logged-in user data
  const [loading, setLoading] = useState(true); // Loading state for API call

  useEffect(() => {
    // Get the token from localStorage (or sessionStorage) that was saved upon login
    const token = localStorage.getItem('accessToken');
    if (token) {
      // Fetch user details using the token
      http.get('/user/auth', {
        headers: {
          Authorization: `Bearer ${token}`, // Send the token in the Authorization header
        }
      })
      .then((response) => {
        setUser(response.data.user); // Assuming your API returns the user data under 'user' key
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error fetching user details:', error);
        setLoading(false);
      });
    } else {
      setLoading(false); // If no token, stop loading and no user data to display
    }
  }, []);

  if (loading) {
    return <div>Loading...</div>; // Show a loading message while fetching data
  }

  if (!user) {
    return <div>Please log in to see your profile.</div>; // If user is not logged in
  }

  return (
    <div>
      {/* Content */}
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '100vh', textAlign: 'center', paddingTop: '80px' }}>
        <img src="../assets/BookwormsOnline.png" alt="Bookworms" style={{ width: '150px', height: 'auto', marginBottom: '20px' }} />
        <h1 style={{ fontWeight: 'bold', color: '#333' }}>Bookworms Online</h1>

        {/* Display User Details */}
        <div style={{ marginTop: '20px' }}>
          <h3>Welcome, {user.name}</h3>
          <p>Email: {user.email}</p>
          <p>Billing Address: {user.billingAddress}</p>
          <p>Shipping Address: {user.shippingAddress}</p>
          {/* You can also include profile image if necessary */}
          {user.profileImage && (
            <img src={`/uploads/${user.profileImage}`} alt="Profile" style={{ width: '100px', height: 'auto', marginTop: '10px' }} />
          )}
        </div>
      </div>
    </div>
  );
};

export default Index;
