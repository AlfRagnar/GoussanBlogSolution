import React, { createContext, useState } from "react";

export const AuthContext = createContext();

const AuthContextProvider = ({ children }) => {
  const [auth, setAuth] = useState(false);
  const [user, setUser] = useState("");
  const [token, setToken] = useState("");

  const changeAuthStatus = () => {
    setAuth(!auth);
    sessionStorage.clear();
  };

  return (
    <AuthContext.Provider
      value={{
        auth,
        token,
        changeAuthStatus,
        setAuth,
        setToken,
        user,
        setUser,
      }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContextProvider;
