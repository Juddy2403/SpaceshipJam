using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public List<int> PlayerScores = new();
    public event Action<int, int> OnScoreChanged;
    public List<String> PlayerColorNames;
    public List<Color> PlayerColors;

    void Start()
    {
        PlayerScores.Add(0); // Player 1 score
        PlayerScores.Add(0); // Player 2 score
        PlayerColorNames = new List<string> { "Blue", "Red"};
        PlayerColors = new List<Color> { Color.blue, Color.red };
    }
    public void ResetScores()
    {
        for (int i = 0; i < PlayerScores.Count; i++)
        {
            PlayerScores[i] = 0;
        }

    }

    // Update is called once per frame
    public void AddScore(int playerNumber, int scoreToAdd)
    {
        if (playerNumber < 0 || playerNumber >= PlayerScores.Count)
        {
            Debug.LogError("Invalid player number");
            return;
        }
        PlayerScores[playerNumber] += scoreToAdd;

        OnScoreChanged?.Invoke(playerNumber, PlayerScores[playerNumber]);

    }
}
