import React, { useState, useContext } from "react";

import {
  Button,
  Dialog,
  DialogTitle,
  DialogContentText,
} from "@material-ui/core";
import { AuthContext } from "../contexts/AuthContext";

export default function ViewJWTButton() {
  const [open, setOpen] = useState(false);
  const { token } = useContext(AuthContext);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <>
      <Button variant="contained" color="primary" onClick={handleClickOpen}>
        View My Token
      </Button>
      <Dialog onClose={handleClose} open={open}>
        <DialogTitle id="dialog-title">Your JWT Token</DialogTitle>
        <div className="container">
          <DialogContentText>{token}</DialogContentText>
        </div>
      </Dialog>
    </>
  );
}
