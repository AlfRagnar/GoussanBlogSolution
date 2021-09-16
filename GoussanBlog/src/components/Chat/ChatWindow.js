import { makeStyles } from "@material-ui/core";
import { createRef, useContext, useEffect } from "react";
import { ChatContext } from "../../contexts/ChatContext";
import { ThemeContext } from "../../contexts/ThemeContext";
import ChatInput from "./ChatInput";
import ChatMessage from "./ChatMessage";
import SimpleBar from "simplebar-react";
import "simplebar/dist/simplebar.min.css";

const useStyles = makeStyles((theme) => ({
  chatContent: {
    margin: theme.spacing(1),
    height: "57vh",
    flexDirection: "flex-end",
    overflowX: "auto",
  },
}));

export default function ChatWindow() {
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const classes = useStyles();
  const { chat } = useContext(ChatContext);
  const scrollableNodeRef = createRef();

  const currentChat = chat.map((m) => (
    <ChatMessage
      key={Date.now() * Math.random()}
      user={m.user}
      message={m.message}
    />
  ));

  useEffect(() => {
    console.log("Getting chat content");
    var chatHistory = document.getElementById("chatcontent");
    scrollableNodeRef.current.scrollTop = chatHistory.scrollHeight;
  }, [chat, scrollableNodeRef]);

  return (
    <div style={{ color: theme.text, background: theme.background }}>
      <SimpleBar
        scrollableNodeProps={{ ref: scrollableNodeRef }}
        id="chatcontent"
        className={classes.chatContent}>
        {currentChat}
      </SimpleBar>
      <ChatInput />
    </div>
  );
}
