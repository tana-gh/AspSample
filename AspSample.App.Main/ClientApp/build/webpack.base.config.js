const path = require('path')
// const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const CopyWebpackPlugin    = require('copy-webpack-plugin')
const HtmlWebpackPlugin    = require('html-webpack-plugin')
const Sass   = require('sass')
const Fibers = require('fibers')

const PATHS = {
    root  : path.join(__dirname, '..'),
    src   : path.join(__dirname, '../src'),
    dist  : path.join(__dirname, '../dist'),
    assets: path.join(__dirname, '../assets')
}

const TARGETS = {
    main: 'web'
}

const ENTRIES = {
    main: PATHS.src
}

const PAGES_DIR = `${PATHS.assets}/html`
const PAGES = {
    main: 'index.html'
}

module.exports = mode => Object.entries(TARGETS).map(([ key, target ]) => ({
    mode,
    target,
    entry: { [key]: ENTRIES[key] },
    output: {
        filename: '[name].bundle.js',
        path: PATHS.dist,
        publicPath: ''
    },
    ...(!target.includes('web') ? {
        node: {
            __dirname : false,
            __filename: false
        }
    } : {}),
    resolve: {
        extensions: [ '.js', '.jsx', '.ts', '.tsx', 'json' ],
        alias: {
            '~': PATHS.src
        }
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                vendor: {
                    name   : 'vendor',
                    test   : /node_modules/,
                    chunks : 'all',
                    enforce: true
                }
            }
        }
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                use: [
                    'cache-loader',
                    'babel-loader',
                    'eslint-loader'
                ],
                exclude: /node_modules/
            },
            {
                test: /\.tsx?$/,
                use: [
                    'cache-loader',
                    'babel-loader',
                    'ts-loader',
                    'eslint-loader'
                ]
            },
            {
                test: /\.elm$/,
                use: [
                    'cache-loader',
                    ...(mode === 'development' ? [ 'elm-hot-webpack-loader' ] : []),
                    {
                        loader: 'elm-webpack-loader',
                        options: { cwd: PATHS.root }
                    }
                ],
                exclude: [ /elm-stuff/, /node_modules/ ]
            },
            {
                test: /\.css$/,
                use: [
                    'cache-loader',
                    'style-loader',
                    // MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: { sourceMap: true }
                    },
                    {
                        loader: 'postcss-loader',
                        options: { sourceMap: true }
                    }
                ]
            },
            {
                test: /\.sass$/,
                use: [
                    'cache-loader',
                    'style-loader',
                    // MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: { sourceMap: true }
                    },
                    {
                        loader: 'postcss-loader',
                        options: { sourceMap: true }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap     : true,
                            implementation: Sass,
                            sassOptions   : {
                                indentedSyntax: true,
                                fiber: Fibers
                            }
                        }
                    }
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    'cache-loader',
                    'style-loader',
                    // MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: { sourceMap: true }
                    },
                    {
                        loader: 'postcss-loader',
                        options: { sourceMap: true }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap     : true,
                            implementation: Sass,
                            sassOptions   : { fiber: Fibers }
                        }
                    }
                ]
            },
            {
                test: /\.(png|jpe?g|gif|svg)(\?.*)?$/,
                use: [
                    'cache-loader',
                    {
                        loader: 'url-loader',
                        options: {
                            limit: 10000,
                            name : 'assets/images/[name].[contenthash].[ext]'
                        }
                    }
                ]
            },
            {
                test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
                use: [
                    'cache-loader',
                    {
                        loader: 'url-loader',
                        options: {
                            limit: 10000,
                            name : 'assets/fonts/[name].[contenthash].[ext]'
                        }
                    }
                ]
            },
            {
                test: /\.(txt|glsl|raw)$/,
                use: [
                    'cache-loader',
                    'raw-loader'
                ]
            }
        ]
    },
    plugins: [
        // new MiniCssExtractPlugin({
        //     filename: 'assets/css/[name].css'
        // }),
        
        new CopyWebpackPlugin({
            patterns: [
                { from: `${PATHS.assets}/favicon/`  , to: 'assets/favicon/' },
                { from: `${PATHS.assets}/images/`   , to: 'assets/images/'  },
                { from: `${PATHS.assets}/misc/`     , to: 'assets/misc/'    },
                { from: `${PATHS.assets}/robots.txt`, to: 'robots.txt'      }
            ]
        }),

        ...(PAGES[key] ? [
            new HtmlWebpackPlugin({
                template     : `${PAGES_DIR}/${PAGES[key]}`,
                filename     : PAGES[key],
                inject       : 'head',
                scriptLoading: 'defer',
                chunks       : [ key, 'vendor' ]
            })
        ] : [])
    ]
}))

module.exports.externals = {
    paths: PATHS
}
