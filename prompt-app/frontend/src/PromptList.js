import React, { useEffect, useState } from 'react';
import promptService from './services/promptService';

function PromptList({ token, onLogout }) {
  const [prompts, setPrompts] = useState([]);
  const [search, setSearch] = useState('');
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [editing, setEditing] = useState(null);

  const fetchPrompts = async (q = '') => {
    const data = await promptService.getPrompts(token, q);
    setPrompts(data);
  };

  useEffect(() => {
    fetchPrompts();
  }, []);

  const handleSearch = (e) => {
    setSearch(e.target.value);
    fetchPrompts(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (editing) {
      await promptService.updatePrompt(token, editing.id, { title, content });
    } else {
      await promptService.createPrompt(token, { title, content });
    }
    setTitle('');
    setContent('');
    setEditing(null);
    fetchPrompts(search);
  };

  const handleEdit = (prompt) => {
    setEditing(prompt);
    setTitle(prompt.title);
    setContent(prompt.content);
  };

  const handleDelete = async (id) => {
    await promptService.deletePrompt(token, id);
    fetchPrompts(search);
  };

  return (
    <div>
      <button onClick={onLogout}>Logout</button>
      <h2>Prompts</h2>
      <input
        placeholder="Search prompts"
        value={search}
        onChange={handleSearch}
      />
      <ul>
        {prompts.map((p) => (
          <li key={p.id}>
            <strong>{p.title}</strong>: {p.content}
            <button onClick={() => handleEdit(p)}>Edit</button>
            <button onClick={() => handleDelete(p.id)}>Delete</button>
          </li>
        ))}
      </ul>
      <form onSubmit={handleSubmit}>
        <h3>{editing ? 'Edit Prompt' : 'Add Prompt'}</h3>
        <input
          placeholder="Title"
          value={title}
          onChange={e => setTitle(e.target.value)}
          required
        />
        <textarea
          placeholder="Content"
          value={content}
          onChange={e => setContent(e.target.value)}
          required
        />
        <button type="submit">{editing ? 'Update' : 'Add'}</button>
        {editing && <button type="button" onClick={() => { setEditing(null); setTitle(''); setContent(''); }}>Cancel</button>}
      </form>
    </div>
  );
}

export default PromptList;