using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Match Settings")]
    public int roundsToWin = 3;

    private Dictionary<int, int> roundWins = new Dictionary<int, int>();
    public int currentRound = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddWin(int playerIndex)
    {
        if (playerIndex < 0) return; // draws don't score

        if (!roundWins.ContainsKey(playerIndex))
            roundWins[playerIndex] = 0;

        roundWins[playerIndex]++;
    }

    public bool HasMatchWinner(out int winnerIndex)
    {
        foreach (var kvp in roundWins)
        {
            if (kvp.Value >= roundsToWin)
            {
                winnerIndex = kvp.Key;
                return true;
            }
        }
        winnerIndex = -1;
        return false;
    }

    public int GetWins(int playerIndex) =>
        roundWins.ContainsKey(playerIndex) ? roundWins[playerIndex] : 0;

    public string BuildScoreboardText(int playerCount)
    {
        string result = $"Round {currentRound}  —  ";
        for (int i = 0; i < playerCount; i++)
        {
            result += $"P{i + 1}: {GetWins(i)}";
            if (i < playerCount - 1) result += "   ";
        }
        return result;
    }

    public void ResetMatch()
    {
        roundWins.Clear();
        currentRound = 1;
    }
}