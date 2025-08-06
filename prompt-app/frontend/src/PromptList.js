import React, { useEffect, useState } from 'react';
import promptService from './services/promptService';
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Divider,
  AppBar,
  Toolbar,
  Container,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import LogoutIcon from '@mui/icons-material/Logout';

function PromptList({ token, onLogout }) {
  const [prompts, setPrompts] = useState([]);
  const [search, setSearch] = useState('');
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [editing, setEditing] = useState(null);
  const [open, setOpen] = useState(false);

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
    setOpen(false);
    fetchPrompts(search);
  };

  const handleEdit = (prompt) => {
    setEditing(prompt);
    setTitle(prompt.title);
    setContent(prompt.content);
    setOpen(true);
  };

  const handleDelete = async (id) => {
    await promptService.deletePrompt(token, id);
    fetchPrompts(search);
  };

  const handleAdd = () => {
    setEditing(null);
    setTitle('');
    setContent('');
    setOpen(true);
  };

  const handleClose = () => {
    setEditing(null);
    setTitle('');
    setContent('');
    setOpen(false);
  };

  return (
    <Box>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Prompts
          </Typography>
          <Button color="inherit" startIcon={<LogoutIcon />} onClick={onLogout}>
            Logout
          </Button>
        </Toolbar>
      </AppBar>
      <Container maxWidth="sm" sx={{ mt: 4 }}>
        <Paper sx={{ p: 2, mb: 2 }}>
          <TextField
            label="Search prompts"
            value={search}
            onChange={handleSearch}
            fullWidth
            margin="normal"
          />
          <Button
            variant="contained"
            color="primary"
            onClick={handleAdd}
            sx={{ mt: 1 }}
            fullWidth
          >
            Add Prompt
          </Button>
        </Paper>
        <Paper sx={{ p: 2 }}>
          <List>
            {prompts.map((p) => (
              <React.Fragment key={p.id}>
                <ListItem alignItems="flex-start">
                  <ListItemText
                    primary={p.title}
                    secondary={p.content}
                  />
                  <ListItemSecondaryAction>
                    <IconButton edge="end" aria-label="edit" onClick={() => handleEdit(p)}>
                      <EditIcon />
                    </IconButton>
                    <IconButton edge="end" aria-label="delete" onClick={() => handleDelete(p.id)}>
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>
                <Divider component="li" />
              </React.Fragment>
            ))}
          </List>
        </Paper>
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
          <DialogTitle>{editing ? 'Edit Prompt' : 'Add Prompt'}</DialogTitle>
          <form onSubmit={handleSubmit}>
            <DialogContent>
              <TextField
                label="Title"
                value={title}
                onChange={e => setTitle(e.target.value)}
                fullWidth
                margin="normal"
                required
              />
              <TextField
                label="Content"
                value={content}
                onChange={e => setContent(e.target.value)}
                fullWidth
                margin="normal"
                multiline
                minRows={3}
                required
              />
            </DialogContent>
            <DialogActions>
              <Button onClick={handleClose}>Cancel</Button>
              <Button type="submit" variant="contained" color="primary">
                {editing ? 'Update' : 'Add'}
              </Button>
            </DialogActions>
          </form>
        </Dialog>
      </Container>
    </Box>
  );
}

export default PromptList;