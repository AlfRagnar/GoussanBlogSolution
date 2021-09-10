import React, { useContext, useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import { makeStyles, Typography } from "@material-ui/core";
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
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const { fetchedVideos, fetchedImages, images, videos } =
    useContext(MediaContext);

  const [loading, isLoading] = useState(true);

  const useStyles = makeStyles((theme) => ({
    root: {},
    paperBody: {
      margin: theme.spacing(1, 1, 2, 2),
      background: currentTheme.background,
      color: currentTheme.text,
      textAlign: "center",
      border: "2px solid #000",
    },
    bodyText: {
      color: currentTheme.text,
    },
  }));
  const classes = useStyles();

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
    <Paper variant="outlined" className={classes.paperBody}>
      {loading ? (
        <>
          <Skeleton variant="text" />
          <Skeleton variant="circle" />
          <Skeleton variant="rect" />
        </>
      ) : (
        <>
          <div className="lander container-fluid">
            <Typography className={classes.bodyText} variant="h2">
              Goussanjarga
            </Typography>
            <Typography className={classes.bodyText} variant="body1">
              A Simple Media Sharing Website
            </Typography>
            <Typography className={classes.bodyText} variant="body2">
              Website is currently under development so any content made or
              uploaded to this website are up for change and any integrity or
              data protection is not guaranteed. Chances are the website
              database and file storage will be wiped multiple times a day.
            </Typography>
            <Typography className={classes.bodyText} variant="body2">
              No File Size Limit set on Video Files yet, might change it the
              future
            </Typography>

            {auth ? (
              <ViewJWTButton />
            ) : (
              <>
                <Typography className={classes.bodyText}>
                  You're not logged in
                </Typography>
              </>
            )}
          </div>

          {fetchedVideos && videos.length > 0 ? (
            <RenderVideos />
          ) : (
            <>
              <Typography className={classes.bodyText}>
                Trying to get Videos
              </Typography>
              <Skeleton variant="text" />
              <Skeleton variant="circle" />
              <Skeleton variant="rect" />
            </>
          )}
          {fetchedImages && images.length > 0 ? (
            <RenderImages />
          ) : (
            <>
              <Typography className={classes.bodyText}>
                Trying to get Images
              </Typography>
              <Skeleton variant="text" />
              <Skeleton variant="circle" />
              <Skeleton variant="rect" />
            </>
          )}
        </>
      )}
    </Paper>
  );
}
