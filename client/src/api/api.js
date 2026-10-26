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
 * Attaches the saved JWT as a Bearer token unless skipAuth is set (login/register).
 * If the server rejects a token we sent (expired/invalid), the saved session is
 * cleared and the user is sent back to the login page.
 * 
 * @param {string} path The API path to request from.
 * @param {Object} options Optional configuration (e.g., method, body, headers).
 *
 * @returns {Promise<any>} A promise that resolves to the parsed JSON data.
 */
const request = async (path, options = {}) => {
  const { skipAuth = false, headers, ...fetchOptions } = options;

  const token = skipAuth ? null : localStorage.getItem("token");


  const response = await fetch(`${SERVER_URL}${path}`, {
    ...fetchOptions,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...headers,
    },
  });

  // Token missing, expired or invalid on a protected endpoint.
  if (response.status === 401 && !skipAuth) {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    window.location.assign("/login");
    throw new Error("Your session has expired. Please log in again.");
  }


  const text = await response.text();
  let data = text;
  try {
    data = JSON.parse(text);
  } catch (err) {
    // plain text response from server
  }

  if (!response.ok) {
    // Plain-text errors are strings; validation errors come back as a JSON object.
    const message = typeof data === "string" ? data : data?.title || data?.message;
    throw new Error(message || `Request failed: ${response.status}`);
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
      skipAuth: true,
      body: JSON.stringify({ username, email, password }),
    });
  },

  // sends login info to server and recieves token + user
  async loginUser(email, password) {
    return request(`/api/user/login`, {
      method: "POST",
      skipAuth: true,
      body: JSON.stringify({ email, password }),
    });
  },
};