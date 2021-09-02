import React, { createContext, useState } from "react";

export const VideoContext = createContext();

const VideoContextProvider = ({ children }) => {
  const [videos, setVideos] = useState([""]);

  return (
    <VideoContext.Provider value={{ videos, setVideos }}>
      {children}
    </VideoContext.Provider>
  );
};

export default VideoContextProvider;
