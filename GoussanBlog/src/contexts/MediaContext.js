import React, { createContext, useState } from "react";

export const MediaContext = createContext();

const MediaContextProvider = ({ children }) => {
  const [videos, setVideos] = useState([""]);
  const [images, setImages] = useState([""]);
  const [fetchedVideos, setFetchedVideos] = useState(false);
  const [fetchedImages, setFetchedImages] = useState(false);

  return (
    <MediaContext.Provider
      value={{
        videos,
        setVideos,
        images,
        setImages,
        fetchedVideos,
        setFetchedVideos,
        fetchedImages,
        setFetchedImages,
      }}>
      {children}
    </MediaContext.Provider>
  );
};

export default MediaContextProvider;
