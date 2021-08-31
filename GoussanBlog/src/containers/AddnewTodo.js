import React, { useState } from "react";

export default function AddnewTodo({ addTodo }) {
  const [todos, setTodos] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    addTodo(todos);
    setTodos("");
  };
  return (
    <form onSubmit={handleSubmit}>
      <label htmlFor="todo">To do</label>
      <input
        type="text"
        value={todos}
        id="todo"
        onChange={(e) => setTodos(e.target.value)}
      />
      <input type="submit" />
    </form>
  );
}
