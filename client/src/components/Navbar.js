import { Link } from 'react-router-dom';

// main nav functon
function Navbar() {
  return (
    <nav>
      <h2>Nat 20 Companion</h2>
      <ul>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/login">Login</Link></li>
        <li><Link to="/register">Create Account</Link></li>
      </ul>
    </nav>
  );
}

export default Navbar;