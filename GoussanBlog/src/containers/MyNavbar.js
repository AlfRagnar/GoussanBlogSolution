import React, { useContext, useState } from "react";
import { useHistory } from "react-router-dom";
import { Nav } from "react-bootstrap";
import {
  Button,
  Toolbar,
  AppBar,
  makeStyles,
  Typography,
  IconButton,
  Fab,
  Menu,
  MenuItem,
  FormGroup,
  FormControlLabel,
} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import KeyboardArrowUpIcon from "@material-ui/icons/KeyboardArrowUp";
import Switch from "@material-ui/core/Switch";
import { ThemeContext } from "../contexts/ThemeContext";
import { AuthContext } from "../contexts/AuthContext";
import UploadImage from "../components/Media/UploadImage";
import UploadVideo from "../components/Media/UploadVideo";
import Register from "../components/Register";
import Login from "../components/Login";
import ScrollTop from "../components/Utilities/ScrollTop";

const useStyles = makeStyles((theme) => ({
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
}));

export default function MyNavbar() {
  const { isDarkTheme, darkTheme, lightTheme, setTheme } =
    useContext(ThemeContext);
  const { auth, changeAuthStatus } = useContext(AuthContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const [anchorEl, setAnchorEl] = useState(null);
  const open = Boolean(anchorEl);
  const history = useHistory();

  const handleHome = () => {
    history.push("/");
  };

  const handleChange = (event) => {
    setTheme(event.target.checked);
  };

  const handleMenu = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const classes = useStyles();
  return (
    <>
      <AppBar
        style={{
          color: currentTheme.text,
          background: currentTheme.background,
        }}>
        <Toolbar>
          {/* MENU BUTTON  */}
          <IconButton
            edge="start"
            className={classes.menuButton}
            color="inherit"
            aria-label="menu"
            onClick={handleMenu}>
            <MenuIcon />
          </IconButton>
          <Menu
            id="menu-appbar"
            color="inherit"
            anchorEl={anchorEl}
            anchorOrigin={{
              vertical: "top",
              horizontal: "right",
            }}
            keepMounted
            transformOrigin={{
              vertical: "top",
              horizontal: "right",
            }}
            open={open}
            onClose={handleClose}>
            {/*  MENU ITEMS */}

            <MenuItem>
              <FormGroup>
                <FormControlLabel
                  control={
                    <Switch
                      checked={isDarkTheme}
                      onChange={handleChange}
                      aria-label="login switch"
                    />
                  }
                  label={isDarkTheme ? "Dark Theme" : "Light Theme"}
                />
              </FormGroup>
            </MenuItem>
          </Menu>
          <Typography variant="h6" className={classes.title}>
            <Button color="inherit" size="large" onClick={handleHome}>
              Goussanjarga Media Website
            </Button>
          </Typography>
          <Nav activeKey={window.location.pathname}>
            {auth ? (
              <>
                <UploadImage />
                <UploadVideo />
                <Button onClick={changeAuthStatus} color="inherit">
                  Logout
                </Button>
              </>
            ) : (
              <>
                <Register />
                <Login />
              </>
            )}
          </Nav>
        </Toolbar>
      </AppBar>
      <Toolbar id="back-to-top-anchor" />
      <ScrollTop>
        <Fab color="secondary" size="small" aria-label="scroll back to top">
          <KeyboardArrowUpIcon />
        </Fab>
      </ScrollTop>
    </>
  );
}
