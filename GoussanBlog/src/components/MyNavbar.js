import React, { useContext } from "react";
import { Nav } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { ThemeContext } from "../contexts/ThemeContext";
import { AuthContext } from "../contexts/AuthContext";
import {
  Button,
  Toolbar,
  AppBar,
  makeStyles,
  Typography,
} from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
}));

export default function MyNavbar() {
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const { auth, changeAuthStatus } = useContext(AuthContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;

  const classes = useStyles();
  return (
    <div className={classes.root}>
      <AppBar position="sticky" style={{ background: currentTheme.background }}>
        <Toolbar>
          <Typography variant="h6" className={classes.title}>
            <LinkContainer style={{ color: currentTheme.text }} to="/">
              <Button color="inherit" variant="text" size="large">
                Goussanjarga Media Website
              </Button>
            </LinkContainer>
          </Typography>
          <Nav activeKey={window.location.pathname}>
            <LinkContainer style={{ color: currentTheme.text }} to="/todolist">
              <Button color="inherit">todolist</Button>
            </LinkContainer>
            {auth ? (
              <>
                <Button onClick={changeAuthStatus} color="inherit">
                  Logout
                </Button>
              </>
            ) : (
              <div>
                <LinkContainer
                  style={{ color: currentTheme.text }}
                  to="/register">
                  <Button color="inherit">Register</Button>
                </LinkContainer>
                <LinkContainer style={{ color: currentTheme.text }} to="/login">
                  <Button color="inherit">Login</Button>
                </LinkContainer>
              </div>
            )}
          </Nav>
        </Toolbar>
      </AppBar>
    </div>
  );
}
