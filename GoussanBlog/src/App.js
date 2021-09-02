import React from "react";
import Routes from "./Routes";
import ThemeContextProvider from "./contexts/ThemeContext";
import AuthContextProvider from "./contexts/AuthContext";
import VideoContextProvider from "./contexts/VideoContext";

function App() {
  return (
    <>
      <AuthContextProvider>
        <ThemeContextProvider>
          <VideoContextProvider>
            <Routes />
          </VideoContextProvider>
        </ThemeContextProvider>
      </AuthContextProvider>
    </>
  );
}

export default App;
