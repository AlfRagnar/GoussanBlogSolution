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
import { Replay } from "vimond-replay";
import "vimond-replay/index.css";
import HlsjsVideoStreamer from "vimond-replay/video-streamer/hlsjs";

const useStyles = makeStyles((theme) => ({
  modal: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  paper: {
    position: "absolute",
    width: 400,
    backgroundColor: theme.palette.background.paper,
    border: "2px solid #000",
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
  imageList: {
    flexWrap: "wrap",
    // Promote the list into his own layer on Chrome. This cost memory but helps keeping high FPS.
    transform: "translateZ(0)",
  },
  root: {
    // display: "flex",
    flexWrap: "wrap",
    justifyContent: "space-around",
    overflow: "hidden",
  },
}));

export default function RenderVideos() {
  const classes = useStyles();
  const { videos } = useContext(MediaContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  const [popupVideo, setPopupVideo] = useState(videos[0]);
  const [open, setOpen] = useState(false);

  const showVideo = (video) => {
    setPopupVideo(video);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const RemoveControlBar = () => {
    var controlbar = document.querySelectorAll(".replay-controls-bar");
    controlbar.forEach((el) => {
      el.style.visibility = "hidden";
    });
  };

  const replayOptions = {
    controls: {
      includeControls: [],
    },
    videoStreamer: {
      hlsjs: {
        customConfiguration: {
          capLevelToPlayerSize: true,
          maxBufferLength: 20,
          maxBufferSize: 1,
        },
      },
    },
  };
  const replayPopUp = {
    controls: {
      includeControls: [
        "playPauseButton",
        "timeline",
        "timeDisplay",
        "volume",
        "fullscreenButton",
        "qualitySelector",
        "bufferingIndicator",
      ],
    },
    videoStreamer: {
      hlsjs: {
        customConfiguration: {
          capLevelToPlayerSize: true,
          maxBufferLength: 30,
          maxBufferSize: 10,
        },
      },
    },
  };

  return (
    <div className={classes.root}>
      <Typography variant="h3">Uploaded Videos</Typography>
      <ImageList className={classes.imageList} cols={4}>
        {videos.map((video) => (
          <ImageListItem key={video.id + "-list"} cols={video.cols || 1}>
            <CardActionArea onClick={() => showVideo(video)}>
              <Replay
                onPlaybackActionsReady={() => RemoveControlBar()}
                initialPlaybackProps={{ isPaused: true }}
                source={video.streamingPaths[0]}
                options={replayOptions}>
                <HlsjsVideoStreamer />
              </Replay>
            </CardActionArea>
            <ImageListItemBar title={video.title} />
          </ImageListItem>
        ))}
      </ImageList>

      <Modal
        id="popupVideo"
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
            className="container"
            style={{
              background: theme.background,
              color: theme.text,
              textAlign: "center",
            }}>
            <Typography variant="h2" style={{ color: theme.text }}>
              {popupVideo.title}
            </Typography>
            <Replay
              initialPlaybackProps={{ isPaused: true }}
              source={popupVideo.streamingPaths[0]}
              options={replayPopUp}>
              <HlsjsVideoStreamer />
            </Replay>
            <Typography paragraph style={{ color: theme.text }}>
              {popupVideo.description}
            </Typography>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
