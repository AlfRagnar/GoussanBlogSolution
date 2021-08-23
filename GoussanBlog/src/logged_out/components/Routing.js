import React, { memo } from "react";
import { Switch } from "react-router-dom";
import PropTypes from "prop-types";
import { Button } from "@material-ui/core";

function Routing(props) {
  const {} = props;

  return (
    <Switch>
      <Button>Home Button</Button>
    </Switch>
  );
}

export default memo(Routing);
