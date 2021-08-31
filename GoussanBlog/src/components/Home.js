import React, { useContext, useState } from "react";
import { ThemeContext } from "../contexts/ThemeContext";
import Paper from "@material-ui/core/Paper";
import { ToggleButton, ToggleButtonGroup } from "react-bootstrap";
import { AuthContext } from "../contexts/AuthContext";
import {
  Button,
  DialogTitle,
  DialogContentText,
  Dialog,
  Typography,
} from "@material-ui/core";

export default function Home() {
  const { isDarkTheme, darkTheme, lightTheme, setTheme } =
    useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const { auth, token } = useContext(AuthContext);
  const [open, setOpen] = useState(false);

  const handleClose = () => {
    setOpen(false);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  return (
    <div className="container">
      <Paper
        variant="outlined"
        style={{
          background: theme.background,
          color: theme.text,
          textAlign: "center",
        }}>
        <div className="lander container-fluid">
          <h1>Goussanjarga</h1>
          <p>A simple media sharing App</p>
          {auth ? (
            <>
              <Button
                variant="contained"
                color="primary"
                onClick={handleClickOpen}>
                View My Token
              </Button>
              <Dialog onClose={handleClose} open={open}>
                <DialogTitle id="dialog-title">View my JWT token</DialogTitle>
                <DialogContentText>{token}</DialogContentText>
              </Dialog>
            </>
          ) : (
            <>
              <Typography>You're not logged in</Typography>
            </>
          )}
        </div>
      </Paper>

      <footer>
        <ToggleButtonGroup type="checkbox">
          <ToggleButton
            variant="outlined"
            style={{
              background: darkTheme.background,
              color: darkTheme.text,
            }}
            id="tbg-btn-1"
            onClick={() => setTheme(true)}>
            Dark Theme
          </ToggleButton>
          <ToggleButton
            variant="outlined"
            style={{
              background: lightTheme.background,
              color: lightTheme.text,
            }}
            id="tbg-btn-2"
            onClick={() => setTheme(false)}>
            Light Theme
          </ToggleButton>
        </ToggleButtonGroup>
      </footer>
    </div>
  );
}
