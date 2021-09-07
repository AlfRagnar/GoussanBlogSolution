import { makeStyles, Paper, Typography } from "@material-ui/core";
import { useContext } from "react";
import { ThemeContext } from "../../contexts/ThemeContext";

const useStyles = makeStyles((theme) => ({
  root: {
    position: "fixed",
    width: "25%",
    margin: theme.spacing(1),
  },
  paper: {
    border: "2px solid #000",
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
  chatBorder: {
    border: "2px solid #000",
    height: "73vh",
  },
}));

export default function ChatWrapper({ children }) {
  const classes = useStyles();
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  return (
    <div className={classes.root}>
      <Paper
        className={classes.paper}
        style={{ color: theme.text, background: theme.background }}>
        <Typography
          variant="h6"
          style={{ color: theme.text, background: theme.background }}>
          Goussanjarga Real-Time Chat
        </Typography>
        <Typography variant="body2">
          Here you can leave feedback, a greeting or just test out the different
          functionality that you will find around on this website.
        </Typography>
        <div className={classes.chatBorder}>{children}</div>
      </Paper>
    </div>
  );
}
