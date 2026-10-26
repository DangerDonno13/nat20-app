import React, { useState } from 'react';
import './DiceRoller.css';

// component to render polyhedral die shapes
function DieIcon({ dieType, className }) {
  switch (dieType) {
    case 2:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <circle cx="50" cy="50" r="42" fill="#120202" />
          <circle cx="50" cy="50" r="34" strokeDasharray="6 4" strokeWidth="2.5" />
        </svg>
      );
    case 4:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <polygon points="50,12 14,84 86,84" fill="#120202" strokeLinejoin="round" />
          <line x1="50" y1="12" x2="50" y2="84" strokeWidth="3" />
        </svg>
      );
    case 6:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <rect x="16" y="16" width="68" height="68" rx="10" fill="#120202" />
        </svg>
      );
    case 8:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <polygon points="50,10 88,50 50,90 12,50" fill="#120202" strokeLinejoin="round" />
          <line x1="12" y1="50" x2="88" y2="50" strokeWidth="3" />
          <line x1="50" y1="10" x2="50" y2="90" strokeWidth="3" />
        </svg>
      );
    case 10:
    case 100:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <polygon points="50,10 88,42 68,90 32,90 12,42" fill="#120202" strokeLinejoin="round" />
          <line x1="50" y1="10" x2="50" y2="90" strokeWidth="3" />
        </svg>
      );
    case 12:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <polygon points="50,10 86,35 72,78 28,78 14,35" fill="#120202" strokeLinejoin="round" />
          <polygon points="50,30 70,45 62,68 38,68 30,45" strokeWidth="3" />
        </svg>
      );
    case 20:
    default:
      return (
        <svg viewBox="0 0 100 100" className={className} fill="none" stroke="currentColor" strokeWidth="5">
          <polygon points="50,8 90,30 90,75 50,95 10,75 10,30" fill="#120202" strokeLinejoin="round" />
          <polygon points="50,32 78,72 22,72" strokeWidth="3" />
          <line x1="50" y1="8" x2="50" y2="32" strokeWidth="3" />
          <line x1="10" y1="30" x2="22" y2="72" strokeWidth="3" />
          <line x1="90" y1="30" x2="78" y2="72" strokeWidth="3" />
        </svg>
      );
  }
}

export default function DiceRoller() {
  const diceOptions = [
    { label: 'Coin (d2)', value: 2 },
    { label: 'd4', value: 4 },
    { label: 'd6', value: 6 },
    { label: 'd8', value: 8 },
    { label: 'd10', value: 10 },
    { label: 'd12', value: 12 },
    { label: 'd20', value: 20 },
    { label: 'd100', value: 100 }
  ];

  const [selectedDie, setSelectedDie] = useState(20);
  const [diceCount, setDiceCount] = useState(1);
  const [results, setResults] = useState([]);
  const [lastRolledDie, setLastRolledDie] = useState(20);
  const [total, setTotal] = useState(0);
  const [isRolling, setIsRolling] = useState(false);

  const maxDice = 10;

  // format coin as H/T or return die value
  const formatDieValue = (rollItem) => {
    if (rollItem.die === 2) {
      return rollItem.value === 1 ? 'H' : 'T';
    }
    return rollItem.value;
  };

  // check if a roll is a nat 20 or nat 1 on a d20
  const getCritClass = (rollItem) => {
    if (rollItem.die === 20) {
      if (rollItem.value === 20) return 'crit-success';
      if (rollItem.value === 1) return 'crit-fail';
    }
    return '';
  };

  // functon to clear the collector tray
  const handleClear = () => {
    setResults([]);
    setTotal(0);
  };

  // roll functon to lock in results
  const handleRoll = () => {
    if (isRolling) return;

    setIsRolling(true);

    setTimeout(() => {
      const newRolls = [];
      let currentTotal = 0;

      for (let i = 0; i < diceCount; i++) {
        // allgorithm to roll between 1 and selected die
        const rollValue = Math.floor(Math.random() * selectedDie) + 1;
        newRolls.push({ die: selectedDie, value: rollValue });
        currentTotal += rollValue;
      }

      setResults(newRolls);
      setTotal(currentTotal);
      setLastRolledDie(selectedDie);
      setIsRolling(false);
    }, 850);
  };

  return (
    <div className="tower-wrapper">
      <div className="dice-tower">
        {/* top turret / hopper */}
        <div className="turret-top">
          <div className="crenellations">
            <span className="merlon"></span>
            <span className="merlon"></span>
            <span className="merlon"></span>
          </div>

          <div className="turret-controls">
            <div className="control-row">
              <label htmlFor="die-select">Die:</label>
              <select 
                id="die-select"
                value={selectedDie}
                onChange={(e) => setSelectedDie(Number(e.target.value))}
              >
                {diceOptions.map((die) => (
                  <option key={die.value} value={die.value}>
                    {die.label}
                  </option>
                ))}
              </select>
            </div>

            <div className="control-row">
              <label htmlFor="dice-amount">Qty:</label>
              <input
                id="dice-amount"
                type="number"
                min="1"
                max={maxDice}
                value={diceCount}
                onChange={(e) => {
                  const val = parseInt(e.target.value, 10);
                  if (val > maxDice) setDiceCount(maxDice);
                  else if (val < 1 || isNaN(val)) setDiceCount(1);
                  else setDiceCount(val);
                }}
              />
            </div>

            <button className="drop-btn" onClick={handleRoll} disabled={isRolling}>
              {isRolling ? 'Dropping...' : 'Drop Dice'}
            </button>
          </div>
        </div>

        {/* middle shaft */}
        <div className="tower-shaft">
          <div className="slit-window"></div>
          {isRolling && (
            <div className="shaft-tumble-zone">
              {Array.from({ length: diceCount }).map((_, idx) => (
                <div 
                  key={idx} 
                  className="tumbling-die-item"
                  style={{ animationDelay: `${idx * 0.08}s` }}
                >
                  <DieIcon dieType={selectedDie} className="die-svg-icon" />
                </div>
              ))}
            </div>
          )}
        </div>

        {/* archway exit */}
        <div className="tower-arch"></div>

        {/* bottom collector tray */}
        <div className="dice-tray">
          <div className="tray-header">
            <span>Collector Tray</span>

            <div className="tray-actions">
              {results.length > 0 && !isRolling && (
                <span className="tray-total">
                  {lastRolledDie === 2
                    ? `Heads: ${results.filter((r) => r.value === 1).length} | Tails: ${results.filter((r) => r.value === 2).length}`
                    : `Total: ${total}`}
                </span>
              )}
              {results.length > 0 && !isRolling && (
                <button className="clear-btn" onClick={handleClear} title="Clear tray">
                  Clear
                </button>
              )}
            </div>
          </div>

          <div className="tray-contents">
            {!isRolling && results.map((item, index) => (
              <div key={index} className={`die-result-wrapper ${getCritClass(item)}`}>
                <DieIcon dieType={item.die} className="result-die-svg" />
                <span className="die-number">{formatDieValue(item)}</span>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}