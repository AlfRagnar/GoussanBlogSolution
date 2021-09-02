import React, { useContext } from "react";
import { AzureMP } from "react-azure-mp";
import { Typography, Card, CardHeader, CardContent } from "@material-ui/core";
import { ThemeContext } from "../contexts/ThemeContext";
import { VideoContext } from "../contexts/VideoContext";

export default function RenderVideos() {
  const { videos } = useContext(VideoContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;

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

            <AzureMP
              skin="amp-flush"
              options={{
                fluid: true,
              }}
              src={[
                {
                  src: video.streamingPaths[0],
                  type: "application/x-mpegURL",
                },
              ]}
            />
          </CardContent>
        </Card>
      ))}
    </>
  );
}
