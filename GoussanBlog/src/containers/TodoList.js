import React, { useContext } from "react";
import { ThemeContext } from "../contexts/ThemeContext";
import { TodolistContext } from "../contexts/TodolistContext";

export default function TodoList() {
  const { todos } = useContext(TodolistContext);
  const { isDarkTheme, lightTheme, darkTheme, changeTheme } =
    useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  return (
    <div
      className="container"
      style={{
        background: theme.background,
        color: theme.text,
        height: "140px",
        textAlign: "center",
      }}>
      {todos.length ? (
        todos.map((todo) => {
          return (
            <p key={todo.id} className="item">
              {todo.text}
            </p>
          );
        })
      ) : (
        <div>You have no Todos</div>
      )}
      <button className="ui button primary" onClick={changeTheme}>
        Change the theme
      </button>
    </div>
  );
}
