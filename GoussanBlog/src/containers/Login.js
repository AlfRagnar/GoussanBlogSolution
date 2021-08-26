import React, { useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import "./Login.css";
import axios from "axios";
import { useAppContext } from "../contexts/contextLib";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const { userHasAuthenticated } = useAppContext();

  function validateForm() {
    return username.length > 0 && password.length > 0;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    try {
      await axios
        .post("/user/authenticate", {
          username: username,
          password: password,
        })
        .then((res) => {
          userHasAuthenticated({ auth: true, token: res.data.jwtToken });
          setUsername("");
          setPassword("");
        });
    } catch (e) {
      console.log("Invalid Username/password");
      console.log(e.message);
    }
  }

  return (
    <div className="Login">
      <Form onSubmit={handleSubmit}>
        <Form.Group size="lg" controlId="username">
          <Form.Label>Username</Form.Label>
          <Form.Control
            autoFocus
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
          className="mb-2"
          variant="primary"
          size="lg"
          type="submit"
          disabled={!validateForm()}>
          Login
        </Button>
      </Form>
    </div>
  );
}
