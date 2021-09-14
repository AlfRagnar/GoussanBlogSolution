import React, { createContext, useState } from "react";

export const AuthContext = createContext();

const AuthContextProvider = ({ children }) => {
  const [auth, setAuth] = useState(false);
  const [user, setUser] = useState("");
  const [token, setToken] = useState("");
  const [FileSizeLimit, setFileSizeLimit] = useState(50);

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
        FileSizeLimit,
        setFileSizeLimit,
      }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContextProvider;
