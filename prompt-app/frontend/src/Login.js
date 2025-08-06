import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from './services/authService';

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
    <form onSubmit={handleSubmit}>
      <h2>Login</h2>
      {error && <div style={{ color: 'red' }}>{error}</div>}
      <input placeholder="Username" value={username} onChange={e => setUsername(e.target.value)} />
      <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
      <button type="submit">Login</button>
    </form>
  );
}

export default Login;
