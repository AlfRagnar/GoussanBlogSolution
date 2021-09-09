import React from "react";
import { Route, Switch } from "react-router-dom";
import Home from "./containers/Home";
import MyNavbar from "./containers/MyNavbar";
import ChatHub from "./containers/ChatHub";
import { Grid } from "@material-ui/core";
import ConfirmPage from "./components/Utilities/ConfirmPage";
import NotFound from "./components/Utilities/NotFound";
import SimpleBar from "simplebar-react";
import "simplebar/dist/simplebar.min.css";

export default function Routes() {
  return (
    <SimpleBar style={{ flexGrow: 1, maxHeight: "100vh" }}>
      <Grid
        container
        direction="row"
        justifyContent="flex-start"
        alignItems="flex-start">
        <Grid item xs={12}>
          <MyNavbar />
        </Grid>
        <Grid item xs={3}>
          <ChatHub />
        </Grid>
        <Grid item xs={9}>
          <Switch>
            <Route exact path="/">
              <Home />
            </Route>
            <Route path="/confirm/:token" component={ConfirmPage} />
            <Route>
              <NotFound />
            </Route>
          </Switch>
        </Grid>
      </Grid>
    </SimpleBar>
  );
}
