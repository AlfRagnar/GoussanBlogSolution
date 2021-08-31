import React, { useContext, useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import axios from "axios";
import { AuthContext } from "../contexts/AuthContext";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const { setAuth, setToken } = useContext(AuthContext);

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
          setAuth(true);
          setToken(res.data.jwtToken);
          setUsername("");
          setPassword("");
          console.log("User Logged In");
        });
    } catch (e) {
      console.log("Invalid Username/password");
      console.log(e.message);
    }
  }

  return (
    <div className="container">
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
