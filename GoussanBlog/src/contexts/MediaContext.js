import axios from "axios";
import React, { createContext, useContext, useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";

export const MediaContext = createContext();

const MediaContextProvider = ({ children }) => {
  const { token } = useContext(AuthContext);
  const [videos, setVideos] = useState([""]);
  const [allVideos, setAllVideos] = useState([""]);
  const [images, setImages] = useState([""]);
  const [blogs, setBlogs] = useState([""]);
  const [fetchedVideos, setFetchedVideos] = useState(false);
  const [fetchedImages, setFetchedImages] = useState(false);
  const [fetchedBlogs, setFetchedBlogs] = useState(false);

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

  async function fetchAllVideos() {
    try {
      await axios
        .get("/videos/all", {
          headers: {
            Authorization: `bearer ${token}`,
          },
        })
        .then((res) => {
          setAllVideos(res.data);
        });
    } catch (err) {
      setAllVideos([""]);
    }
  }

  useEffect(() => {
    fetchBlogs();
    fetchVideos();
    fetchImages();
    fetchAllVideos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
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
        allVideos,
        setAllVideos,
        fetchAllVideos,
        fetchVideos,
      }}>
      {children}
    </MediaContext.Provider>
  );
};

export default MediaContextProvider;
