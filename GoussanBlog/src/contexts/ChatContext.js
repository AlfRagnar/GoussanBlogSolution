import React, { createContext, useEffect, useRef, useState } from "react";
import config from "../config";
import { HubConnectionBuilder } from "@microsoft/signalr";
import axios from "axios";

export const ChatContext = createContext();

const ChatContextProvider = ({ children }) => {
  const [connection, setConnection] = useState(null);
  const [chat, setChat] = useState([]);
  const latestChat = useRef(null);
  const endpoint = config.chatEndpoint;
  latestChat.current = chat;

  useEffect(() => {
    console.log("Creating Chathub Connection");
    const newConnection = new HubConnectionBuilder()
      .withUrl(endpoint + "/chathub")
      .withAutomaticReconnect()
      .build();
    setConnection(newConnection);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected to Goussanjarga Chat Hub!");

          axios.get("/chat/history").then((res) => {
            if (res.data !== null) {
              const reversed = res.data.reverse();
              const updateChat = [...latestChat.current];
              for (var i = 0; i < reversed.length; i++) {
                const chatMessage = {
                  user: reversed[i].user,
                  message: reversed[i].message,
                };
                updateChat.push(chatMessage);
              }
              setChat(updateChat);
            }
          });

          connection.on("ReceiveMessage", (message) => {
            const updateChat = [...latestChat.current];
            updateChat.push(message);
            setChat(updateChat);
          });
        })
        .catch((e) => console.log("Connection failed: ", e));
    }
  }, [connection]);

  const sendMessage = async (user, message) => {
    const chatMessage = {
      user: user,
      message: message,
    };

    if (connection.connectionStarted) {
      try {
        await connection.send("SendMessage", chatMessage);
      } catch (error) {
        console.log(error);
      }
    } else {
      console.log("No Connection to server yet");
    }
  };
  return (
    <ChatContext.Provider value={{ connection, chat, sendMessage }}>
      {children}
    </ChatContext.Provider>
  );
};

export default ChatContextProvider;
