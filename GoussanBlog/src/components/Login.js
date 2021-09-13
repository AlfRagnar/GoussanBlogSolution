import React, { useContext, useState } from "react";
import Form from "react-bootstrap/Form";
import axios from "axios";
import {
  Button,
  Backdrop,
  Fade,
  makeStyles,
  Modal,
  TextField,
  Paper,
} from "@material-ui/core";
import { ThemeContext } from "../contexts/ThemeContext";
import { AuthContext } from "../contexts/AuthContext";

export default function Login() {
  const [open, setOpen] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const { setAuth, setToken, setUser } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);

  const currentTheme = isDarkTheme ? darkTheme : lightTheme;

  const useStyles = makeStyles((theme) => ({
    Button: {
      color: currentTheme.background,
      background: currentTheme.text,
    },
    modal: {
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    },
    paper: {
      position: "absolute",
      maxWidth: "75%",
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
    userInputGroup: {
      margin: theme.spacing(1),
    },
    passInputGroup: {
      margin: theme.spacing(1),
    },
    formSubmitBtn: {
      margin: theme.spacing(1),
    },
  }));
  const classes = useStyles();

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
        .post("/user/authenticate", {
          username: username,
          password: password,
        })
        .then((res) => {
          setAuth(true);
          setUser(username);
          setToken(res.data.jwtToken);
          sessionStorage.setItem("authToken", res.data.jwtToken);
          sessionStorage.setItem("user", username);
          setUsername("");
          setPassword("");
          setOpen(false);
        });
    } catch (e) {
      localStorage.clear();
    }
  }

  return (
    <>
      <Button color="inherit" onClick={handleOpen}>
        Login
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
          <Paper className={classes.paper}>
            <Form onSubmit={handleSubmit}>
              <Form.Group
                className={classes.userInputGroup}
                size="lg"
                controlId="username">
                <TextField
                  InputProps={{ className: classes.input }}
                  InputLabelProps={{ className: classes.inputLabel }}
                  id="standard-username-input"
                  label="Username"
                  autoComplete="current-username"
                  variant="outlined"
                  onChange={(e) => setUsername(e.target.value)}
                />
              </Form.Group>
              <Form.Group
                className={classes.passInputGroup}
                size="lg"
                controlId="password">
                <TextField
                  InputProps={{ className: classes.input }}
                  InputLabelProps={{ className: classes.inputLabel }}
                  id="password"
                  label="Password"
                  type="password"
                  autoComplete="current-password"
                  variant="outlined"
                  onChange={(e) => setPassword(e.target.value)}
                />
              </Form.Group>
              <Form.Group className={classes.formSubmitBtn}>
                <Button
                  className={classes.submitButton}
                  variant="contained"
                  type="submit"
                  disabled={!validateForm()}>
                  Login
                </Button>
              </Form.Group>
            </Form>
          </Paper>
        </Fade>
      </Modal>
    </>
  );
}
