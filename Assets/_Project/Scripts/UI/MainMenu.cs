using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private GameObject settingsObj;
    [SerializeField] private GameObject leaderboardObj;
    
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    void Start()
    {
        highScoreText.text = Stopwatch.Format(Stopwatch.HighScore);
        usernameText.text = GameSaveHandler.currentUser.name;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenSettings()
    {
        settingsObj.gameObject.SetActive(true);
    }
    public void OpenLeaderboard()
    {
        leaderboardObj.gameObject.SetActive(true);
    }
}
