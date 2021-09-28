import React, { useContext, useState } from "react";
import {
  Typography,
  Modal,
  CardActionArea,
  makeStyles,
  Backdrop,
  Fade,
  Paper,
  ImageList,
  ImageListItem,
  ImageListItemBar,
} from "@material-ui/core";
// Local Modules/Components
import { ThemeContext } from "../../../contexts/ThemeContext";
import { MediaContext } from "../../../contexts/MediaContext";

export default function RenderBlogs() {
  const { blogs } = useContext(MediaContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const [popupBlog, setPopupBlog] = useState(blogs[0]);
  const [open, setOpen] = useState(false);

  const useStyles = makeStyles((theme) => ({
    modal: {
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    },
    paper: {
      position: "absolute",
      maxWidth: "75%",
      border: "2px solid #000",
      boxShadow: theme.shadows[5],
      padding: theme.spacing(2, 4, 3),
      background: currentTheme.background,
      color: currentTheme.text,
    },
    imageList: {
      flexWrap: "wrap",
      // Promote the list into his own layer on Chrome. This cost memory but helps keeping high FPS.
      transform: "translateZ(0)",
    },
    bodyText: {
      color: currentTheme.text,
      background: currentTheme.background,
    },
    root: {
      margin: theme.spacing(1),
    },
    blogTitle: {
      top: "5vh",
    },
  }));
  const classes = useStyles();

  const showBlog = (blog) => {
    setPopupBlog(blog);
    setOpen(true);
  };

  const listBlogs = (blogs) => {
    return blogs.map((blog) => (
      <ImageListItem key={blog.id} cols={blog.cols || 1}>
        <CardActionArea onClick={() => showBlog(blog)}>
          <Typography className={classes.blogDescription}>
            {blog.content}
          </Typography>
          <ImageListItemBar className={classes.blogTitle} title={blog.title} />
        </CardActionArea>
      </ImageListItem>
    ));
  };

  return (
    <div className={classes.root}>
      <Typography variant="h3">Uploaded Blogs</Typography>
      <ImageList className={classes.imageList} cols={3}>
        {listBlogs(blogs)}
      </ImageList>

      <Modal
        id="popupBlog"
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        className={classes.modal}
        open={open}
        onClose={() => setOpen(!open)}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}>
        <Fade in={open}>
          <Paper className={classes.paper}>
            <Typography variant="h2" className={classes.bodyText}>
              {popupBlog.title}
            </Typography>

            <Typography className={classes.bodyText} paragraph>
              {popupBlog.content}
            </Typography>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
