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

export default function RenderImages() {
  const { images } = useContext(MediaContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;
  const [popupImage, setPopupImage] = useState(images[0]);
  const [open, setOpen] = useState(false);

  const useStyles = makeStyles((theme) => ({
    img: {
      // display: "Block",
      maxWidth: "100%",
      maxHeight: "100%",
    },
    imgPopup: {
      // display: "block",
      maxWidth: "100%",
      maxHeight: "100%",
    },
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
    imageTitle: {
      top: "13vh",
    },
  }));

  const classes = useStyles();

  const showImage = (Image) => {
    setPopupImage(Image);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div className={classes.root}>
      <Typography variant="h3">Uploaded Images</Typography>
      <ImageList className={classes.imageList} cols={3}>
        {images.map((image) => (
          <ImageListItem key={image.id + "-list"} cols={image.cols || 1}>
            <CardActionArea onClick={() => showImage(image)}>
              <img
                className={classes.img}
                src={image.storagePath}
                alt={image.title}
              />
              <ImageListItemBar
                className={classes.imageTitle}
                title={image.title}
              />
            </CardActionArea>
          </ImageListItem>
        ))}
      </ImageList>

      <Modal
        id="popupImage"
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        className={classes.modal}
        open={open}
        onClose={handleClose}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}>
        <Fade in={open}>
          <Paper className={classes.paper}>
            <Typography variant="h2" className={classes.bodyText}>
              {popupImage.title}
            </Typography>
            <img
              className={classes.imgPopup}
              src={popupImage.storagePath}
              alt={popupImage.title}
            />
            <Typography className={classes.bodyText} paragraph>
              {popupImage.description}
            </Typography>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
