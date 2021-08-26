import React from "react";
import { ThemeContext } from "../contexts/ThemeContext";
import Button from "@material-ui/core/Button";
import Paper from "@material-ui/core/Paper";

class Home extends React.Component {
  static contextType = ThemeContext;
  render() {
    const { isDarkTheme, darkTheme, lightTheme, changeTheme } = this.context;
    const theme = isDarkTheme ? darkTheme : lightTheme;

    return (
      <Paper
        variant="outlined"
        style={{
          background: theme.background,
          color: theme.text,
          textAlign: "center",
        }}>
        <div className="lander">
          <h1>Goussanjarga</h1>
          <p>A simple media sharing App</p>
        </div>
        <Button variant="contained" color="primary" onClick={changeTheme}>
          Change Theme
        </Button>
      </Paper>
    );
  }
}

export default Home;
