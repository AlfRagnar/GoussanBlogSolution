import React, { useState, useContext } from "react";
import Form from "react-bootstrap/Form";
import axios from "axios";
import {
  Button,
  Backdrop,
  Fade,
  makeStyles,
  Modal,
  TextField,
} from "@material-ui/core";
import { ThemeContext } from "../contexts/ThemeContext";

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
  const [rPassword, setRPassword] = useState("");
  const [open, setOpen] = React.useState(false);

  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);

  const theme = isDarkTheme ? darkTheme : lightTheme;

  function validateForm() {
    // eslint-disable-next-line no-useless-escape
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var emailCheck = email.match(mailformat);
    var lengthCheck = username.length > 0 && password.length > 0;
    var passwordCheck = password === rPassword;
    return lengthCheck && passwordCheck && emailCheck;
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
        .then(() => {
          setUsername("");
          setEmail("");
          setPassword("");
          setOpen(false);
        });
    } catch (e) {
      alert("Invalid Username/password");
    }
  }

  return (
    <div>
      <Button color="inherit" onClick={handleOpen}>
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
                type="password"
                autoComplete="current-password"
                variant="filled"
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>
            <Form.Group size="lg" controlId="repeat-password">
              <TextField
                className={classes.input}
                id="repeat-password"
                label="repeat-password"
                type="password"
                autoComplete="current-password"
                variant="filled"
                onChange={(e) => setRPassword(e.target.value)}
                helperText="Repeat your Password"
              />
            </Form.Group>
            <div className="container">
              <Button
                className="mb-2"
                style={{ color: theme.text, background: theme.background }}
                variant="outlined"
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
