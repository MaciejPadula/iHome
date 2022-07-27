const path = require('path');

module.exports = {
    entry: {
        rooms: './src/rooms',
    },
    output: {
        path: path.resolve(__dirname, '../iHome/wwwroot/js'),
        filename: '[name].js',
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                },
            },
        ],
    },
    devtool: 'inline-source-map',
    resolve: {
        extensions: ['.jsx', '.js'],
    },
};
