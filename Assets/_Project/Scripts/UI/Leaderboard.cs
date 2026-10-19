using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private LeaderboardItem itemPrefab;
    [SerializeField] private RectTransform content;
    private UIAnimSequencer animSequencer;
    
    void Start()
    {
        List<UserData> users = GameSaveHandler.saveData.users;
        animSequencer = GetComponent<UIAnimSequencer>();
        animSequencer.SetEntries(users.Count);
        
        users.Sort((a, b) => b.highScore.CompareTo(a.highScore));
        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200 + 53 * users.Count);
        for (int i = 0; i < users.Count; i++)
        {
            LeaderboardItem item = Instantiate(itemPrefab, content);
            if(i < 11)
                animSequencer.AddEntry(item.uiElement, i, 0.1f, 0);
            else
                animSequencer.AddEntry(item.uiElement, i, 0, 0);
            item.SetText(i + 1, users[i].name, Stopwatch.Format(users[i].highScore));
        }
    }
    public void CloseLeaderboard()
    {
        gameObject.SetActive(false);
    }
}
