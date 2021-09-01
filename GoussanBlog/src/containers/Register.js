import React, { useContext, useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import axios from "axios";
import { ThemeContext } from "../contexts/ThemeContext";
import { Paper } from "@material-ui/core";

export default function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  function validateForm() {
    return username.length > 0 && password.length > 0;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    try {
      await axios
        .post("/user/register", { username, email, password })
        .then((res) => {
          alert("User Registered");
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
    <div className="container">
      <Paper
        variant="outlined"
        style={{
          background: theme.background,
          color: theme.text,
          textAlign: "center",
        }}>
        <Form onSubmit={handleSubmit}>
          <Form.Group size="lg" controlId="email">
            <Form.Label>Email</Form.Label>
            <Form.Control
              autoFocus
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </Form.Group>
          <Form.Group size="lg" controlId="username">
            <Form.Label>Username</Form.Label>
            <Form.Control
              type="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </Form.Group>

          <Form.Group size="lg" controlId="password">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </Form.Group>
          <Button
            block="true"
            size="lg"
            type="submit"
            disabled={!validateForm()}>
            Login
          </Button>
        </Form>
      </Paper>
    </div>
  );
}
