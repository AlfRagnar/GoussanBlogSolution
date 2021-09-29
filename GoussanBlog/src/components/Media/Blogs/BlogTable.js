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
    field: "userId",
    headerName: "UserId",
    width: 200,
  },
];

export default function ImageTable() {
  const apiRef = useGridApiRef();
  const [selectedCell, setSelectedCell] = useState(null);
  const { blogs, fetchedBlogs, fetchBlogs } = useContext(MediaContext);
  const { token } = useContext(AuthContext);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const currentTheme = isDarkTheme ? darkTheme : lightTheme;

  const handleCellClick = (params) => {
    setSelectedCell(params);
  };

  const deleteOperation = async (id) => {
    await axios
      .delete(`/blog/${id}`, {
        headers: {
          Authorization: `bearer ${token}`,
        },
      })
      .then(() => {
        console.log(`Deleted Blog Object: ${id}`);
        fetchBlogs();
      })
      .catch((err) => {
        console.log(err);
        console.log(`Failed to delete Blog Object: ${id}`);
      });
  };

  const DeleteButton = () => {
    const handleClick = async () => {
      const { id } = selectedCell;
      await deleteOperation(id);
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
    await deleteOperation(id);
  };

  useEffect(() => {
    console.log("Blog Table Getting all Blogs");
    fetchBlogs();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <SimpleBar>
      <Typography>List of Blog Objects stored in Database</Typography>
      <DataGrid
        style={{
          color: currentTheme.text,
          background: currentTheme.background,
        }}
        autoHeight
        ref={apiRef}
        rows={blogs}
        columns={columns}
        pageSize={50}
        onCellClick={handleCellClick}
        onCellDoubleClick={handleDoubleClick}
        loading={!fetchedBlogs}
        components={{
          Toolbar: DeleteButton,
        }}
      />
    </SimpleBar>
  );
}
