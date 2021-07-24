
module.exports = {
    moduleFileExtensions: [ 'js', 'jsx', 'ts', 'tsx' ],
    moduleDirectories: [ 'node_modules' ],
    moduleNameMapper: {
        '\\.(css|s[ac]ss)$': 'identity-obj-proxy',
        '\\.(png|jpe?g|gif|svg|woff2?|eot|ttf|otf)$': '<rootDir>/mocks/fileMock.js',
        '^@/(.+)': '<rootDir>/src/$1'
    },
    transform: {
        '\\.(jsx?)$': 'babel-jest',
        '\\.(tsx?)$': 'ts-jest'
    },
    testMatch: [
        '**/__tests__/**/*.([jt]s?(x))',
        '**/*.(spec|test).([jt]s?(x))'
    ],
    collectCoverage: true,
    collectCoverageFrom: [
        'src/**/*.([jt]s?(x))',
        '!**/node_modules/**'
    ],
    testEnvironment: 'jsdom'
}
