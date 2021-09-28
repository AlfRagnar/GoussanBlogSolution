import React, { useContext, useState } from "react";
import {
  Backdrop,
  Button,
  Fade,
  makeStyles,
  Modal,
  Paper,
  TextField,
} from "@material-ui/core";
import Form from "react-bootstrap/Form";
import axios from "axios";
import { ThemeContext } from "../../../contexts/ThemeContext";
import { AuthContext } from "../../../contexts/AuthContext";

export default function CreateBlogs() {
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const { token } = useContext(AuthContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const [open, setOpen] = useState(false);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  const useStyles = makeStyles((theme) => ({
    Button: {
      color: currentTheme.text,
      background: currentTheme.background,
    },
    modal: {
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    },
    paper: {
      position: "absolute",
      maxWidth: "100vh",
      border: "2px solid #000",
      boxShadow: theme.shadows[5],
      padding: theme.spacing(2, 4, 3),
      background: currentTheme.background,
      color: currentTheme.text,
    },
    SubmitButton: {
      color: currentTheme.text,
      background: currentTheme.background,
    },
    input: {
      color: currentTheme.text,
      background: currentTheme.background,
    },
    inputLabel: {
      color: currentTheme.text,
    },
  }));
  const classes = useStyles();

  function validateForm(event) {
    event.preventDefault();
    handleSubmit(event);
  }

  async function handleSubmit(event) {
    event.preventDefault();
    try {
      const formData = new FormData();
      formData.append("Title", title);
      formData.append("Description", description);

      const bodyData = {
        title: title,
        Content: description,
      };

      await axios
        .post("/blog", bodyData, {
          headers: {
            Authorization: `bearer ${token}`,
          },
        })
        .then((res) => {
          setOpen(false);
        })
        .catch((e) => {
          console.log(e.response.data.errors);
          console.log(e.message);
          console.log(e);
        });
    } catch (e) {}
  }

  return (
    <>
      <Button color="inherit" onClick={() => setOpen(!open)}>
        Create new Blog Post
      </Button>
      <Modal
        id="popupBlog"
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        className={classes.modal}
        open={open}
        onClose={() => setOpen(!open)}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}>
        <Fade in={open}>
          <Paper className={classes.paper}>
            <Form className={classes.root} onSubmit={validateForm}>
              <Form.Group size="lg" controlId="title">
                <TextField
                  InputProps={{ className: classes.input }}
                  InputLabelProps={{ className: classes.inputLabel }}
                  id="title"
                  label="Title"
                  variant="filled"
                  multiline
                  maxRows={3}
                  onChange={(e) => setTitle(e.target.value)}
                />
              </Form.Group>
              <Form.Group size="lg" controlId="description">
                <TextField
                  InputProps={{ className: classes.input }}
                  InputLabelProps={{ className: classes.inputLabel }}
                  id="description"
                  label="Description"
                  variant="filled"
                  multiline
                  maxRows={10}
                  onChange={(e) => setDescription(e.target.value)}
                />
              </Form.Group>
              <Button
                className={classes.SubmitButton}
                variant="outlined"
                size="medium"
                type="submit">
                Create Blog Post
              </Button>
            </Form>
          </Paper>
        </Fade>
      </Modal>
    </>
  );
}
