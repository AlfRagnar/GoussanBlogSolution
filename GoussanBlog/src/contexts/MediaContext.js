import axios from "axios";
import React, { createContext, useEffect, useState } from "react";

export const MediaContext = createContext();

const MediaContextProvider = ({ children }) => {
  const [videos, setVideos] = useState([""]);
  const [images, setImages] = useState([""]);
  const [blogs, setBlogs] = useState([""]);
  const [fetchedVideos, setFetchedVideos] = useState(false);
  const [fetchedImages, setFetchedImages] = useState(false);
  const [fetchedBlogs, setFetchedBlogs] = useState(false);

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
            if (res.data !== null) {
              setImages(res.data);
              setFetchedImages(true);
            }
          })
          .catch((e) => {
            console.log(e);
          });
      } catch (err) {
        console.log(err);
        setImages("");
        setFetchedImages(false);
      }
    }

    async function fetchBlogs() {
      try {
        await axios.get("/blog").then((res) => {
          setBlogs(res.data);
          setFetchedBlogs(true);
        });
      } catch (err) {
        setBlogs("");
        setFetchedBlogs(false);
      }
    }
    fetchBlogs();
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
        blogs,
        setBlogs,
        fetchedVideos,
        setFetchedVideos,
        fetchedImages,
        setFetchedImages,
        fetchedBlogs,
        setFetchedBlogs,
      }}>
      {children}
    </MediaContext.Provider>
  );
};

export default MediaContextProvider;
