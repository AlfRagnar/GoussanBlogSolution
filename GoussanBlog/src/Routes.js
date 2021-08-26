import React from "react";
import { Route, Switch } from "react-router-dom";
import Home from "./components/Home";
import Login from "./containers/Login";
import NotFound from "./containers/NotFound";
import Register from "./containers/Register";
import ConfirmPage from "./containers/ConfirmPage";

export default function Routes() {
  return (
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
      <Route path="/confirm/:token">
        <ConfirmPage />
      </Route>
      <Route>
        <NotFound />
      </Route>
    </Switch>
  );
}
