using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Screen")]
    [SerializeField] private TMP_Dropdown screenDropdown;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SfxKey = "SFXVolume";
    private const string ScreenKey = "ScreenMode";
    
    private static readonly string[] screenNames = { "Windowed", "Fullscreen" };
    private static readonly FullScreenMode[] screenModes =
    {
        FullScreenMode.Windowed,
        FullScreenMode.FullScreenWindow
    };

    void Start()
    {
        SetupSlider(masterSlider, MasterKey);
        SetupSlider(musicSlider, MusicKey);
        SetupSlider(sfxSlider, SfxKey);
        SetupDropdown();
    }

    void OnDisable()
    {
        GameSaveHandler.Save();
    }

    private void SetupSlider(Slider slider, string key)
    {
        slider.SetValueWithoutNotify(GameSaveHandler.GetFloat(key));
        ApplyVolume(key, slider.value);

        slider.onValueChanged.AddListener(value =>
        {
            ApplyVolume(key, value);
            GameSaveHandler.SetFloat(key, value);
        });
    }

    private void ApplyVolume(string key, float value)
    {
        mixer.SetFloat(key, Mathf.Log10(value) * 20f);
    }

    private void SetupDropdown()
    {
        screenDropdown.ClearOptions();
        screenDropdown.AddOptions(new List<string>(screenNames));

        int saved = Mathf.Clamp(GameSaveHandler.GetInt(ScreenKey, 0), 0, screenModes.Length - 1);
        screenDropdown.SetValueWithoutNotify(saved);
        Screen.fullScreenMode = screenModes[saved];

        screenDropdown.onValueChanged.AddListener(index =>
        {
            Screen.fullScreenMode = screenModes[index];
            GameSaveHandler.SetInt(ScreenKey, index);
        });
    }
    
    public void CloseSettingsMenu()
    {
        gameObject.SetActive(false);
    }
}