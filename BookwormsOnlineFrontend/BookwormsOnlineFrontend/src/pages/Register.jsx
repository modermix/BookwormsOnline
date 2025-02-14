import React, { useState } from 'react';
import { Box, Typography, TextField, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as yup from 'yup';
import http from '../http';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function Register() {
    const navigate = useNavigate();
    const [imageFile, setImageFile] = useState(null);

    const formik = useFormik({
        initialValues: {
            firstName: "",
            lastName: "",
            email: "",
            password: "",
            confirmPassword: "",
            creditCardNo: "",
            mobileNo: "",
            billingAddress: "",
            shippingAddress: ""
        },
        validationSchema: yup.object({
            firstName: yup.string().trim()
                .min(3, 'First Name must be at least 3 characters')
                .max(50, 'First Name must be at most 50 characters')
                .required('First Name is required')
                .matches(/^[a-zA-Z '-,.]+$/, "First Name only allows letters, spaces, and characters: ' - , ."),
            lastName: yup.string().trim()
                .min(3, 'Last Name must be at least 3 characters')
                .max(50, 'Last Name must be at most 50 characters')
                .required('Last Name is required')
                .matches(/^[a-zA-Z '-,.]+$/, "Last Name only allows letters, spaces, and characters: ' - , ."),
            email: yup.string().trim()
                .email('Enter a valid email')
                .max(50, 'Email must be at most 50 characters')
                .required('Email is required'),
            password: yup.string().trim()
                .min(8, 'Password must be at least 8 characters')
                .max(50, 'Password must be at most 50 characters')
                .required('Password is required')
                .matches(/^(?=.*[a-zA-Z])(?=.*[0-9]).{8,}$/, "Password must contain at least 1 letter and 1 number"),
            confirmPassword: yup.string().trim()
                .required('Confirm password is required')
                .oneOf([yup.ref('password')], 'Passwords must match'),
            creditCardNo: yup.string().trim()
                .min(15, 'Credit Card No must be at least 15 digits')
                .max(19, 'Credit Card No must be at most 19 digits')
                .required('Credit Card No is required')
                .matches(/^\d+$/, "Credit Card No must be numeric"),
            mobileNo: yup.string().trim()
                .length(8, 'Mobile No must be exactly 8 digits')
                .required('Mobile No is required')
                .matches(/^\d+$/, "Mobile No must be numeric"),
            billingAddress: yup.string().trim()
                .max(255, 'Billing Address must be at most 255 characters')
                .required('Billing Address is required'),
            shippingAddress: yup.string().trim()
                .max(255, 'Shipping Address must be at most 255 characters')
                .required('Shipping Address is required')
        }),
        onSubmit: (data) => {
            // Trim string values and format as necessary
            data.firstName = data.firstName.trim();
            data.lastName = data.lastName.trim();
            data.email = data.email.trim().toLowerCase();
            data.password = data.password.trim();

            const formData = new FormData();
            formData.append('firstName', data.firstName);
            formData.append('lastName', data.lastName);
            formData.append('email', data.email);
            formData.append('password', data.password);
            formData.append('confirmPassword', data.confirmPassword);
            formData.append('creditCardNo', data.creditCardNo);
            formData.append('mobileNo', data.mobileNo);
            formData.append('billingAddress', data.billingAddress);
            formData.append('shippingAddress', data.shippingAddress);

            if (imageFile) {
                formData.append("file", imageFile);
            }

            http.post("/user/register", formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then((res) => {
                navigate("/login");
            })
            .catch(function (err) {
                toast.error(`${err.response.data.message}`);
            });
        }
    });

    const onFileChange = (e) => {
        let file = e.target.files[0];
        if (file) {
            const fileExtension = file.name.split('.').pop().toLowerCase();
            if (fileExtension !== 'jpg') {
                toast.error('Only .jpg files are allowed');
                setImageFile(null); // Reset the image file state
                return; // Prevent further action
            }
            setImageFile(file);
        }
    };

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center'
        }}>
            <Typography variant="h5" sx={{ my: 2 }}>
                Register
            </Typography>
            <Box component="form" sx={{ maxWidth: '500px' }} onSubmit={formik.handleSubmit}>
                {/* Form Fields */}
                <TextField
                    fullWidth margin="dense" autoComplete="off"
                    label="First Name"
                    name="firstName"
                    value={formik.values.firstName}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.firstName && Boolean(formik.errors.firstName)}
                    helperText={formik.touched.firstName && formik.errors.firstName}
                />
                <TextField
                    fullWidth margin="dense" autoComplete="off"
                    label="Last Name"
                    name="lastName"
                    value={formik.values.lastName}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.lastName && Boolean(formik.errors.lastName)}
                    helperText={formik.touched.lastName && formik.errors.lastName}
                />
                <TextField
                    fullWidth margin="dense" autoComplete="off"
                    label="Email"
                    name="email"
                    value={formik.values.email}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.email && Boolean(formik.errors.email)}
                    helperText={formik.touched.email && formik.errors.email}
                />
                <TextField
                    fullWidth margin="dense" autoComplete="off"
                    label="Password"
                    name="password" type="password"
                    value={formik.values.password}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.password && Boolean(formik.errors.password)}
                    helperText={formik.touched.password && formik.errors.password}
                />
                <TextField
                    fullWidth margin="dense" autoComplete="off"
                    label="Confirm Password"
                    name="confirmPassword" type="password"
                    value={formik.values.confirmPassword}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.confirmPassword && Boolean(formik.errors.confirmPassword)}
                    helperText={formik.touched.confirmPassword && formik.errors.confirmPassword}
                />
                {/* New Fields */}
                <TextField
                    fullWidth margin="dense"
                    label="Credit Card No"
                    name="creditCardNo"
                    value={formik.values.creditCardNo}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.creditCardNo && Boolean(formik.errors.creditCardNo)}
                    helperText={formik.touched.creditCardNo && formik.errors.creditCardNo}
                />
                <TextField
                    fullWidth margin="dense"
                    label="Mobile No"
                    name="mobileNo"
                    value={formik.values.mobileNo}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.mobileNo && Boolean(formik.errors.mobileNo)}
                    helperText={formik.touched.mobileNo && formik.errors.mobileNo}
                />
                <TextField
                    fullWidth margin="dense"
                    label="Billing Address"
                    name="billingAddress"
                    value={formik.values.billingAddress}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.billingAddress && Boolean(formik.errors.billingAddress)}
                    helperText={formik.touched.billingAddress && formik.errors.billingAddress}
                />
                <TextField
                    fullWidth margin="dense"
                    label="Shipping Address"
                    name="shippingAddress"
                    value={formik.values.shippingAddress}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    error={formik.touched.shippingAddress && Boolean(formik.errors.shippingAddress)}
                    helperText={formik.touched.shippingAddress && formik.errors.shippingAddress}
                />

                {/* File upload button */}
                <Button variant="contained" component="label" sx={{ mt: 2 }}>
                    Upload Profile Image
                    <input hidden accept="image/*" type="file" onChange={onFileChange} />
                </Button>
                {/* Display selected image */}
                {imageFile && (
                    <Box sx={{ mt: 2, textAlign: 'center' }}>
                        <Typography variant="body2">Selected file: {imageFile.name}</Typography>
                    </Box>
                )}

                <Button fullWidth variant="contained" sx={{ mt: 2 }} type="submit">
                    Register
                </Button>
            </Box>

            <ToastContainer />
        </Box>
    );
}

export default Register;
