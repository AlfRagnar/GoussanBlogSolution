import React, { createContext, useContext } from "react";

export const ThemeContext = createContext();

export function useThemeContext() {
  return useContext(ThemeContext);
}
class ThemeContextProvider extends React.Component {
  state = {
    isDarkTheme: true,
    lightTheme: {
      text: "#222222",
      background: "#d8ddf1",
    },
    darkTheme: {
      text: "#ffffff",
      background: "#5c5c5c",
    },
  };

  setTheme = (boolean) => {
    this.setState({ isDarkTheme: boolean });
  };

  changeTheme = () => {
    this.setState({ isDarkTheme: !this.state.isDarkTheme });
  };

  render() {
    return (
      <ThemeContext.Provider
        value={{
          ...this.state,
          changeTheme: this.changeTheme,
          setTheme: this.setTheme,
        }}>
        {this.props.children}
      </ThemeContext.Provider>
    );
  }
}

export default ThemeContextProvider;
