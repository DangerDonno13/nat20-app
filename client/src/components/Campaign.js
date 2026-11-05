import { useState, useEffect } from 'react';
import { api } from '../api/api.js';
import CampaignCard from './CampaignCard.js';

function Campaign() {
  // Separate state for owned and shared campaigns
  const [ownedCampaigns, setOwnedCampaigns] = useState([]);
  const [sharedCampaigns, setSharedCampaigns] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  // Modal & Form State for Creation
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [setting, setSetting] = useState('ForgottenRealms');
  const [status, setStatus] = useState('Active');
  const [formError, setFormError] = useState('');
  const [submitting, setSubmitting] = useState(false);

  // Fetch campaigns on mount
  useEffect(() => {
    loadCampaigns();
  }, []);

  const loadCampaigns = async () => {
    setLoading(true);
    setError('');
    try {
      const data = await api.getCampaigns();
      
      // Safely map lists from UserCampaignsResponseDto
      setOwnedCampaigns(Array.isArray(data?.ownedCampaigns) ? data.ownedCampaigns : []);
      setSharedCampaigns(Array.isArray(data?.sharedCampaigns) ? data.sharedCampaigns : []);
    } catch (err) {
      setError(err.message || 'Failed to load campaigns.');
      setOwnedCampaigns([]);
      setSharedCampaigns([]);
    } finally {
      setLoading(false);
    }
  };

  // Handle campaign creation submission
  const handleCreate = async (e) => {
    e.preventDefault();
    setFormError('');
    setSubmitting(true);

    try {
      await api.createCampaign(name, description, setting, status);
      setIsModalOpen(false);
      setName('');
      setDescription('');
      setSetting('ForgottenRealms');
      setStatus('Active');
      await loadCampaigns(); // Refresh list
    } catch (err) {
      setFormError(err.message || 'Could not create campaign.');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return <div style={{ textAlign: 'center', padding: '50px', color: '#666' }}>Loading campaigns...</div>;
  }

  return (
    <div style={{ width: '100%', maxWidth: '1100px', margin: '0 auto', padding: '10px 0' }}>
      {/* Header */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '30px', borderBottom: 'var(--border-thick)', paddingBottom: '15px' }}>
        <div>
          <h1 style={{ margin: 0, color: 'var(--text-main)', fontSize: '28px' }}>Campaigns</h1>
          <p style={{ margin: '5px 0 0 0', color: 'var(--text-muted)', fontSize: '14px' }}>Manage your realms and adventures</p>
        </div>
        <button
          onClick={() => setIsModalOpen(true)}
          style={{
            backgroundColor: 'var(--accent-gold)',
            color: 'var(--accent-black)',
            border: 'none',
            padding: '10px 20px',
            borderRadius: '6px',
            cursor: 'pointer',
            fontWeight: 'bold',
            fontSize: '14px',
            boxShadow: '0 2px 6px rgba(0,0,0,0.4)',
          }}
        >
          + New Campaign
        </button>
      </div>

      {error && <p style={{ color: '#e74c3c', marginBottom: '20px' }}>{error}</p>}

      {/* OWNED CAMPAIGNS SECTION */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ color: 'var(--accent-gold)', fontSize: '20px', marginBottom: '15px' }}>
          Owned Campaigns ({ownedCampaigns.length})
        </h2>
        <div style={gridStyle}>
          {ownedCampaigns.map((camp, index) => (
            <CampaignCard 
              key={camp.id || camp._id || `owned-${index}`} 
              campaign={camp} 
            />
          ))}

          {/* Dedicated Create Card */}
          <div
            onClick={() => setIsModalOpen(true)}
            style={{
              backgroundColor: 'var(--bg-card)',
              border: '2px dashed #5a1e1e',
              borderRadius: '8px',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              justifyContent: 'center',
              minHeight: '235px',
              cursor: 'pointer',
              transition: 'border-color 0.2s',
            }}
          >
            <div style={{ fontSize: '36px', color: 'var(--accent-gold)', marginBottom: '5px' }}>+</div>
            <div style={{ fontWeight: 'bold', color: 'var(--text-main)' }}>Create Campaign</div>
          </div>
        </div>
      </section>

      {/* SHARED CAMPAIGNS SECTION */}
      {sharedCampaigns.length > 0 && (
        <section style={{ marginBottom: '40px' }}>
          <h2 style={{ color: 'var(--accent-gold)', fontSize: '20px', marginBottom: '15px' }}>
            Shared Campaigns ({sharedCampaigns.length})
          </h2>
          <div style={gridStyle}>
            {sharedCampaigns.map((camp, index) => (
              <CampaignCard 
                key={camp.id || camp._id || `shared-${index}`} 
                campaign={camp} 
              />
            ))}
          </div>
        </section>
      )}

      {/* CREATE CAMPAIGN MODAL */}
      {isModalOpen && (
        <div style={modalOverlayStyle}>
          <div style={{
            backgroundColor: 'var(--bg-card)',
            border: 'var(--border-thick)',
            padding: '30px',
            borderRadius: '8px',
            width: '100%',
            maxWidth: '450px',
            boxShadow: '0 8px 24px rgba(0,0,0,0.8)',
            color: 'var(--text-main)',
          }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px', borderBottom: 'var(--border-subtle)', paddingBottom: '10px' }}>
              <h3 style={{ margin: 0, color: 'var(--accent-gold)' }}>Create New Campaign</h3>
              <button
                onClick={() => setIsModalOpen(false)}
                style={{ background: 'none', border: 'none', fontSize: '20px', cursor: 'pointer', color: 'var(--text-muted)' }}
              >
                &times;
              </button>
            </div>

            {formError && <p style={{ color: '#e74c3c', fontSize: '14px' }}>{formError}</p>}

            <form onSubmit={handleCreate}>
              <div style={{ marginBottom: '15px' }}>
                <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px', color: 'var(--text-muted)' }}>Name</label>
                <input
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                  placeholder="e.g. Curse of Strahd Reimagined"
                  style={inputStyle}
                />
              </div>

              <div style={{ marginBottom: '15px' }}>
                <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px', color: 'var(--text-muted)' }}>Description</label>
                <textarea
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  rows="3"
                  placeholder="Short summary of the adventure..."
                  style={{ ...inputStyle, resize: 'vertical' }}
                />
              </div>

              <div style={{ marginBottom: '20px' }}>
                <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px', color: 'var(--text-muted)' }}>Setting</label>
                <select
                  value={setting}
                  onChange={(e) => setSetting(e.target.value)}
                  style={inputStyle}
                >
                  <option value="ForgottenRealms">Forgotten Realms</option>
                  <option value="Eberron">Eberron</option>
                  <option value="Homebrew">Homebrew</option>
                  <option value="Unspecified">Unspecified</option>
                </select>
              </div>

              <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
                <button
                  type="button"
                  onClick={() => setIsModalOpen(false)}
                  style={{ padding: '8px 16px', border: '1px solid #5a1e1e', background: 'transparent', color: 'var(--text-main)', borderRadius: '4px', cursor: 'pointer' }}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={submitting}
                  style={{ padding: '8px 20px', background: 'var(--accent-gold)', color: 'var(--accent-black)', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
                >
                  {submitting ? 'Creating...' : 'Create Campaign'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

// Styles
const gridStyle = {
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
  gap: '20px',
};

const modalOverlayStyle = {
  position: 'fixed',
  top: 0,
  left: 0,
  right: 0,
  bottom: 0,
  backgroundColor: 'rgba(0, 0, 0, 0.75)',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  zIndex: 1000,
};

const inputStyle = {
  width: '100%',
  padding: '8px 12px',
  borderRadius: '4px',
  border: '1px solid #5a1e1e',
  backgroundColor: 'var(--accent-black)',
  color: 'var(--text-main)',
  fontSize: '14px',
  boxSizing: 'border-box',
  fontFamily: 'inherit',
};

export default Campaign;