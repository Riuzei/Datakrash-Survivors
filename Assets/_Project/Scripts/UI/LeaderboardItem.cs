using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    public UIElementAnimator uiElement;
    
    public void SetText(int rank, string username, string score)
    {
        rankText.text = rank.ToString();
        usernameText.text = username;
        scoreText.text = score;
    }
}
