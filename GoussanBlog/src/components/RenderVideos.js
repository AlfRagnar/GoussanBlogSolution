import React, { useContext } from "react";
import { Typography, Card, CardHeader, CardContent } from "@material-ui/core";
import { ThemeContext } from "../contexts/ThemeContext";
import { VideoContext } from "../contexts/VideoContext";
import { Replay } from "vimond-replay";
import "vimond-replay/index.css";
import HlsjsVideoStreamer from "vimond-replay/video-streamer/hlsjs";

export default function RenderVideos() {
  const { videos } = useContext(VideoContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

  const replayOptions = {
    videoStreamer: {
      hlsjs: {
        customConfiguration: {
          capLevelToPlayerSize: true,
          maxBufferLength: 45,
        },
      },
    },
  };

  return (
    <>
      {videos.map((video) => (
        <Card
          className="container"
          style={{
            background: theme.background,
            color: theme.text,
            textAlign: "center",
          }}
          key={video.id}>
          <CardHeader title={video.title} />
          <CardContent>
            <Typography variant="body2">
              Video Description: <br />
              {video.description}
            </Typography>
            <Replay
              initialPlaybackProps={{ isPaused: true }}
              source={video.streamingPaths[0]}
              options={replayOptions}>
              <HlsjsVideoStreamer />
            </Replay>
          </CardContent>
        </Card>
      ))}
    </>
  );
}
