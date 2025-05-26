module.exports = {
     testEnvironment: "jsdom",
     setupFilesAfterEnv: ["<rootDir>/src/tests/setupTests.js"],
     moduleNameMapper: {
       "\\.(css)$": "<rootDir>/src/tests/__mocks__/styleMock.js",
     },
     transform: {
       "^.+\\.(js|jsx)$": "babel-jest",
     },
     transformIgnorePatterns: ["/node_modules/(?!@testing-library)"],
   };