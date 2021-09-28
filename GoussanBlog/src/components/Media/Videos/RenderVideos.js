import React, { useContext, useEffect, useState } from "react";
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
import { ThemeContext } from "../../../contexts/ThemeContext";
import { MediaContext } from "../../../contexts/MediaContext";
import { Replay } from "vimond-replay";
import "vimond-replay/index.css";
import HlsjsVideoStreamer from "vimond-replay/video-streamer/hlsjs";

export default function RenderVideos() {
  const { videos, fetchVideos } = useContext(MediaContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;

  const [popupVideo, setPopupVideo] = useState(videos[0]);
  const [open, setOpen] = useState(false);

  const useStyles = makeStyles((theme) => ({
    paperBody: {
      background: currentTheme.background,
      color: currentTheme.text,
      textAlign: "center",
      margin: theme.spacing(1),
      width: "75%",
    },
    modal: {
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    },
    imageList: {
      flexWrap: "wrap",
      // Promote the list into his own layer on Chrome. This cost memory but helps keeping high FPS.
      transform: "translateZ(0)",
    },
    root: {
      margin: theme.spacing(1),
    },
    imageTitle: {
      bottom: "5vh",
    },
    videoPlayer: { margin: theme.spacing(1) },
    popupDescription: {
      color: currentTheme.text,
    },
    popupTitle: {
      color: currentTheme.text,
    },
  }));

  const classes = useStyles();

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

  useEffect(() => {
    console.log("fetching videos");
    fetchVideos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className={classes.root}>
      <Typography variant="h3">Uploaded Videos</Typography>
      <ImageList className={classes.imageList} cols={4} rowHeight={150}>
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
              <ImageListItemBar
                className={classes.imageTitle}
                title={video.title}
              />
            </CardActionArea>
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
          <Paper className={classes.paperBody}>
            <Typography variant="h2" className={classes.popupTitle}>
              {popupVideo.title}
            </Typography>
            <div className={classes.videoPlayer}>
              <Replay
                initialPlaybackProps={{ isPaused: true }}
                source={popupVideo.streamingPaths[0]}
                options={replayPopUp}>
                <HlsjsVideoStreamer />
              </Replay>
            </div>
            <Typography paragraph className={classes.popupDescription}>
              {popupVideo.description}
            </Typography>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
