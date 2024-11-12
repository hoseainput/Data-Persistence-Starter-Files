using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public string PlayerName { get; private set; }
    public int HighScore { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadPlayerData();  // Load data when the game starts
    }

    // Save player name and score to PlayerPrefs
    public void SavePlayerData(string playerName, int score)
    {
        PlayerName = playerName;
        HighScore = score;

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    // Load player data from PlayerPrefs
    public void LoadPlayerData()
    {
        // Use default values if data is not available
        PlayerName = PlayerPrefs.GetString("PlayerName", "NoName");
        HighScore = PlayerPrefs.GetInt("HighScore", 0);

        // Debug to verify that data is loading correctly
        Debug.Log($"Loaded PlayerName: {PlayerName}, HighScore: {HighScore}");

    }

    public void SetHighScore(int newHighScore)
{
    if (newHighScore > HighScore)
    {
        HighScore = newHighScore;
        PlayerPrefs.SetInt("HighScore", HighScore); // Save the new high score to PlayerPrefs
        PlayerPrefs.Save(); // Ensure changes are saved
    }
}
}
