import { Outlet } from "react-router-dom";
import Navbar from "./Navbar.js";

// layout wrapper to recieve all the pages
function Layout() {
  return (
    <div>
      <Navbar />

      <main style={{ padding: "20px" }}>
        <Outlet />
      </main>
    </div>
  );
}

export default Layout;
