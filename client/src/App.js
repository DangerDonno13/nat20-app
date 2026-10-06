import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';


// main app functon for the routes
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<div>home page placeholder</div>} />
          <Route path="login" element={<div>login page placeholder</div>} />
          <Route path="register" element={<div>account creation placeholder</div>} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;