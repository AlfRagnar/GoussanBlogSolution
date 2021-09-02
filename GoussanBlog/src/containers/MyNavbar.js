import React, { useContext, useState } from "react";
import { useHistory } from "react-router-dom";
import { Nav } from "react-bootstrap";
import { ThemeContext } from "../contexts/ThemeContext";
import { AuthContext } from "../contexts/AuthContext";
import {
  Button,
  Toolbar,
  AppBar,
  makeStyles,
  Typography,
  IconButton,
  Zoom,
  Fab,
  useScrollTrigger,
  Menu,
  MenuItem,
  FormGroup,
  FormControlLabel,
} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import KeyboardArrowUpIcon from "@material-ui/icons/KeyboardArrowUp";
import Switch from "@material-ui/core/Switch";
import Login from "../components/Login";
import Register from "../components/Register";
import Upload from "../components/Upload";

const useStyles = makeStyles((theme) => ({
  root: {
    position: "fixed",
    bottom: theme.spacing(2),
    right: theme.spacing(2),
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
  offset: theme.mixins.toolbar,
}));

function ScrollTop(props) {
  const { children } = props;
  const classes = useStyles();

  const trigger = useScrollTrigger();

  const handleClick = (event) => {
    const anchor = (event.target.ownerDocument || document).querySelector(
      "#back-to-top-anchor"
    );

    if (anchor) {
      anchor.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  };

  return (
    <Zoom in={trigger}>
      <div onClick={handleClick} role="presentation" className={classes.root}>
        {children}
      </div>
    </Zoom>
  );
}

export default function MyNavbar(props) {
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
                <Upload />
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
      <ScrollTop {...props}>
        <Fab color="secondary" size="small" aria-label="scroll back to top">
          <KeyboardArrowUpIcon />
        </Fab>
      </ScrollTop>
      <div className={classes.offset} />
    </>
  );
}
