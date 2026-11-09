import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./components/Layout.js";
import Home from "./components/Home.js";
import Login from "./components/Login.js";
import Register from "./components/Register.js";
import DiceRoller from "./components/DiceRoller.js";
import Campaign from "./components/Campaign.js";
import PartyRoster from "./components/PartyRoster.js";
import Rules from "./components/Rules.js";

// main app functon for the routes
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          <Route path="dice" element={<DiceRoller />} />
          <Route path="campaign" element={<Campaign />} />
          <Route path="campaign/:campaignId/roster" element={<PartyRoster />} />
          <Route path="rules" element={<Rules />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
