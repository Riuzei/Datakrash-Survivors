using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginOptionsObj;
    public GameObject createUserObj;
    public GameObject loginUserObj;

    [Header("Inputs")]
    public TMP_InputField createUserInput; 
    public TMP_InputField loginUserInput;

    [Header("Other")]
    public TMP_Text feedbackText;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void BackToLoginOptions() => ShowPanel(loginOptionsObj); 
    public void OpenLoginUser() => ShowPanel(loginUserObj);
    public void OpenCreateUser() => ShowPanel(createUserObj);
    
    public void CreateUser()
    {
        string input = createUserInput.text;
        bool userCreateSuccess = GameSaveHandler.CreateUser(input);
        if(string.IsNullOrEmpty(input))
            feedbackText.text = "Name is empty";
        else if (userCreateSuccess)
            SceneManager.LoadScene(mainMenuSceneName);
        else
            feedbackText.text = "Name is already taken";
    }

    public void LoginUser()
    {
        bool userLoginSuccess = GameSaveHandler.LoginUser(loginUserInput.text);
        if (userLoginSuccess)
            SceneManager.LoadScene(mainMenuSceneName);
        else if(GameSaveHandler.currentUser == null)
            feedbackText.text = "User not found";

        Debug.Log(GameSaveHandler.currentUser);
    }

    private void ShowPanel(GameObject panel)
    {
        loginOptionsObj.SetActive(panel == loginOptionsObj);
        createUserObj.SetActive(panel == createUserObj);
        loginUserObj.SetActive(panel == loginUserObj);
        feedbackText.text = "";
    }
}