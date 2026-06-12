using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager: MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;

    public PlayerController player; // reference to player script

    private bool isPlaying = false;
    private float score = 0f;

[Header("Difficulty Scaling")]
public float speedIncreasePerSecond = 0.05f; // adjust as needed
public float maxSpeed = 25f;                 // maximum player speed



    void Start()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        player.enabled = false; // player cannot move until start
    }

    void Update()
{
    if (!isPlaying) return;

    // Increase score
    score += player.forwardSpeed * Time.deltaTime;
    scoreText.text = "Score: " + Mathf.FloorToInt(score);

    // Gradually increase forward speed
    player.forwardSpeed += speedIncreasePerSecond * Time.deltaTime;
    if (player.forwardSpeed > maxSpeed)
        player.forwardSpeed = maxSpeed;
}

    public void StartGame()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        player.enabled = true;
        isPlaying = true;
    }

    public void GameOver()
{
    isPlaying = false;
    player.enabled = false;

    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    
        finalScoreText.text = "Score: " + Mathf.FloorToInt(score);
    }







    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


