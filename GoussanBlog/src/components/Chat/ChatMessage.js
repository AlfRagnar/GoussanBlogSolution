import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
  message: {
    flexDirection: "flex-end",
    overflow: "auto",
  },
}));

export default function ChatMessage(props) {
  const classes = useStyles();
  return (
    <Typography className={classes.message}>
      <strong>{props.user}</strong>: {props.message}
    </Typography>
  );
}
