import React, { useContext, useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import { makeStyles, Typography, Grid } from "@material-ui/core";
import { Skeleton } from "@material-ui/lab";
import { AuthContext } from "../contexts/AuthContext";
import { ThemeContext } from "../contexts/ThemeContext";
import { MediaContext } from "../contexts/MediaContext";
import RenderVideos from "../components/Media/Videos/RenderVideos";
import RenderImages from "../components/Media/Images/RenderImages";
import RenderBlogs from "../components/Media/Blogs/RenderBlogs";

export default function Home() {
  const { setAuth, setToken, setUser, FileSizeLimit } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const {
    fetchedVideos,
    fetchedImages,
    fetchedBlogs,
    images,
    videos,
    blogs,
    fetchAllVideos,
    fetchVideos,
    fetchBlogs,
    fetchImages,
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
    console.log("Home Page Content");
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
    }, 200);
    fetchAllVideos();
    fetchVideos();
    fetchBlogs();
    fetchImages();

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
            <Grid item xs={3}>
              <Typography
                className={classes.bodyText}
                variant="body2"
                paragraph>
                Website is currently under development so any content made or
                uploaded to this website are up for change and any integrity or
                data protection is not guaranteed. Chances are the website
                database and file storage will be wiped multiple times a day.
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <Typography className={classes.bodyText} variant="body2">
                Current File Size Limit for uploading is set to: {FileSizeLimit}{" "}
                MB
              </Typography>
            </Grid>
            <Grid item xs={3}>
              <Typography
                className={classes.bodyText}
                paragraph
                variant="body2">
                Link To Repo:
                <a
                  href="https://github.com/AlfRagnar/GoussanBlogSolution"
                  target="_blank"
                  rel="noreferrer">
                  GitHub
                </a>
              </Typography>
            </Grid>

            {fetchedVideos && videos.length > 0 ? (
              <Grid item xs={12}>
                <RenderVideos />
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
                <RenderImages />
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
                <RenderBlogs />
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
