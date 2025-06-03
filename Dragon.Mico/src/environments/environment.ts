const url = 'localhost:5216';
const httpProtocol = 'http://';

// const url = 'apidragon.aviquon.com';
// const httpProtocol = 'https://';

export const environment = {
  production: false,
  apiURL: `${httpProtocol}${url}/`,
  fileURL: `${httpProtocol}${url}/dragon`,
};
