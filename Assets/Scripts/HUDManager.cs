using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scaredTimerText;
    [SerializeField] private GameObject[] lifeIcons;




    public void loadMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void setScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void setTimer(float time)
    {
        timerText.text = "Time: " + time.ToString("F1");
    }

    public void setLives(int lives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < lives);
        }
    }

    public void showScaredTimer(float time)
    {
        scaredTimerText.gameObject.SetActive(true);
        scaredTimerText.text = "Scared Time: " + time.ToString("F1");
    }

    public void HideScaredTimer()
    {
        scaredTimerText.gameObject.SetActive(false);
    }
}
