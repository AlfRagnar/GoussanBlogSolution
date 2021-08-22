import React, { Component } from "react";

export default class App extends Component {
  static displayName = App.name;

  constructor(props) {
    super(props);
    this.state = { videos: [], loading: true };
  }

  componentDidMount() {
    this.populateVideosData();
  }

  static renderVideosTable(videos) {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>ID</th>
            <th>Title</th>
            <th>Description</th>
            <th>Filename</th>
            <th>Date Created</th>
          </tr>
        </thead>
        <tbody>
          {videos.map((video) => (
            <tr key={video.id}>
              <td>{video.title}</td>
              <td>{video.description}</td>
              <td>{video.filename}</td>
              <td>{video.created}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading... Please refresh once the ASP.NET backend has started.</em>
      </p>
    ) : (
      App.renderVideosTable(this.state.videos)
    );

    return (
      <div>
        <h1 id="tabelLabel">Videos in Cosmos DB</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateVideosData() {
    const response = await fetch("videos");
    const data = await response.json();
    this.setState({ videos: data, loading: false });
  }
}
