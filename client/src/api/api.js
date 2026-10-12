//---------------------------------------------------------------------------//
// file:    api.js
// authors: Amon R., Don J., Tristan H.
// brief:   Provides access to server/client communication functions.
// date:    2026/10/12
//---------------------------------------------------------------------------//

import "dotenv/config";

const SERVER_URL = process.env.SERVER_URL;

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

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return response.json();
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
};
