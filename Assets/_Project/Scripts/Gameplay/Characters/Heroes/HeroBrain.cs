using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBrain : BaseEntityBrain
{
    [SerializeField] protected Canvas healthUICanvas;
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected Slider healthBarShadow;
    [SerializeField] protected Weapon[] weapons = new Weapon[5];
    [SerializeField] protected PassiveItem[] passiveItems = new PassiveItem[5];
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float armor = 1;
    [SerializeField] protected float attackPower = 1f;
    
    public Vector2 moveInput;
    public readonly Dictionary<ModifierInfo, IItem> itemByModifier = new();
    
    private bool isMoving;
    private ParticleSystem healVFX;
    
#region MonoBehaviour    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        healVFX = Instantiate(GameplayManager.Instance.gamePrefabs.healVFX, transform.position, Quaternion.identity, transform);
    }
    void Start()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon == null || weapon.modifier == null) continue;
            itemByModifier[weapon.modifier] = weapon;
            weapon.currentLevel = 1;
            SetWeaponSlotIcon(0, weapon.modifier.icon);
        }
    }
    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        base.OnFixedUpdate(fixedDeltaTime);
        rb.MovePosition(rb.position + moveInput * moveSpeed * fixedDeltaTime); 
    }
#endregion

#region Combat
    protected virtual void Attack(Weapon weapon){}
    public override void TakeDamage(float damage)
    {
        float dmgTaken = Mathf.Max(1f, damage - armor);
        currentHealth = Mathf.Max(0, currentHealth - dmgTaken);
        AudioManager.Instance.PlaySfx(AudioManager.Instance.playerHit);
        GameplayManager.PlayerHit();
        if(currentHealth <= 0.15 * maxHealth)
            GameplayManager.Instance.SetCriticalHealth();
        if(currentHealth <= 0)
            Die();
    }
    public void UpdateHealthUI()
    {
        RectTransform rect = (RectTransform)healthBar.transform;
        RectTransform rectShadow = (RectTransform)healthBarShadow.transform;
        rectShadow.sizeDelta = rect.sizeDelta = new Vector2(maxHealth, rect.sizeDelta.y);
        healthBarShadow.value = healthBar.value = currentHealth / maxHealth;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        UpdateHealthUI();
        healVFX.Play();
        if(currentHealth >= 0.15 * maxHealth)
            GameplayManager.Instance.RemoveCriticalHealth();
    }
    
    [ContextMenu("Kill Hero")]
    public override void Die()
    {
        base.Die();
        animator.Play("Death");
        GameplayManager.Instance.GameOver();
        AudioManager.Instance.PlaySfx(AudioManager.Instance.playerDeath);
    }
#endregion

#region Upgrade System
    void SetWeaponSlotIcon(int index, Sprite icon)
    {
        ProgressionManager.Instance.weaponSlotIcons[index].enabled = true;
        ProgressionManager.Instance.weaponSlotIcons[index].sprite = icon;
    }
    void SetPassiveSlotIcon(int index, Sprite icon)
    {
        ProgressionManager.Instance.passiveSlotIcons[index].enabled = true;
        ProgressionManager.Instance.passiveSlotIcons[index].sprite = icon;
    }
    

    public void HandleInput(Vector2 _moveInput)
    {
        moveInput = _moveInput;
        isMoving = moveInput.sqrMagnitude > 0.01f;
        
        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        
        if (isMoving)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }
    public void AddItem(ModifierInfo _modifier)
    {
        if(_modifier is WeaponModifier modifier)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null) continue;
                Weapon weapon = Instantiate(modifier.weaponPrefab, transform);
                weapon.damage *= attackPower;
                weapon.currentLevel = 1;
                weapons[i] = weapon;
                SetWeaponSlotIcon(i, _modifier.icon);
                itemByModifier[modifier] = weapon;
                return;
            }
        }
        else if(_modifier is PassiveModifier passiveModifier)
        {
            for (int i = 0; i < passiveItems.Length; i++)
            {
                if (passiveItems[i] != null) continue;

                PassiveItem passiveItem = new PassiveItem(passiveModifier, 1);
                passiveItems[i] = passiveItem;
                SetPassiveSlotIcon(i, _modifier.icon);
                itemByModifier[passiveModifier] = passiveItem;
                ModifierInfo.Upgrade upgrade = passiveModifier.upgrades[0];
                switch (upgrade.type)
                {
                    case EUpgradeType.Damage:
                        MultiplyDamage(1f + upgrade.value / 100f);
                        break;
                    case EUpgradeType.MoveSpeed:
                        moveSpeed *= ConvertPercentage(upgrade.value);
                        break;
                    case EUpgradeType.MaxHealth:
                        maxHealth *= ConvertPercentage(upgrade.value);
                        UpdateHealthUI();
                        break;
                    case EUpgradeType.Armor:
                        armor *= ConvertPercentage(upgrade.value);
                        break;
                    default:
                        break;
                }
                return;
            }
        }
    }
    public void ApplyModifier(ModifierInfo modifier, int level)
    {
        ModifierInfo.Upgrade upgrade = modifier.upgrades[level];
        if (itemByModifier.TryGetValue(modifier, out IItem item))
        {
            if(item is Weapon _weapon)
            {
                _weapon.LevelUp();
                _weapon.Apply(upgrade);
            }
            else if(item is PassiveItem passiveItem)
            {
                passiveItem.LevelUp();
                switch (upgrade.type)
                {
                    case EUpgradeType.Damage:
                        MultiplyDamage(1f + upgrade.value / 100f);
                        break;
                    case EUpgradeType.MoveSpeed:
                        moveSpeed *= ConvertPercentage(upgrade.value);
                        break;
                    case EUpgradeType.MaxHealth:
                        maxHealth *= ConvertPercentage(upgrade.value);
                        UpdateHealthUI();
                        break;
                    case EUpgradeType.Armor:
                        armor *= ConvertPercentage(upgrade.value);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            AddItem(modifier);
        }
    }
    public void MultiplyDamage(float mult)
    {
        attackPower *= mult;
        foreach (Weapon weapon in weapons)
        {
            if (weapon == null) continue;
            weapon.damage *= mult;
        }
    }

    private float ConvertPercentage(float value)
    {
        value = 1f + value/100f;
        return value;
    }
#endregion

}
