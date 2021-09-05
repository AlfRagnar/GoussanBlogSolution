const { createProxyMiddleware } = require("http-proxy-middleware");

const context = ["/videos", "/me", "/user", "/images"];

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    target: "https://goussanapi.azure-api.net",
    secure: false,
  });

  app.use(appProxy);
};
