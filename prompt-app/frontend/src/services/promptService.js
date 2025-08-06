import axios from 'axios';

const API_URL = process.env.API_URL + '/prompts'

const getPrompts = async (token, q = '') => {
  const res = await axios.get(API_URL, {
    headers: { Authorization: `Bearer ${token}` },
    params: q ? { q } : {},
  });
  return res.data;
};

const createPrompt = async (token, { title, content }) => {
  const res = await axios.post(
    API_URL,
    { title, content },
    { headers: { Authorization: `Bearer ${token}` } }
  );
  return res.data;
};

const updatePrompt = async (token, id, { title, content }) => {
  const res = await axios.put(
    `${API_URL}/${id}`,
    { title, content },
    { headers: { Authorization: `Bearer ${token}` } }
  );
  return res.data;
};

const deletePrompt = async (token, id) => {
  const res = await axios.delete(`${API_URL}/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return res.data;
};

export default {
  getPrompts,
  createPrompt,
  updatePrompt,
  deletePrompt,
};
