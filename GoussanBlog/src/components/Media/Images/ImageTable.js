import React, { useContext, useEffect, useState } from "react";
import { DataGrid, useGridApiRef } from "@mui/x-data-grid";
import { Typography, Button } from "@material-ui/core";
import SimpleBar from "simplebar-react";
import "simplebar/dist/simplebar.min.css";
import axios from "axios";
// Local Modules/Components
import { MediaContext } from "../../../contexts/MediaContext";
import { ThemeContext } from "../../../contexts/ThemeContext";
import { AuthContext } from "../../../contexts/AuthContext";

const columns = [
  { field: "id", headerName: "ID", width: 150 },
  {
    field: "title",
    headerName: "Title",
    width: 250,
  },
  {
    field: "description",
    headerName: "Description",
    width: 250,
  },
  {
    field: "userId",
    headerName: "UserId",
    width: 200,
  },
  {
    field: "created",
    headerName: "Created",
    width: 130,
  },
];

export default function ImageTable() {
  const apiRef = useGridApiRef();
  const [selectedCell, setSelectedCell] = useState(null);
  const { images, fetchedImages, fetchImages } = useContext(MediaContext);
  const { token } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;

  const handleCellClick = (params) => {
    setSelectedCell(params);
  };

  const DeleteButton = () => {
    const handleClick = async () => {
      const { id } = selectedCell;
      await axios
        .delete(`/videos/${id}`, {
          headers: {
            Authorization: `bearer ${token}`,
          },
        })
        .then(() => {
          console.log(`Deleted Image Object: ${id}`);
          fetchImages();
        })
        .catch(() => {});
    };

    const handleMouseDown = (event) => {
      // Keep the focus in the cell
      event.preventDefault();
    };

    return (
      <Button
        onClick={handleClick}
        onMouseDown={handleMouseDown}
        disabled={!selectedCell}>
        Delete
      </Button>
    );
  };

  const handleDoubleClick = async () => {
    const { id } = selectedCell;
    console.log("Double Click");
    await axios
      .delete(`/videos/${id}`, {
        headers: {
          Authorization: `bearer ${token}`,
        },
      })
      .then(() => {
        console.log(`Deleted Image Object: ${id}`);
        fetchImages();
      })
      .catch((err) => {
        console.log(err);
        console.log(`Failed to delete Image Object: ${id}`);
      });
  };

  useEffect(() => {
    console.log("Image Table Getting all Image");
    fetchImages();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <SimpleBar>
      <Typography>List of Image Objects stored in Database</Typography>
      <DataGrid
        style={{ color: currentTheme.text }}
        autoHeight
        ref={apiRef}
        rows={images}
        columns={columns}
        loading={!fetchedImages}
        pageSize={50}
        onCellClick={handleCellClick}
        onCellDoubleClick={handleDoubleClick}
        components={{
          Toolbar: DeleteButton,
        }}
      />
    </SimpleBar>
  );
}
