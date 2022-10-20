using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameFinishedMessage;
    bool gameOver;
   
    // Start is called before the first frame update
    void Start()
    {

        FindObjectOfType<Goal>().OnGoalReached += OnGameWon;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnGameOver()
    {
        gameOverScreen.SetActive(true);
        gameFinishedMessage.text = "You Were Caught!";
        gameOver = true;
    }
    void OnGameWon()
    {
        gameOverScreen.SetActive(true);
        gameFinishedMessage.text = "You're a Master of the Shadow";
        gameOver = true;
    }
}
