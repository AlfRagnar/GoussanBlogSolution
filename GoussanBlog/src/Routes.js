import React from "react";
import { Route, Switch } from "react-router-dom";
import Login from "./components/Login";
import NotFound from "./components/NotFound";
import Register from "./components/Register";
import ConfirmPage from "./components/ConfirmPage";
import Home from "./containers/Home";
import MyNavbar from "./containers/MyNavbar";

export default function Routes() {
  return (
    <>
      <MyNavbar />
      <Switch>
        <Route exact path="/">
          <Home />
        </Route>
        <Route exact path="/login">
          <Login />
        </Route>
        <Route exact path="/register">
          <Register />
        </Route>
        <Route exact path="/confirm">
          <ConfirmPage />
        </Route>
        <Route>
          <NotFound />
        </Route>
      </Switch>
    </>
  );
}
