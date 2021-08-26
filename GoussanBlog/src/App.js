import React, { useState } from "react";
import "./App.css";
import Routes from "./Routes";
import { AppContext } from "./contexts/contextLib";
import ThemeContextProvider from "./contexts/ThemeContext";
import MyNavbar from "./components/MyNavbar";
import { Container } from "@material-ui/core";

function App() {
  const [isAuthenticated, userHasAuthenticated] = useState({
    auth: false,
    token: "",
  });

  return (
    <Container className="App">
      <ThemeContextProvider>
        <MyNavbar />
        <AppContext.Provider value={{ isAuthenticated, userHasAuthenticated }}>
          <Routes />
        </AppContext.Provider>
      </ThemeContextProvider>
    </Container>
  );
}

export default App;
