import React, { createContext, useState } from "react";

export const TodolistContext = createContext();

const TodolistContextProvider = ({ children }) => {
  const [todos, setTodos] = useState([
    { text: "Plan the family trip", id: 1 },
    { text: "Go shopping for dinner", id: 2 },
    { text: "Go for a walk", id: 3 },
  ]);

  return (
    <TodolistContext.Provider value={{ todos, setTodos }}>
      {children}
    </TodolistContext.Provider>
  );
};

export default TodolistContextProvider;
