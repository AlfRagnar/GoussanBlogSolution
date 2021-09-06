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
import { ThemeContext } from "../contexts/ThemeContext";
import { MediaContext } from "../contexts/MediaContext";

const useStyles = makeStyles((theme) => ({
  img: {
    // display: "Block",
    maxWidth: 450,
    maxHeight: 450,
  },
  imgPopup: {
    // display: "block",
    maxWidth: 940,
    maxHeight: 940,
  },
  modal: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  paper: {
    position: "absolute",
    maxWidth: 1050,
    backgroundColor: theme.palette.background.paper,
    border: "2px solid #000",
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
    // overflow: "hidden",
  },
  imageList: {
    flexWrap: "wrap",
    // Promote the list into his own layer on Chrome. This cost memory but helps keeping high FPS.
    transform: "translateZ(0)",
  },
  root: {
    display: "flex",
    flexWrap: "wrap",
    justifyContent: "space-around",
    overflow: "hidden",
  },
}));

export default function RenderImages() {
  const classes = useStyles();
  const { images } = useContext(MediaContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  const [popupImage, setPopupImage] = useState(images[0]);
  const [open, setOpen] = useState(false);

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
            </CardActionArea>
            <ImageListItemBar title={image.title} />
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
          <Paper
            className={classes.paper}
            style={{
              background: theme.background,
              color: theme.text,
            }}>
            <Typography variant="h2" style={{ color: theme.text }}>
              {popupImage.title}
            </Typography>
            <img
              className={classes.imgPopup}
              src={popupImage.storagePath}
              alt={popupImage.title}
            />
            <Typography paragraph style={{ color: theme.text }}>
              {popupImage.description}
            </Typography>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
