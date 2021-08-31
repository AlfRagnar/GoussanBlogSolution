import React, { createContext, useState } from "react";

export const AuthContext = createContext();

const AuthContextProvider = ({ children }) => {
  const [auth, setAuth] = useState(false);
  const [token, setToken] = useState("");

  const changeAuthStatus = () => {
    setAuth(!auth);
  };

  return (
    <AuthContext.Provider
      value={{ auth, token, changeAuthStatus, setAuth, setToken }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContextProvider;
