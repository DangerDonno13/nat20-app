import { useEffect, useState } from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import { api } from "../api/api.js";
import "./Rules.css";

const CHAPTERS = [
  { slug: "01-playing-the-game", title: "Playing the Game" },
  { slug: "02-creating-a-character", title: "Creating a Character" },
  { slug: "04-social-interaction", title: "Social Interaction" },
  { slug: "05-exploration", title: "Exploration" },
  { slug: "06-combat", title: "Combat" },
  { slug: "07-attacks", title: "Attacks" },
  { slug: "08-damage-and-healing", title: "Damage and Healing" },
  { slug: "09-conditions", title: "Conditions" },
  { slug: "10-rests", title: "Rests" },
  { slug: "11-spellcasting", title: "Spellcasting" },
  { slug: "12-level-advancement", title: "Level Advancement" },
];

// Keep chapter headings below the page-level "Rules" heading.
const markdownComponents = {
  h1: ({ children }) => <h2>{children}</h2>,
  h2: ({ children }) => <h3>{children}</h3>,
  h3: ({ children }) => <h4>{children}</h4>,
  table: ({ children }) => (
    <div className="rules-table-scroll">
      <table>{children}</table>
    </div>
  ),
};

export default function Rules() {
  const [selectedSlug, setSelectedSlug] = useState(CHAPTERS[0].slug);
  const [content, setContent] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [reloadKey, setReloadKey] = useState(0);

  useEffect(() => {
    let active = true;

    async function loadChapter() {
      setLoading(true);
      setError("");
      setContent("");

      try {
        const data = await api.getRuleBySlug(selectedSlug);

        if (typeof data?.content !== "string") {
          throw new Error("The server returned an invalid rules chapter.");
        }

        if (active) {
          setContent(data.content);
        }
      } catch (err) {
        if (active) {
          setError(err.message || "Could not load this chapter.");
        }
      } finally {
        if (active) {
          setLoading(false);
        }
      }
    }

    loadChapter();

    // Ignore responses from previous selections or an unmounted page.
    return () => {
      active = false;
    };
  }, [selectedSlug, reloadKey]);

  const selectedChapter = CHAPTERS.find(
    (chapter) => chapter.slug === selectedSlug,
  );

  return (
    <div className="rules-page">
      <header className="rules-header">
        <h1>Rules</h1>
        <p>Your reference for adventures, encounters, and combat</p>
      </header>

      <div className="rules-layout">
        <nav className="rules-chapters" aria-label="Rule chapters">
          {CHAPTERS.map((chapter) => (
            <button
              key={chapter.slug}
              type="button"
              aria-current={selectedSlug === chapter.slug ? "true" : undefined}
              onClick={() => setSelectedSlug(chapter.slug)}
            >
              {chapter.title}
            </button>
          ))}
        </nav>

        <article
          className="rules-content"
          aria-label={selectedChapter.title}
          aria-busy={loading}
        >
          {loading ? (
            <p className="rules-muted" role="status">
              Loading chapter...
            </p>
          ) : error ? (
            <div>
              <p className="rules-error" role="alert">
                {error}
              </p>
              <button
                className="rules-retry"
                type="button"
                onClick={() => setReloadKey((value) => value + 1)}
              >
                Try Again
              </button>
            </div>
          ) : (
            <ReactMarkdown
              remarkPlugins={[remarkGfm]}
              components={markdownComponents}
            >
              {content}
            </ReactMarkdown>
          )}
        </article>
      </div>
    </div>
  );
}
