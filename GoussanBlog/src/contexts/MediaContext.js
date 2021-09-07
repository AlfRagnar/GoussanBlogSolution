import axios from "axios";
import React, { createContext, useEffect, useState } from "react";

export const MediaContext = createContext();

const MediaContextProvider = ({ children }) => {
  const [videos, setVideos] = useState([""]);
  const [images, setImages] = useState([""]);
  const [fetchedVideos, setFetchedVideos] = useState(false);
  const [fetchedImages, setFetchedImages] = useState(false);

  useEffect(() => {
    async function fetchVideos() {
      try {
        await axios.get("/videos").then((res) => {
          setVideos(res.data);
          setFetchedVideos(true);
        });
      } catch (err) {
        setVideos("");
        setFetchedVideos(false);
      }
    }

    async function fetchImages() {
      try {
        await axios
          .get("/Image")
          .then((res) => {
            setImages(res.data);
            setFetchedImages(true);
          })
          .catch((e) => {
            console.log(e);
          });
      } catch (err) {
        console.log(err);
        setVideos("");
        setFetchedVideos(false);
      }
    }
    fetchVideos();
    fetchImages();
  }, []);

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
