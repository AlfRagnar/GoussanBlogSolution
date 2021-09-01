import React from "react";
import Routes from "./Routes";
import ThemeContextProvider from "./contexts/ThemeContext";
import AuthContextProvider from "./contexts/AuthContext";

function App() {
  return (
    <>
      <AuthContextProvider>
        <ThemeContextProvider>
          <Routes />
        </ThemeContextProvider>
      </AuthContextProvider>
    </>
  );
}

export default App;
