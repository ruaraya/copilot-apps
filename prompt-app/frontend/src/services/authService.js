import axios from 'axios';

const API_URL = process.env.API_URL + '/auth';
console.log(API_URL);
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
