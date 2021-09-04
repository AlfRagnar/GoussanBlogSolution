import React, { useContext, useState } from "react";
import Form from "react-bootstrap/Form";
import {
  Button,
  Backdrop,
  Fade,
  makeStyles,
  Modal,
  Paper,
  TextField,
} from "@material-ui/core";
import axios from "axios";
import { ThemeContext } from "../contexts/ThemeContext";
import { AuthContext } from "../contexts/AuthContext";

const useStyles = makeStyles((theme) => ({
  root: {
    "& > *": {
      margin: theme.spacing(1),
      width: "30ch",
    },
  },
  input: {
    background: "#ffff",
    borderRadius: 8,
  },
  modal: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  paper: {
    backgroundColor: theme.palette.background.paper,
    border: "2px solid #000",
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
}));

export default function Upload() {
  const classes = useStyles();
  const [open, setOpen] = useState(false);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const { token } = useContext(AuthContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [file, setFile] = useState();

  function validateForm() {
    return title.length > 0 && description.length > 0 && file !== undefined;
  }

  const handleOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  const onFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  async function handleSubmit(event) {
    event.preventDefault();
    try {
      const formData = new FormData();
      formData.append("Title", title);
      formData.append("Description", description);
      formData.append("File", file, file.name);
      formData.append("token", token);

      await axios
        .post("/Videos", formData, {
          headers: {
            Authorization: `bearer ${token}`,
          },
        })
        .then((res) => {
          setOpen(false);
        });
    } catch (e) {
      console.log(e.message);
    }
  }

  return (
    <div>
      <Button color="inherit" onClick={handleOpen}>
        Upload
      </Button>
      <Modal
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        className={classes.modal}
        open={open}
        onClose={handleClose}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}>
        <Fade in={open}>
          <Paper
            style={{
              background: theme.background,
              color: theme.text,
              textAlign: "center",
            }}>
            <Form className={classes.root} onSubmit={handleSubmit}>
              <Form.Group size="lg" controlId="title">
                <TextField
                  className={classes.input}
                  id="title"
                  label="Title"
                  variant="filled"
                  multiline
                  onChange={(e) => setTitle(e.target.value)}
                />
              </Form.Group>
              <Form.Group size="lg" controlId="description">
                <TextField
                  className={classes.input}
                  id="description"
                  label="Description"
                  variant="filled"
                  multiline
                  onChange={(e) => setDescription(e.target.value)}
                />
              </Form.Group>
              <Form.Group size="lg" controlId="file">
                <input type="file" onChange={onFileChange} />
              </Form.Group>
              <Button
                className="mb-2"
                style={{ color: theme.text, background: theme.background }}
                variant="outlined"
                size="medium"
                type="submit"
                disabled={!validateForm()}>
                Upload
              </Button>
            </Form>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
