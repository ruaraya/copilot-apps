import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL + '/auth';

const registerUser = async (username, password) => {
  const res = await axios.post(API_URL + '/register', { username, password }, {
    headers: { 'Content-Type': 'application/json' },
  });
  return res.data;
}

const loginUser = async (username, password) => {
  const res = await axios.post(API_URL + '/login', { username, password }, {
    headers: { 'Content-Type': 'application/json' },
  });

  return res;
}

export default {
  registerUser,
  loginUser
};
