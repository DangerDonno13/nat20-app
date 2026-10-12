import { api } from "./api.js";

(async () => {
  try {
    const result = await api.getUserByEmail("chud@chud.com");
    console.log(result);
  } catch (e) {
    console.error(e);
  }
})();
