import React, { useContext, useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import { makeStyles, TextField } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import AccountCircle from "@material-ui/icons/AccountCircle";
import { ThemeContext } from "../../contexts/ThemeContext";
import { ChatContext } from "../../contexts/ChatContext";
import { AuthContext } from "../../contexts/AuthContext";

export default function ChatInput() {
  const { sendMessage } = useContext(ChatContext);
  const { user } = useContext(AuthContext);
  const [chatUser, setChatUser] = useState("Anonymous");
  const [message, setMessage] = useState("");
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;

  useEffect(() => {
    if (user && user !== "") {
      setChatUser(user);
    }
  }, [user]);

  const useStyles = makeStyles((theme) => ({
    root: {
      margin: theme.spacing(2),
    },
    button: {
      color: currentTheme.text,
      background: currentTheme.background,
      position: "fixed",
    },
    userDisplay: {
      color: currentTheme.text,
      background: currentTheme.background,
    },
    chatInput: {
      color: currentTheme.text,
      background: currentTheme.background,
    },
  }));

  const classes = useStyles();

  const onSubmit = (e) => {
    e.preventDefault();

    const isUserProvided = chatUser && chatUser !== "";
    const isMessageProvided = message && message !== "";

    if (isUserProvided && isMessageProvided) {
      sendMessage(chatUser, message);
      setMessage("");
    } else {
      if (!isUserProvided && isMessageProvided) {
        alert("Please Insert a Username");
      } else if (isUserProvided && !isMessageProvided) {
        alert("Please Put in a Message");
      } else {
        alert("Please put in a Username and a Message");
      }
    }
  };

  return (
    <div className={classes.root}>
      <Form onSubmit={onSubmit} noValidate autoComplete="off">
        <TextField
          id="userDisplay"
          size="small"
          value={chatUser}
          onChange={(e) => setChatUser(e.target.value)}
          InputProps={{
            className: classes.userDisplay,
            startAdornment: (
              <AccountCircle style={{ color: currentTheme.text, margin: 3 }} />
            ),
          }}
        />
        <TextField
          id="message"
          value={message}
          onChange={(e) => setMessage(e.target.value)}
          label="Message"
          placeholder="Message"
          fullWidth
          multiline
          maxRows={2}
          InputProps={{
            className: classes.chatInput,
          }}
        />
        <br />
        <Button variant="contained" className={classes.button} type="submit">
          Submit
        </Button>
      </Form>
    </div>
  );
}
