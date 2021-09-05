import React from "react";
import Routes from "./Routes";
import ThemeContextProvider from "./contexts/ThemeContext";
import AuthContextProvider from "./contexts/AuthContext";
import MediaContextProvider from "./contexts/MediaContext";

function App() {
  return (
    <>
      <AuthContextProvider>
        <ThemeContextProvider>
          <MediaContextProvider>
            <Routes />
          </MediaContextProvider>
        </ThemeContextProvider>
      </AuthContextProvider>
    </>
  );
}

export default App;
