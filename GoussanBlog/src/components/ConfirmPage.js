import React from "react";

export default function ConfirmPage(props) {
  var token = props.match.params.token;

  return <div>Your token is: {token}</div>;
}
