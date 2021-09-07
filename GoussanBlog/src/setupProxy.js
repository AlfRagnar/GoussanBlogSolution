const { createProxyMiddleware } = require("http-proxy-middleware");

const context = ["/videos", "/me", "/user", "/images", "/chathub"];

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    // target: "https://goussanapi.azure-api.net",
    target: "https://localhost:5001",
    secure: false,
  });

  app.use(appProxy);
};
