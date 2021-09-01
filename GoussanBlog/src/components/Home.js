import React, { useContext, useEffect, useState } from "react";
import { ThemeContext } from "../contexts/ThemeContext";
import Paper from "@material-ui/core/Paper";
import { AuthContext } from "../contexts/AuthContext";
import {
  Button,
  DialogTitle,
  DialogContentText,
  Dialog,
  Typography,
  Card,
  CardHeader,
  CardContent,
} from "@material-ui/core";
import axios from "axios";
import { AzureMP } from "react-azure-mp";

export default function Home() {
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const { auth, token, setAuth, setToken } = useContext(AuthContext);
  const [open, setOpen] = useState(false);
  const [videos, setVideos] = useState([""]);
  const [fetchedVideos, setFetchedVideos] = useState(false);

  const handleClose = () => {
    setOpen(false);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  useEffect(() => {
    var localToken = sessionStorage.getItem("authToken");
    if (localToken !== null) {
      setToken(localToken);
      setAuth(true);
    }

    async function fetchVideos() {
      try {
        await axios.get("/videos").then((res) => {
          setVideos(res.data);
          setFetchedVideos(true);
        });
      } catch (err) {
        setVideos("");
        setFetchedVideos(false);
      }
    }
    fetchVideos();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className="container">
      <Paper
        variant="outlined"
        style={{
          background: theme.background,
          color: theme.text,
          textAlign: "center",
        }}>
        <div className="lander container-fluid">
          <Typography style={{ color: theme.text }} variant="h2">
            Goussanjarga
          </Typography>
          <Typography style={{ color: theme.text }} variant="body2">
            A Simple Media Sharing Website
          </Typography>
          {auth ? (
            <>
              <Button
                variant="contained"
                color="primary"
                onClick={handleClickOpen}>
                View My Token
              </Button>
              <Dialog onClose={handleClose} open={open}>
                <DialogTitle id="dialog-title">Your JWT Token</DialogTitle>
                <div className="container">
                  <DialogContentText>{token}</DialogContentText>
                </div>
              </Dialog>
            </>
          ) : (
            <>
              <Typography>You're not logged in</Typography>
            </>
          )}
        </div>

        {fetchedVideos ? (
          <>
            {videos.map((video) => (
              <Card
                className="container"
                style={{
                  background: theme.background,
                  color: theme.text,
                  textAlign: "center",
                }}
                key={video.id}>
                <CardHeader title={video.title} />
                <CardContent>
                  <Typography variant="body2">
                    Video Description: <br />
                    {video.description}
                  </Typography>

                  <AzureMP
                    skin="amp-flush"
                    options={{
                      fluid: true,
                    }}
                    src={[
                      {
                        src: video.streamingPaths[0],
                        type: "application/x-mpegURL",
                      },
                    ]}
                  />
                </CardContent>
              </Card>
            ))}
          </>
        ) : (
          <Typography>Getting Video Data from Database</Typography>
        )}
      </Paper>
    </div>
  );
}
