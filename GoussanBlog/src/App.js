import React from "react";
import Pages from "./Pages";
import ThemeContextProvider from "./contexts/ThemeContext";
import AuthContextProvider from "./contexts/AuthContext";
import MediaContextProvider from "./contexts/MediaContext";
import ChatContextProvider from "./contexts/ChatContext";

function App() {
  return (
    <>
      <AuthContextProvider>
        <ThemeContextProvider>
          <MediaContextProvider>
            <ChatContextProvider>
              <Pages />
            </ChatContextProvider>
          </MediaContextProvider>
        </ThemeContextProvider>
      </AuthContextProvider>
    </>
  );
}

export default App;
