const initState = {
  theme: "dark mode",
};

// eslint-disable-next-line import/no-anonymous-default-export
export default (state = initState, action) => {
  switch (action.type) {
    case "CHANGE_THEME": {
      return { theme: action.theme };
    }
    default: {
      return state;
    }
  }
};
