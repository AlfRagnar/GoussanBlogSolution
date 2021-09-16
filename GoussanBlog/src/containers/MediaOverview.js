import React, { useContext, useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import { makeStyles, Typography, Grid } from "@material-ui/core";
import { Skeleton } from "@material-ui/lab";
import { AuthContext } from "../contexts/AuthContext";
import { ThemeContext } from "../contexts/ThemeContext";
import { MediaContext } from "../contexts/MediaContext";
import VideoTable from "../components/Media/VideoTable";
import ImageTable from "../components/Media/ImageTable";
import BlogTable from "../components/Media/BlogTable";

export default function MediaOverview() {
  const { setAuth, setToken, setUser } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const {
    fetchedVideos,
    fetchedImages,
    fetchedBlogs,
    images,
    allVideos,
    blogs,
  } = useContext(MediaContext);

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
    console.log("Getting user Token and User ID");
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
          <Grid
            container
            direction="row"
            justifyContent="flex-start"
            alignItems="flex-start">
            <Grid item xs={12}>
              <Typography className={classes.bodyText} variant="h2">
                Goussanjarga
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <Typography className={classes.bodyText} variant="body1">
                A Simple Media Sharing Website
              </Typography>
            </Grid>

            {fetchedVideos && allVideos.length > 0 ? (
              <Grid item xs={12}>
                <VideoTable />
              </Grid>
            ) : (
              <Grid item xs={12}>
                <Typography className={classes.bodyText}>
                  Trying to get Videos
                </Typography>
                <Skeleton variant="text" />
                <Skeleton variant="circle" />
                <Skeleton variant="rect" />
              </Grid>
            )}
            {fetchedImages && images.length > 0 ? (
              <Grid item xs={12}>
                <ImageTable />
              </Grid>
            ) : (
              <Grid item xs={12}>
                <Typography className={classes.bodyText}>
                  Trying to get Images
                </Typography>
                <Skeleton variant="text" />
                <Skeleton variant="circle" />
                <Skeleton variant="rect" />
              </Grid>
            )}

            {fetchedBlogs && blogs.length > 0 ? (
              <Grid item xs={12}>
                <BlogTable />
              </Grid>
            ) : (
              <Grid item xs={12}>
                <Typography className={classes.bodyText}>
                  Trying to get Blogs
                </Typography>
                <Skeleton variant="text" />
                <Skeleton variant="circle" />
                <Skeleton variant="rect" />
              </Grid>
            )}
          </Grid>
        </>
      )}
    </Paper>
  );
}
