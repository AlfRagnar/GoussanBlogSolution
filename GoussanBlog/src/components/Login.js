import React, { useContext, useState } from "react";
import Form from "react-bootstrap/Form";
import axios from "axios";
import { AuthContext } from "../contexts/AuthContext";
import {
  Button,
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

export default function Login() {
  const classes = useStyles();
  const [open, setOpen] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const { setAuth, setToken } = useContext(AuthContext);

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
          setToken(res.data.jwtToken);
          setUsername("");
          setPassword("");
          console.log("User Logged In");
          sessionStorage.setItem("authToken", res.data.jwtToken);
          setOpen(false);
        });
    } catch (e) {
      localStorage.clear();
    }
  }

  return (
    <div>
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
          <Form className={classes.root} onSubmit={handleSubmit}>
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
                Login
              </Button>
            </div>
          </Form>
        </Fade>
      </Modal>
    </div>
  );
}
