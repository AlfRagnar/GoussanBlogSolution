const { createProxyMiddleware } = require("http-proxy-middleware");

const context = ["/videos", "/me", "/user"];

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    target: "https://goussanapi.azure-api.net",
    secure: false,
  });

  app.use(appProxy);
};
