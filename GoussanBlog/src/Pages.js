import React from "react";
import { Route, Routes } from "react-router";
import Home from "./containers/Home";
import MyNavbar from "./containers/MyNavbar";
import ChatHub from "./containers/ChatHub";
import { Grid } from "@material-ui/core";
import ConfirmPage from "./components/Utilities/ConfirmPage";
import SimpleBar from "simplebar-react";
import "simplebar/dist/simplebar.min.css";
import MediaOverview from "./containers/MediaOverview";

export default function Pages() {
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
          <Routes>
            <Route exact path="/" element={<Home />} />
            <Route path="/overview" component={<MediaOverview />} />
            <Route path="/confirm/:token" component={<ConfirmPage />} />
          </Routes>
        </Grid>
      </Grid>
    </SimpleBar>
  );
}
