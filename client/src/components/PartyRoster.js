import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { api } from "../api/api.js";
import "./PartyRoster.css";

export default function PartyRoster() {
  const { campaignId } = useParams();
  const [roster, setRoster] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [reloadKey, setReloadKey] = useState(0);

  useEffect(() => {
    let active = true;

    async function loadRoster() {
      setLoading(true);
      setError("");
      setRoster(null);

      try {
        const data = await api.getPartyRoster(campaignId);

        if (!data || !Array.isArray(data.members)) {
          throw new Error("The server returned an invalid roster.");
        }

        if (active) {
          setRoster(data);
        }
      } catch (err) {
        if (active) {
          setError(err.message || "Could not load the party roster.");
        }
      } finally {
        if (active) {
          setLoading(false);
        }
      }
    }

    loadRoster();

    return () => {
      active = false;
    };
  }, [campaignId, reloadKey]);

  const members = roster?.members ?? [];

  return (
    <div className="party-page">
      <Link className="party-back" to="/campaign">
        Back to Campaigns
      </Link>

      <header className="party-header">
        <div>
          <h1>Party Roster</h1>
          <p>{roster?.session?.name || "Active session members"}</p>
        </div>

        <button
          type="button"
          disabled={loading}
          onClick={() => setReloadKey((value) => value + 1)}
        >
          {loading ? "Loading..." : "Refresh"}
        </button>
      </header>

      <section
        className="party-panel"
        aria-label="Active session party roster"
        aria-busy={loading}
      >
        {loading ? (
          <p role="status">Loading party members...</p>
        ) : error ? (
          <p className="party-error" role="alert">
            {error}
          </p>
        ) : !roster?.session ? (
          <p>There is no active session for this campaign.</p>
        ) : members.length === 0 ? (
          <p>No allied members have joined this session yet.</p>
        ) : (
          <>
            <p className="party-count">
              {members.length} allied{" "}
              {members.length === 1 ? "member" : "members"}
            </p>

            <div className="party-table-scroll">
              <table>
                <caption>Allied members in the active session</caption>
                <thead>
                  <tr>
                    <th scope="col">Name</th>
                    <th scope="col">Class</th>
                    <th scope="col">Level</th>
                  </tr>
                </thead>
                <tbody>
                  {members.map((member) => (
                    <tr key={member.id}>
                      <th scope="row">{member.name}</th>
                      <td>{member.characterClass}</td>
                      <td>{member.level}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </>
        )}
      </section>
    </div>
  );
}
