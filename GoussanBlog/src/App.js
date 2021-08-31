import React from "react";
import Routes from "./Routes";
import ThemeContextProvider from "./contexts/ThemeContext";
import MyNavbar from "./components/MyNavbar";
import AuthContextProvider from "./contexts/AuthContext";

function App() {
  return (
    <div>
      <AuthContextProvider>
        <ThemeContextProvider>
          <MyNavbar />
          <Routes />
        </ThemeContextProvider>
      </AuthContextProvider>
    </div>
  );
}

export default App;
