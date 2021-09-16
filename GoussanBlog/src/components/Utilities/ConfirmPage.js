import { Typography } from "@material-ui/core";
import { Skeleton } from "@material-ui/lab";
import axios from "axios";
import React, { useState, useEffect } from "react";

export default function ConfirmPage(props) {
  var token = props.match.params.token;
  const [loading, isLoading] = useState(true);
  const [activateStatus, setActivateStatus] = useState(false);

  const activateUser = async () => {
    try {
      await axios
        .post("/user/activate", null, {
          params: {
            token,
          },
        })
        .then((res) => {
          if (res.status === 400) {
            setActivateStatus(false);
          } else {
            setActivateStatus(true);
          }
        });
    } catch (err) {}
  };

  useEffect(() => {
    console.log("Activating user");
    activateUser();

    setTimeout(() => {
      isLoading(false);
    }, 2000);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <>
      {loading ? (
        <>
          <Skeleton variant="text" />
          <Skeleton variant="circle" width={40} height={40} />
          <Skeleton variant="rect" width={210} height={118} />
        </>
      ) : (
        <>
          {activateStatus ? (
            <>
              <Typography variant="body1">
                Your Account have been activated. You can now Login!
              </Typography>
            </>
          ) : (
            <>
              <Typography variant="body1">
                Failed to activate your account, check your token
              </Typography>
            </>
          )}
        </>
      )}
    </>
  );
}
