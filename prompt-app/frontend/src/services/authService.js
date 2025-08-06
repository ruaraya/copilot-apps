import axios from 'axios';

const API_URL = 'http://localhost:4000/api/auth';

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
  console.log(res);
  //return res.data;
  return res;
}

export default {
  registerUser,
  loginUser
};
