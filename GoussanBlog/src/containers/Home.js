import React, { useContext, useEffect, useState } from "react";
import { ThemeContext } from "../contexts/ThemeContext";
import Paper from "@material-ui/core/Paper";
import { AuthContext } from "../contexts/AuthContext";
import { Typography } from "@material-ui/core";
import axios from "axios";
import { Skeleton } from "@material-ui/lab";
import ViewJWTButton from "../components/ViewJWTButton";
import { MediaContext } from "../contexts/MediaContext";
import RenderVideos from "../components/RenderVideos";
import RenderImages from "../components/RenderImages";

export default function Home() {
  const { auth, setAuth, setToken } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const {
    setVideos,
    setImages,
    setFetchedVideos,
    setFetchedImages,
    fetchedVideos,
    fetchedImages,
    images,
    videos,
  } = useContext(MediaContext);

  const [loading, isLoading] = useState(true);

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

    async function fetchImages() {
      try {
        await axios
          .get("/Image")
          .then((res) => {
            setImages(res.data);
            setFetchedImages(true);
          })
          .catch((e) => {
            console.log(e);
          });
      } catch (err) {
        console.log(err);
        setVideos("");
        setFetchedVideos(false);
      }
    }
    fetchVideos();
    fetchImages();

    setTimeout(() => {
      isLoading(false);
    }, 2000);

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
        {loading ? (
          <>
            <Skeleton variant="text" />
            <Skeleton variant="circle" />
            <Skeleton variant="rect" />
          </>
        ) : (
          <>
            <div className="lander container-fluid">
              <Typography style={{ color: theme.text }} variant="h2">
                Goussanjarga
              </Typography>
              <Typography style={{ color: theme.text }} variant="body1">
                A Simple Media Sharing Website
              </Typography>
              <Typography variant="body2">
                Website is currently under development so any content made or
                uploaded to this website are up for change and any integrity or
                data protection is not guaranteed. Chances are the website
                database and file storage will be wiped multiple times a day.
              </Typography>
              <Typography variant="body2">
                Current Upload limit size per File: <em>30MB</em>
              </Typography>

              {auth ? (
                <ViewJWTButton />
              ) : (
                <>
                  <Typography>You're not logged in</Typography>
                </>
              )}
            </div>

            {fetchedVideos && videos.length > 0 ? (
              <RenderVideos />
            ) : (
              <Typography variant="body2">No Video Data in Database</Typography>
            )}
            {fetchedImages && images.length > 0 ? (
              <RenderImages />
            ) : (
              <Typography variant="body2">No Image Data in Database</Typography>
            )}
          </>
        )}
      </Paper>
    </div>
  );
}
