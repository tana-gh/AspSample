const { merge }         = require('webpack-merge')
const baseWebpackConfig = require('./webpack.base.config.js')

const prodWebpackConfig = baseWebpackConfig('production')

module.exports = new Promise((res, rej) => {
    res(prodWebpackConfig)
})
