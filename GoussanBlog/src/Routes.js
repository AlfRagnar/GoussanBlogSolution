import React from "react";
import { Route, Switch } from "react-router-dom";
import NotFound from "./components/NotFound";
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
        <Route path="/confirm/:token" component={ConfirmPage} />
        <Route>
          <NotFound />
        </Route>
      </Switch>
    </>
  );
}
