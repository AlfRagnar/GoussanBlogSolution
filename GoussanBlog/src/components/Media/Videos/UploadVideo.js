import React, { useContext, useState } from "react";
import Form from "react-bootstrap/Form";
import {
  Button,
  Backdrop,
  Fade,
  makeStyles,
  Modal,
  Paper,
  TextField,
} from "@material-ui/core";
import axios from "axios";
import { ThemeContext } from "../../../contexts/ThemeContext";
import { AuthContext } from "../../../contexts/AuthContext";
import { ContainerClient } from "@azure/storage-blob";

const useStyles = makeStyles((theme) => ({
  root: {
    margin: theme.spacing(1),
    width: "30ch",
  },
  input: {
    background: "#ffff",
    borderRadius: 8,
  },
  modal: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  paper: {
    backgroundColor: theme.palette.background.paper,
    border: "2px solid #000",
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
}));

export default function UploadVideo() {
  const classes = useStyles();
  const [open, setOpen] = useState(false);
  const { isDarkTheme, darkTheme, lightTheme } = useContext(ThemeContext);
  const { token, FileSizeLimit } = useContext(AuthContext);
  const theme = isDarkTheme ? darkTheme : lightTheme;
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [file, setFile] = useState();
  const [error, setError] = useState("");

  function validateForm(event) {
    event.preventDefault();
    const One_MegaByte = 1024 * 1024;
    var validateTitle = title.length > 1;
    if (!validateTitle) {
      const Message = "\nYou need to put in a Valid Title";
      setError(Message);
    }
    var validateDescription = description.length > 1;
    if (!validateDescription) {
      const Message = "\nYou need to write a description for this video";
      setError(Message);
    }
    var allowedExtensions = /(\.mp4|\.webm|\.avi)$/i;
    var validateFile = allowedExtensions.exec(file.name);
    if (!validateFile) {
      const Message = "\nYou need to put in a valid file";
      setError(Message);
    }
    var validateSize = One_MegaByte * FileSizeLimit >= file.size;
    if (!validateSize) {
      const Message = `\nFile is too big, Try to keep the file size less than ${FileSizeLimit} MB`;
      setError(Message);
    }

    if (validateTitle && validateDescription && validateFile && validateSize) {
      setError("Uploading...");
      handleSubmit(event);
    }
  }

  const handleOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  const onFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  async function handleSubmit(event) {
    event.preventDefault();
    try {
      const formData = new FormData();
      formData.append("Title", title);
      formData.append("Description", description);
      let ID = "";

      await axios
        .post("/Videos", formData, {
          headers: {
            Authorization: `bearer ${token}`,
          },
        })
        .then(async (res) => {
          const sasUri = res.data.sasUri;
          ID = res.data.id;

          const containerClient = new ContainerClient(sasUri);
          const blobBlockClient = containerClient.getBlockBlobClient(
            "asset-" + res.data.assetid
          );
          await blobBlockClient.uploadData(file);

          console.log("File uploaded to Azure Blob Storage");
        })
        .catch((err) => {
          console.log(err);
        })
        .then(async () => {
          const data = {
            Id: ID,
            filename: file.name,
            filesize: `${file.size}`,
            contenttype: file.type,
          };

          await axios
            .post("/videos/update", data, {
              headers: {
                Authorization: `bearer ${token}`,
              },
            })
            .then(() => {
              console.log("File Scheduled to be Encoded");
              setOpen(false);
            });
        });
    } catch (e) {
      console.log(e.response.data);
    }
    setError("");
  }

  return (
    <div>
      <Button color="inherit" onClick={handleOpen}>
        Upload Video
      </Button>
      <Modal
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
            style={{
              background: theme.background,
              color: theme.text,
              textAlign: "center",
            }}>
            <Form className={classes.root} onSubmit={validateForm}>
              <Form.Group size="lg" controlId="title">
                <TextField
                  className={classes.input}
                  id="title"
                  label="Title"
                  variant="filled"
                  multiline
                  onChange={(e) => setTitle(e.target.value)}
                />
              </Form.Group>
              <Form.Group size="lg" controlId="description">
                <TextField
                  className={classes.input}
                  id="description"
                  label="Description"
                  variant="filled"
                  multiline
                  onChange={(e) => setDescription(e.target.value)}
                />
              </Form.Group>
              <Form.Group size="lg" controlId="file">
                <input type="file" onChange={onFileChange} />
              </Form.Group>
              <Button
                className="mb-2"
                style={{ color: theme.text, background: theme.background }}
                variant="outlined"
                size="medium"
                type="submit">
                Upload
              </Button>
              {error.length > 1 ? (
                <>
                  <p>{error}</p>
                </>
              ) : (
                <div>No Error</div>
              )}
            </Form>
          </Paper>
        </Fade>
      </Modal>
    </div>
  );
}
