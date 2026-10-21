import React, { useState } from 'react';

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

  const maxDice = 10;

  return (
    <div className="dice-roller">
      <div className="die-picker">
        <label htmlFor="die-select">Choose a die: </label>
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

      <div className="dice-count-picker">
        <label htmlFor="dice-amount">Amount (max {maxDice}): </label>
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
    </div>
  );
}