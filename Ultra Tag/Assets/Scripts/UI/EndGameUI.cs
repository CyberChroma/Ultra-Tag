using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public GameObject player;
    public Color winColor;
    public Color loseColor;

    private Image endGameScreen;
    private Text winNameText;
    private MouseHide mouseHide;

    // Start is called before the first frame update
    void Start()
    {
        endGameScreen = transform.Find("End Game Panel").GetComponent<Image>();
        winNameText = endGameScreen.transform.Find("Win Text").GetComponent<Text>();
        endGameScreen.gameObject.SetActive(false);
        mouseHide = FindObjectOfType<MouseHide>();
        Time.timeScale = 1;
    }

    public void EndGame (string winner)
    {
        Time.timeScale = 0;
        endGameScreen.gameObject.SetActive(true);
        if (winner == player.name) {
            endGameScreen.color = winColor;
        } else {
            endGameScreen.color = loseColor;
        }
        winNameText.text = winner + " won!";
        mouseHide.Unhide();
    }

    public void Restart ()
    {
        Time.timeScale = 1;
        mouseHide.Hide();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
