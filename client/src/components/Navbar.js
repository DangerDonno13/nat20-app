import React from 'react';
import { NavLink, Link } from 'react-router-dom';
import './Navbar.css';

export default function Navbar() {
  return (
    <nav className="navbar">
      <div className="nav-brand">
        <Link to="/" className="brand-logo">
          Nat 20 Companion
        </Link>
      </div>

      <ul className="nav-links">
        <li>
          <NavLink 
            to="/" 
            className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}
          >
            Home
          </NavLink>
        </li>
        <li>
          <NavLink 
            to="/login" 
            className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}
          >
            Login
          </NavLink>
        </li>
        <li>
          <NavLink 
            to="/register" 
            className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}
          >
            Create Account
          </NavLink>
        </li>
        <li>
          <NavLink 
            to="/dice" 
            className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}
          >
            Dice Roller
          </NavLink>
        </li>
      </ul>
    </nav>
  );
}