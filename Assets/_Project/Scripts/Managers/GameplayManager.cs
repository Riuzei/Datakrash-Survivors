using System;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }
    public static event Action OnPlayerHit;
    
    public GamePrefabs gamePrefabs;
    public PlayerController player;
    public GameObject pauseMenuObj;
    public GameObject settingsObj;
    public GameObject gameOverObj;
    public Stopwatch stopwatchObj;
    public bool IsTicking = false;
    public PickupSpawner pickupSpawner;
    public VFXSpawner vfxSpawner;

    [SerializeField] private TextMeshProUGUI pauseHighScoreText;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreText;
    [SerializeField] private Material floorMat;
    [SerializeField] private TextMeshProUGUI dmgLabelPrefab;
    [SerializeField] private Canvas worldCanvas;

    private static readonly int FadeId = Shader.PropertyToID("_CriticalTransition");
    private HeroBrain currentHero;
    private GameStateMachine stateMachine;
    private readonly List<IUpdatable> fixedUpdatables = new();
    private readonly List<IUpdatable> updatables = new();
    private ObjectPool<TextMeshProUGUI> pool;
     
#region MonoBehavior
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        stateMachine = new GameStateMachine();
    }

    void Start()
    {
        Tween.MaterialProperty(floorMat, FadeId, endValue: 0f, duration: 0f);
        pool = new ObjectPool<TextMeshProUGUI>
        (
            () => Instantiate(dmgLabelPrefab, worldCanvas.transform),
            label => label.gameObject.SetActive(true),
            label => label.gameObject.SetActive(false)
        );
        stopwatchObj.gameObject.SetActive(true);
        currentHero = player.currentHero;
        if(currentHero == null) return;
        SpawnHero();
    }
    void Update()
    {
        if(!IsTicking) return;
        Tick();
    }
    void FixedUpdate()
    {
        if(!IsTicking) return;
        PhysicsTick();
    }
#endregion
    
#region CustomUpdate
    
    private void Tick()
    {
        for(int i = 0; i < updatables.Count; i++)
        {
            updatables[i].OnUpdate(Time.deltaTime);
        }
    }
    
    private void PhysicsTick()
    {
        for(int i = 0; i < fixedUpdatables.Count; i++)
        {
            fixedUpdatables[i].OnFixedUpdate(Time.fixedDeltaTime);
        }
    }
    public void Register(IUpdatable updateable)
    {
        fixedUpdatables.Add(updateable);
        updatables.Add(updateable);
    }
    public void Unregister(IUpdatable updateable)
    {
        fixedUpdatables.Remove(updateable);
        updatables.Remove(updateable);
    }
#endregion

#region Methods
    public XPGem GetGemObject(EGemType gemType)
    {
        return gamePrefabs.GetGem(gemType);
    }
    public void OnLevelUp()
    {
        if(stateMachine.Is(EGameState.Over)) return;
        stateMachine.SwitchState(EGameState.Upgrade);
    }
    public void OpenPauseMenu()
    {
        if(stateMachine.Is(EGameState.Over)) return;
        if(stateMachine.Is(EGameState.Upgrade)) return;
        stateMachine.SwitchState(EGameState.Pause);
        
        pauseHighScoreText.text = Stopwatch.Format(Stopwatch.HighScore);
    }
    public void OpenSettingsMenu()
    {
        pauseMenuObj.gameObject.SetActive(false);
        settingsObj.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        stateMachine.SwitchState(EGameState.Playing);
    }
    public void GameOver()
    {
        stopwatchObj.SaveHighScore();
        gameOverHighScoreText.text = Stopwatch.Format(Stopwatch.HighScore);
        stateMachine.SwitchState(EGameState.Over);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void SpawnHero()
    {
        currentHero.gameObject.SetActive(false);
        currentHero.ResetHealth();
        currentHero.UpdateHealthUI();
        currentHero.transform.position = Vector2.zero;
        currentHero.gameObject.SetActive(true);
    }

    public void SetCriticalHealth()
    {
        Tween.MaterialProperty(floorMat, FadeId, endValue: 1f, duration: 1f);
    }
    public void RemoveCriticalHealth()
    {
        Tween.MaterialProperty(floorMat, FadeId, endValue: 0f, duration: 1f);
    }
    public void ShowDamageNumber(Vector3 position, float damage)
    {
        TextMeshProUGUI label = pool.Get();
        label.transform.position = position;
        label.text = Mathf.RoundToInt(damage).ToString();
        label.alpha = 1f;
 
        Tween.PositionY(label.transform, endValue: position.y + 1f, duration: 0.6f);
        Tween.Alpha(label, endValue: 0f, duration: 0.6f).OnComplete(() => pool.Release(label));
    }
    public static void PlayerHit()
    {
        OnPlayerHit?.Invoke();
    }
#endregion

}
