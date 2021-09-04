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
  ListSubheader,
} from "@material-ui/core";
import { ThemeContext } from "../contexts/ThemeContext";
import { VideoContext } from "../contexts/VideoContext";
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
}));

export default function RenderVideos() {
  const classes = useStyles();
  const { videos } = useContext(VideoContext);
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

  const replayOptions = {
    videoStreamer: {
      hlsjs: {
        customConfiguration: {
          capLevelToPlayerSize: true,
          maxBufferLength: 45,
        },
      },
      shaka: {
        customConfiguration: {
          streaming: {
            bufferingGoal: 120,
          },
        },
      },
    },
  };

  return (
    <>
      <ImageList className={classes.ImageList} cols={3}>
        <ImageListItem
          key="SubHeaderVideos"
          cols={3}
          style={{ height: "auto" }}>
          <ListSubheader>
            <Typography
              variant="h4"
              style={{ color: theme.text, background: theme.background }}>
              Videos
            </Typography>
          </ListSubheader>
        </ImageListItem>
        {videos.map((video) => (
          <ImageListItem key={video.id + "-list"}>
            <CardActionArea onClick={() => showVideo(video)}>
              <Replay
                initialPlaybackProps={{ isPaused: true }}
                source={video.streamingPaths[0]}
                options={{
                  controls: {
                    includeControls: [""],
                  },
                  interactionDetector: {
                    inactivityDelay: 1,
                  },
                }}>
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
              options={replayOptions}>
              <HlsjsVideoStreamer />
            </Replay>
            <Typography paragraph style={{ color: theme.text }}>
              {popupVideo.description}
            </Typography>
          </Paper>
        </Fade>
      </Modal>
    </>
  );
}
