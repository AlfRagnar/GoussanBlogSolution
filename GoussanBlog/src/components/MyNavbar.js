import React from "react";
import { Nav } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { ThemeContext } from "../contexts/ThemeContext";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Button from "@material-ui/core/Button";

class MyNavbar extends React.Component {
  render() {
    return (
      <ThemeContext.Consumer>
        {(context) => {
          const { isDarkTheme, darkTheme, lightTheme } = context;
          const theme = isDarkTheme ? darkTheme : lightTheme;

          return (
            <AppBar position="static" style={{ background: theme.background }}>
              <Toolbar>
                <LinkContainer style={{ color: theme.text }} to="/">
                  <Button color="inherit" variant="text" size="large">
                    Goussanjarga Media Website
                  </Button>
                </LinkContainer>
                <div className="justify-content-end">
                  <Nav activeKey={window.location.pathname}>
                    <LinkContainer style={{ color: theme.text }} to="/register">
                      <Button color="inherit">Register</Button>
                    </LinkContainer>
                    <LinkContainer style={{ color: theme.text }} to="/login">
                      <Button color="inherit">Login</Button>
                    </LinkContainer>
                  </Nav>
                </div>
              </Toolbar>
            </AppBar>
          );
        }}
      </ThemeContext.Consumer>
    );
  }
}

export default MyNavbar;
