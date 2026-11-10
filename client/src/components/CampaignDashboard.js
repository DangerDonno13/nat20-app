import React, { useState, useEffect } from 'react';
import { api } from '../api/api.js';
import AddCharacter from './AddCharacter.js';

export default function CampaignDashboard() {
  const [characters, setCharacters] = useState([]);
  
  // dummy id for testing until routing is fully built
  const campaignId = "652b49c7f1a3e5d8b8e0a123";

  useEffect(() => {
    // functon to recieve the party roster
    const fetchRoster = async () => {
      try {
        const data = await api.getCampaignCharacters(campaignId);
        setCharacters(data);
      } catch (err) {
        console.error("error loading roster", err);
      }
    }

    
    fetchRoster();
  }, [campaignId]);

  return (
    <div style={{ maxWidth: '800px', margin: '40px auto', padding: '20px' }}>
      <h2>Campaign Dashboard</h2>
      
      <AddCharacter campaignId={campaignId} />

      <div style={{ marginTop: '30px' }}>
        <h3>Party Roster</h3>
        
        {characters.length === 0 ? (
          <p>no characters attached to this campaign yet.</p>
        ) : (
          <ul>
            {characters.map(char => (
              <li key={char.id}>
                {char.name} - Lvl {char.level} {char.class} (HP: {char.hitPoints})
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}