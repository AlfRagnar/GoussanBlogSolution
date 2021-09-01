import React from "react";

export default function ConfirmPage(props) {
  var token = props.match.params.token;

  return (
    <>
      <div>Your token is: {token}</div>
      <div>
        This is where the Confirmation Logic will be ran. Will most likely just
        be an API POST request with the token
      </div>
    </>
  );
}
