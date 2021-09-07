import { makeStyles } from "@material-ui/core";
import { useContext, useEffect } from "react";
import { ChatContext } from "../../contexts/ChatContext";
import { ThemeContext } from "../../contexts/ThemeContext";
import ChatInput from "./ChatInput";
import ChatMessage from "./ChatMessage";

const useStyles = makeStyles((theme) => ({
  chatInput: {
    border: "2px solid #000",
    flexDirection: "flex-end",
  },
  chatContent: {
    margin: theme.spacing(1),
    height: "55vh",
    flexDirection: "flex-end",
    overflowX: "auto",
  },
  ChatContainer: {},
}));

export default function ChatWindow() {
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const classes = useStyles();
  const { chat } = useContext(ChatContext);

  const currentChat = chat.map((m) => (
    <ChatMessage
      key={Date.now() * Math.random()}
      user={m.user}
      message={m.message}
    />
  ));

  useEffect(() => {
    var chatHistory = document.getElementById("chatcontent");
    chatHistory.scrollTop = chatHistory.scrollHeight;
  }, [chat]);

  return (
    <div
      className={classes.ChatContainer}
      style={{ color: theme.text, background: theme.background }}>
      <div id="chatcontent" className={classes.chatContent}>
        {currentChat}
      </div>
      <ChatInput className={classes.chatInput} />
    </div>
  );
}
