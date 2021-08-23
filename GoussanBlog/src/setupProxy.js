﻿const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/videos"
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:5001/api',
        secure: false
    });

    app.use(appProxy);
};
