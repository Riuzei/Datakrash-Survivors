using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour, IUpdatable
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI timeTextShadow;

    private float currentTime = 0f;
    
    void Start() => GameplayManager.Instance.Register(this);
    void OnDisable() => GameplayManager.Instance.Unregister(this);
    
    public void OnUpdate(float deltaTime)
    {
        currentTime += deltaTime;
        timeTextShadow.text = timeText.text = Format(currentTime);
    }
    
    public void OnFixedUpdate(float deltaTime) {}
    private const string HighScoreKey = "HighScore";

    public float CurrentTime => currentTime;
    public static float HighScore => GameSaveHandler.GetFloat(HighScoreKey);

    public bool SaveHighScore()
    {
        if (currentTime <= HighScore) return false;
        GameSaveHandler.SetFloat(HighScoreKey, currentTime);
        GameSaveHandler.Save();
        return true;
    }

    public static string Format(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}m {1:00}s", minutes, seconds);
    }
}
