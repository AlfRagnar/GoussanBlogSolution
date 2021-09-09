import React, { useContext, useEffect, useState } from "react";
import {
  Input,
  InputAdornment,
  InputLabel,
  makeStyles,
} from "@material-ui/core";
import { Form } from "react-bootstrap";
import { AuthContext } from "../../contexts/AuthContext";
import { ChatContext } from "../../contexts/ChatContext";
import Button from "@material-ui/core/Button";
import { ThemeContext } from "../../contexts/ThemeContext";
import AccountCircle from "@material-ui/icons/AccountCircle";

const useStyles = makeStyles((theme) => ({
  root: {
    margin: theme.spacing(1),
  },
}));

export default function ChatInput() {
  const classes = useStyles();
  const { sendMessage } = useContext(ChatContext);
  const { user } = useContext(AuthContext);
  const [chatUser, setChatUser] = useState("Anonymous");
  const [message, setMessage] = useState("");
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  useEffect(() => {
    if (user && user !== "") {
      setChatUser(user);
    }
  }, [user]);

  const onSubmit = (e) => {
    e.preventDefault();

    const isUserProvided = chatUser && chatUser !== "";
    const isMessageProvided = message && message !== "";

    if (isUserProvided && isMessageProvided) {
      sendMessage(chatUser, message);
      setMessage("");
    } else {
      alert("Please insert an user and a message");
    }
  };

  const onUserUpdate = (e) => {
    setChatUser(e.target.value);
  };
  const onMessageUpdate = (e) => {
    setMessage(e.target.value);
  };

  return (
    <div className={classes.root}>
      <Form onSubmit={onSubmit} noValidate autoComplete="off">
        <Input
          style={{ color: theme.text, background: theme.background }}
          id="user"
          name="user"
          value={chatUser}
          onChange={onUserUpdate}
          startAdornment={
            <InputAdornment position="start">
              <AccountCircle />
            </InputAdornment>
          }
        />

        <InputLabel
          style={{ color: theme.text, background: theme.background }}
          htmlFor="message">
          Message
        </InputLabel>

        <Input
          style={{ color: theme.text, background: theme.background }}
          type="text"
          id="message"
          value={message}
          onChange={onMessageUpdate}
        />
        <br />
        <Button variant="contained" color="primary" type="submit">
          Submit
        </Button>
      </Form>
    </div>
  );
}
