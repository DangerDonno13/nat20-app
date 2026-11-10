import { Link } from "react-router-dom";

// Setting-based default artwork mapping for cards
const SETTING_IMAGES = {
  ForgottenRealms:
    "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?auto=format&fit=crop&w=600&q=80",
  Eberron:
    "https://images.unsplash.com/photo-1508739773434-c26b3d09e071?auto=format&fit=crop&w=600&q=80",
  Homebrew:
    "https://images.unsplash.com/photo-1579783902614-a3fb3927b675?auto=format&fit=crop&w=600&q=80",
  Unspecified:
    "https://images.unsplash.com/photo-1534447677768-be436bb09401?auto=format&fit=crop&w=600&q=80",
};

export default function CampaignCard({ campaign }) {
  const imageUrl =
    SETTING_IMAGES[campaign.setting] || SETTING_IMAGES.Unspecified;

  return (
    <div
      style={{
        backgroundColor: "var(--bg-card)",
        border: "var(--border-thick)",
        borderRadius: "8px",
        overflow: "hidden",
        transition: "transform 0.2s ease, box-shadow 0.2s ease",
        boxShadow: "0 4px 16px rgba(0, 0, 0, 0.6)",
      }}
    >
      <div
        style={{
          height: "140px",
          overflow: "hidden",
          backgroundColor: "var(--accent-black)",
        }}
      >
        <img
          src={imageUrl}
          alt={campaign.name}
          style={{
            width: "100%",
            height: "100%",
            objectFit: "cover",
            opacity: "0.85",
          }}
        />
      </div>
      <div style={{ padding: "16px" }}>
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start",
            marginBottom: "8px",
          }}
        >
          <h3
            style={{
              margin: 0,
              fontSize: "18px",
              color: "var(--text-main)",
              fontFamily: "inherit",
            }}
          >
            {campaign.name}
          </h3>
          <span
            style={{
              fontSize: "11px",
              backgroundColor: "#3d1414",
              border: "1px solid #5a1e1e",
              padding: "2px 6px",
              borderRadius: "4px",
              color: "var(--accent-gold)",
            }}
          >
            {campaign.setting || "Homebrew"}
          </span>
        </div>
        <p
          style={{
            margin: 0,
            fontSize: "13px",
            color: "var(--text-muted)",
            lineHeight: "1.4",
            display: "-webkit-box",
            WebkitLineClamp: 2,
            WebkitBoxOrient: "vertical",
            overflow: "hidden",
          }}
        >
          {campaign.description || "No description provided."}
        </p>
        {campaign.id && (
          <Link
            to={`/campaign/${campaign.id}/roster`}
            aria-label={`View party for ${campaign.name}`}
            style={{
              display: "inline-block",
              marginTop: "16px",
              padding: "8px 16px",
              borderRadius: "4px",
              backgroundColor: "var(--accent-gold)",
              color: "var(--accent-black)",
              fontWeight: "bold",
              textDecoration: "none",
            }}
          >
            View Party
          </Link>
        )}
      </div>
    </div>
  );
}
