import React, { useContext, useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import { Typography } from "@material-ui/core";
import { Skeleton } from "@material-ui/lab";
import { AuthContext } from "../contexts/AuthContext";
import { ThemeContext } from "../contexts/ThemeContext";
import { MediaContext } from "../contexts/MediaContext";
import ViewJWTButton from "../components/Utilities/ViewJWTButton";
import RenderVideos from "../components/Media/RenderVideos";
import RenderImages from "../components/Media/RenderImages";

export default function Home() {
  const { auth, setAuth, setToken, setUser } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const { fetchedVideos, fetchedImages, images, videos } =
    useContext(MediaContext);

  const [loading, isLoading] = useState(true);

  useEffect(() => {
    var localToken = sessionStorage.getItem("authToken");
    var localUser = sessionStorage.getItem("user");
    if (localToken !== null) {
      setToken(localToken);
      setAuth(true);
    }
    if (localUser !== null) {
      setUser(localUser);
    }

    setTimeout(() => {
      isLoading(false);
    }, 2000);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className="container-fluid" style={{ marginTop: 10 }}>
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
              <>
                <Typography>Trying to get Videos</Typography>
                <Skeleton variant="text" />
                <Skeleton variant="circle" />
                <Skeleton variant="rect" />
              </>
            )}
            {fetchedImages && images.length > 0 ? (
              <RenderImages />
            ) : (
              <>
                <Typography>Trying to get Images</Typography>
                <Skeleton variant="text" />
                <Skeleton variant="circle" />
                <Skeleton variant="rect" />
              </>
            )}
          </>
        )}
      </Paper>
    </div>
  );
}
