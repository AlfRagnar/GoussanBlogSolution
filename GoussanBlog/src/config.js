const config = {
  defaultEndpoint: "https://goussan-api.azure-api.net", // Set your default endpoint for axios to make API calls to
  // defaultEndpoint: "https://localhost:5001", // Set your default endpoint for axios to make API calls to
  chatEndpoint: "https://goussanblogdata.azurewebsites.net", // Set your default chat endpoint, This needs to be CORS configured for SignalR to Function
};

export default config;
