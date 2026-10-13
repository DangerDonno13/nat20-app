//---------------------------------------------------------------------------//
// file:    api.js
// authors: Amon R., Don J., Tristan H.
// brief:   Provides access to server/client communication functions.
// date:    2026/10/12
//---------------------------------------------------------------------------//

const SERVER_URL = process.env.REACT_APP_SERVER_URL || process.env.SERVER_URL || "http://localhost:5009";

/**
 * Sends HTTP request to server and returns response data.
 *
 * @param {string} path The API path to request from.
 * @param {Object} options Optional configuration (e.g., method, body, headers).
 *
 * @returns {Promise<any>} A promise that resolves to the parsed JSON data.
 */
const request = async (path, options = {}) => {
  const response = await fetch(`${SERVER_URL}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  const text = await response.text();
  let data = text;
  try {
    data = JSON.parse(text);
  } catch (err) {
    // plain text response from server
  }

  if (!response.ok) {
    throw new Error(data || `Request failed: ${response.status}`);
  }

  return data;
};

/**
 * Provides access to server/client communication functions.
 */
export const api = {
  /**
   * Gets the user with given email.
   *
   * @param {string} email The email of the user to get.
   *
   * @returns The user with given email if successful; otherwise exception.
   */
  async getUserByEmail(email) {
    return request(`/api/user/${encodeURIComponent(email)}`);
  },

  // sends register info to server
  async registerUser(username, email, password) {
    return request(`/api/user/register`, {
      method: "POST",
      body: JSON.stringify({ username, email, password }),
    });
  },

  // sends login info to server and recieves token + user
  async loginUser(email, password) {
    return request(`/api/user/login`, {
      method: "POST",
      body: JSON.stringify({ email, password }),
    });
  },
};