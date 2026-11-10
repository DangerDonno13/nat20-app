import React, { useState } from 'react';
import { api } from '../api/api.js';

export default function AddCharacter({ campaignId }) {
  const [characterId, setCharacterId] = useState('');
  const [message, setMessage] = useState('');



  // functon to handle attaching the sheet
  const handleAttach = async (e) => {
    e.preventDefault();
    
    try {
      await api.linkCharacterToCampaign(campaignId, characterId);
      setMessage('character linked!');
    } catch (err) {
      setMessage(err.message);
    }
  }

  return (
    <div className="add-char-box">
      <h3>attach a character</h3>
      <form onSubmit={handleAttach}>
        <input 
          type="text" 
          placeholder="character id" 
          value={characterId} 
          onChange={(e) => setCharacterId(e.target.value)} 
        />


        <button type="submit">link it</button>
      </form>
      
      {message && <p>{message}</p>}
    </div>
  );
}