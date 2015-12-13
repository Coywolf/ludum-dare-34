using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiController : MonoBehaviour {
    public Text LivesText;
    public Text PointsText;
    public Text GameOverPointsText;

    public GameObject GameUi;
    public GameObject MenuUi;
    public GameObject GameOverUi;

    public Game Game;

    private int Points;

    public void SetLives(int lives)
    {
        if (lives <= 0)
        {
            GameUi.SetActive(false);
            GameOverUi.SetActive(true);
            
            GameOverPointsText.text = Points.ToString();
        }
        else
        {
            LivesText.text = "Lives: " + lives;
        }
    }

    public void SetPoints(int points)
    {
        PointsText.text = "Points: " + points;
    }

    public void StartGame()
    {
        Instantiate(Game, Vector3.zero, Quaternion.identity);
        MenuUi.SetActive(false);
        GameUi.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        Destroy(FindObjectOfType<Game>().gameObject);
        GameOverUi.SetActive(false);
        MenuUi.SetActive(true);
    }
}
