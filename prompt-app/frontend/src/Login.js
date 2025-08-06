import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from './services/authService';
import { Box, Button, TextField, Typography, Alert, Paper } from '@mui/material';

function Login({ onLogin }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const res = await authService.loginUser(username, password);
      onLogin(res.data.token);
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.error || 'Login failed');
    }

    //const res = await authService.loginUser(username, password);
    //const data = await res.json();
    //console.log(res);
    // if (res.ok) {
    //   onLogin(res.token);
    // } else {
    //   setError(res.error || 'Login failed');
    // }

    // if (res.request.status === 200) {
    //   onLogin(res.data.token);
    //   navigate('/');
    // } else {
    //   setError(res.data.error || 'Login failed');
    // }
  };

  return (
    <Box
      component={Paper}
      elevation={3}
      sx={{ p: 4, maxWidth: 400, mx: 'auto', mt: 8 }}
    >
      <form onSubmit={handleSubmit}>
        <Typography variant="h4" gutterBottom>Login</Typography>
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        <TextField
          label="Username"
          value={username}
          onChange={e => setUsername(e.target.value)}
          fullWidth
          margin="normal"
          autoFocus
        />
        <TextField
          label="Password"
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
          fullWidth
          margin="normal"
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
          sx={{ mt: 2 }}
        >
          Login
        </Button>
      </form>
    </Box>
  );
}

export default Login;
