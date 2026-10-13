import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout.js';
import Home from './components/Home.js';
import Login from './components/Login.js';
import Register from './components/Register.js';


// main app functon for the routes
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;