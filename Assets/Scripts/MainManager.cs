using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public Text HighScoreText;  // For displaying the high score
    public GameObject GameOverText;
    public GameObject NameInputPanel;
    public TMP_InputField NameInputField;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    // Track if it's the first game played
    private bool isFirstGame = true;

    private void Start()
    {
        // Ensure the high score is loaded when the game starts
        PlayerDataManager.Instance.LoadPlayerData();  

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
                    InitializeBricks();
        }
        UpdateHighScoreText();  // Display the loaded high score at the start of the game
    }

    private void InitializeBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = {1, 1, 2, 2, 5, 5};

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                StartBall();
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Reset the current score (but keep the high score)
                m_Points = 0;
                ScoreText.text = $"Score : {m_Points}";
                m_GameOver = false; // Reset game over state
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the scene
            }
        }
    }

    private void StartBall()
    {
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";  // Update current score
    }

public void GameOver()
{
    m_GameOver = true;
    GameOverText.SetActive(true);

    // Check if the current score is higher than the saved high score
    if (m_Points > PlayerDataManager.Instance.HighScore)
    {
        Debug.Log($"New high score achieved! Current Score: {m_Points}, High Score: {PlayerDataManager.Instance.HighScore}");
        PlayerDataManager.Instance.SavePlayerData(NameInputField.text, m_Points);  // Save the new high score and name
        UpdateHighScoreText();  // Update the high score text
        NameInputPanel.SetActive(true); // Show the input field for the new high score
    }
    else
    {
        Debug.Log("Game over, but no new high score.");
        NameInputPanel.SetActive(false); // Hide input panel if no new high score
    }
}


    public void SubmitName()
    {
        if (NameInputField == null)
        {
            Debug.LogError("NameInputField is null.");
            return;
        }

        string playerName = NameInputField.text;
        Debug.Log("Player Name: " + playerName);

        // Save player name and score
        PlayerDataManager.Instance.SavePlayerData(playerName, m_Points);

        // Ensure the latest high score is displayed
        UpdateHighScoreText();

        // Hide the NameInputPanel after submitting the name
        NameInputPanel.SetActive(false);
    }

    private void UpdateHighScoreText()
    {
        // Display the saved high score and player name
        HighScoreText.text = $"Best Score: {PlayerDataManager.Instance.PlayerName} : {PlayerDataManager.Instance.HighScore}";
    }
}

