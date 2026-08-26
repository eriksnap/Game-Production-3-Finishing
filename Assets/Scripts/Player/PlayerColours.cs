using UnityEngine;

public static class PlayerColours
{
    public static readonly Color[] Colours = new Color[]
    {
        new Color(0f, 0.78f, 0f),      // Player 1: Green
        new Color(1f, 0.86f, 0f),      // Player 2: Yellow
        new Color(0.59f, 0f, 0.78f),   // Player 3: Purple
        new Color(0.86f, 0f, 0f)       // Player 4: Red
    };

    public static Color Get(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= Colours.Length)
            return Color.white;
        return Colours[playerIndex];
    }
}