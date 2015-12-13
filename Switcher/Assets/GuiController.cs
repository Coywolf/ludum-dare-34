using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiController : MonoBehaviour {
    public Text Lives;
    public Text Points;

    public void SetLives(int lives)
    {
        Lives.text = "Lives: " + lives;
    }

    public void SetPoints(int points)
    {
        Points.text = "Points: " + points;
    }
}
