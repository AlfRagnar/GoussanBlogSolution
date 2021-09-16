import React, { useEffect, useRef, useState } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import config from "../config";
import ChatWindow from "./ChatWindow";
import ChatInput from "./ChatInput";

export default function Chat() {
  const [connection, setConnection] = useState(null);
  const [chat, setChat] = useState([]);
  const latestChat = useRef(null);
  const endpoint = config.defaultEndpoint;

  latestChat.current = chat;

  useEffect(() => {
    console.log("Setting Connection");
    const newConnection = new HubConnectionBuilder()
      .withUrl(endpoint + "/chathub")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, [endpoint]);

  useEffect(() => {
    console.log("Checking connection");
    if (connection) {
      connection
        .start()
        .then((result) => {
          console.log("Connected to Goussanjarga Chat Hub!");

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
    <>
      <ChatWindow chat={chat} />
      <hr />
      <ChatInput sendMessage={sendMessage} />
    </>
  );
}
