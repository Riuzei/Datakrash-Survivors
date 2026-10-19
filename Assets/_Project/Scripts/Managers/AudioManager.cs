using PrimeTween;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip playerHit;
    public AudioClip playerDeath;
    public AudioClip levelUp;
    public AudioClip xpPickup;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip buttonClick;
    public const string MasterKey = "MasterVolume";
    public const string MusicKey = "MusicVolume";
    public const string SfxKey = "SFXVolume";

    public AudioMixer mixer;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }
    void Start()
    {
        ApplyVolume(MasterKey, GameSaveHandler.GetFloat(MasterKey));
        ApplyVolume(MusicKey, GameSaveHandler.GetFloat(MusicKey));
        ApplyVolume(SfxKey, GameSaveHandler.GetFloat(SfxKey));
    }

    public void ApplyVolume(string key, float value)
    {
        float dB;
        Debug.Log(value);
        if (value > 0.0f)
			dB = 20.0f * Mathf.Log10(value);
		else
			dB = -80.0f; Debug.Log("Is -80 db");
        mixer.SetFloat(key, dB);
    }
    public void PlaySfx(AudioClip clip) => sfxSource.PlayOneShot(clip);
    
    public void PitchDownMusic(float duration = 1.5f, float value = 0.6f)
    {
        Tween.Custom(
            startValue: musicSource.pitch,
            endValue: value,
            duration: duration,
            onValueChange: v => musicSource.pitch = v,
            ease: Ease.OutQuad,
            useUnscaledTime: true);
    }

    public void ResetMusicPitch() => musicSource.pitch = 1f;
    
}