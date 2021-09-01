import React, { useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import axios from "axios";
import {
  Backdrop,
  Fade,
  makeStyles,
  Modal,
  TextField,
} from "@material-ui/core";

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

export default function Register() {
  const classes = useStyles();
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [open, setOpen] = React.useState(false);

  function validateForm() {
    return username.length > 0 && password.length > 0;
  }

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  async function handleSubmit(event) {
    event.preventDefault();
    try {
      await axios
        .post("/user/register", { username, email, password })
        .then((res) => {
          setUsername("");
          setEmail("");
          setPassword("");
        });
    } catch (e) {
      alert("Invalid Username/password");
      console.log(e.message);
    }
  }

  return (
    <div>
      <Button
        className="mb-2"
        variant="secondary"
        type="button"
        onClick={handleOpen}>
        Register
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
          <Form className={classes.root} onSubmit={handleSubmit}>
            <Form.Group size="lg" controlId="email">
              <TextField
                className={classes.input}
                id="email"
                label="Email"
                variant="filled"
                multiline
                onChange={(e) => setEmail(e.target.value)}
              />
            </Form.Group>
            <Form.Group size="lg" controlId="username">
              <TextField
                className={classes.input}
                id="username"
                label="Username"
                variant="filled"
                multiline
                onChange={(e) => setUsername(e.target.value)}
              />
            </Form.Group>
            <Form.Group size="lg" controlId="password">
              <TextField
                className={classes.input}
                id="password"
                label="Password"
                variant="filled"
                multiline
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>
            <div className="container">
              <Button
                className="mb-2"
                variant="primary"
                size="lg"
                type="submit"
                disabled={!validateForm()}>
                Register
              </Button>
            </div>
          </Form>
        </Fade>
      </Modal>
    </div>
  );
}
