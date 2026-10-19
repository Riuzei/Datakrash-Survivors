using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    [SerializeField] private int maxXP;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private List<ModifierInfo> modifiers = new();
    [SerializeField] private int choicesPerLevelUp = 3;
    [SerializeField] private Image xpFill; 
    [SerializeField] private Color glowColor = Color.cyan;

    public GameObject upgradeMenuObj;
    public readonly List<ModifierInfo.Upgrade> CurrentChoices = new();
    public UpgradeChoice[] weaponUpChoices = new UpgradeChoice[3];
    public UpgradeChoice[] passiveUpChoices = new UpgradeChoice[3];
    public Image[] weaponSlotIcons = new Image[3];
    public Image[] passiveSlotIcons = new Image[3];

    private Color baseColor;
    private Sequence glowTween;

#region Monobehavior
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
        levelText.text = "Lvl " + currentLevel;
        baseColor = xpFill.color;
        foreach(Image slotIcon in weaponSlotIcons)
        {
            slotIcon.enabled = false;
        }
        foreach(Image slotIcon in passiveSlotIcons)
        {
            slotIcon.enabled = false;
        }
    }
#endregion

#region XP System
    private void Glow()
    {
        if (glowTween.isAlive) glowTween.Stop();

        glowTween = Tween.Color(xpFill, glowColor, 0.1f).Chain(Tween.Color(xpFill, glowColor, baseColor, 0.9f)); 
    }
    public void GainXP(int xp)
    {
        Glow();
        Debug.Log(xp);
        currentXP += xp;
        if(currentXP >= maxXP)
        {
            LevelUp();
        }
        currentXP = Mathf.Clamp(currentXP, 0, maxXP);
        Tween.UISliderValue(xpBar, endValue: (float) currentXP / maxXP, duration: 0.5f, ease: Ease.OutCubic);
    }
#endregion

#region Upgrade System
    private void LevelUp()
    {
        currentXP -= maxXP;
        currentLevel++;
        levelText.text = "Lvl " + currentLevel;
        maxXP += 5;
        xpBar.value = 0f;
        RollChoices();
        GameplayManager.Instance.OnLevelUp();
    }
    private bool IsAvailable(ModifierInfo modifier, HeroBrain hero)
    {
        int level = hero.itemByModifier.TryGetValue(modifier, out IItem item) ? item.CurrentLevel : 0;
        return level < modifier.upgrades.Length;
    }
    public void SelectUpgrade(ModifierInfo modifier, int level)
    {
        HeroBrain hero = GameplayManager.Instance.player.currentHero;
        hero.ApplyModifier(modifier, level);
        GameplayManager.Instance.ResumeGame();
        
    }
    private void RollChoices()
    {
        CurrentChoices.Clear();
        HeroBrain hero = GameplayManager.Instance.player.currentHero;

        List<ModifierInfo> pool = new();
        foreach (ModifierInfo _modifier in modifiers)
        {
            if (IsAvailable(_modifier, hero)) pool.Add(_modifier);
        }
        
        for (int i = 0; i < choicesPerLevelUp; i++)
        {
            int index = Random.Range(0, pool.Count);
            ModifierInfo modifier = pool[index];
            pool.RemoveAt(index);

            int _level = hero.itemByModifier.TryGetValue(modifier, out IItem item) ? item.CurrentLevel : 0;
            ModifierInfo.Upgrade upgrade = modifier.upgrades[_level];
            CurrentChoices.Add(upgrade);
            bool isWeapon = modifier is WeaponModifier;
            UpgradeChoice upgradeChoice;
            if(!isWeapon)
            {
                passiveUpChoices[i].gameObject.SetActive(true);
                weaponUpChoices[i].gameObject.SetActive(false);
                upgradeChoice = passiveUpChoices[i];
            }
            else
            {
                passiveUpChoices[i].gameObject.SetActive(false);
                weaponUpChoices[i].gameObject.SetActive(true);
                upgradeChoice = weaponUpChoices[i];
            }
            
            upgradeChoice.button.onClick.RemoveAllListeners();

            if (i >= choicesPerLevelUp || pool.Count == 0)
            {
                upgradeChoice.button.gameObject.SetActive(false);
                continue;
            }

            upgradeChoice.button.gameObject.SetActive(true);
            upgradeChoice.icon.sprite = modifier.icon;
            upgradeChoice.nameText.text = modifier.name;
            upgradeChoice.descriptionText.text = upgrade.description;
            if(_level == 0)
            {
                upgradeChoice.levelText.text = isWeapon ? "NEW WEAPON" : "NEW PASSIVE";
            }
            else
                upgradeChoice.levelText.text = "LEVEL : " + _level;
            upgradeChoice.button.onClick.AddListener(() => SelectUpgrade(modifier, _level));
            upgradeChoice.button.onClick.AddListener(() => GameplayManager.Instance.ResumeGame());
        }
    }
#endregion
}
