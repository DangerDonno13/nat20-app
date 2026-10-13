import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';


// home page component
function Home() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    // check if user info was saved from login
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      setUser(JSON.parse(savedUser));
    }
  }, []);


  // functon to clear storage on logout
  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
  };

  return (
    <div style={{ maxWidth: '500px', margin: '40px auto', padding: '20px' }}>
      <h2>Nat 20 Companion</h2>

      {user ? (
        <div>
          <p style={{ color: 'green', fontWeight: 'bold' }}>
            You are logged in! Welcome back, {user.username} ({user.email}).
          </p>
          <button onClick={handleLogout} style={{ padding: '8px 16px', cursor: 'pointer' }}>
            Log Out
          </button>
        </div>
      ) : (
        <div>
          <p>You are not logged in right now.</p>
          <p>
            <Link to="/login">Log In</Link> | <Link to="/register">Create Account</Link>
          </p>
        </div>
      )}
    </div>
  );
}

export default Home;