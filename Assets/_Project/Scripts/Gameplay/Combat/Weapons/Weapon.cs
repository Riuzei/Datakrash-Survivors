using UnityEngine;

public class Weapon : MonoBehaviour, IItem, IUpdatable
{
    public ModifierInfo modifier;
    public int currentLevel = 0;
    public float damage;
    public float area;
    public float duration;
    public float speed;
    public float cooldown;
    public int amount;
    public bool isAttacking;
    public HeroBrain owner;
    
    ModifierInfo IItem.Modifier => modifier;
    int IItem.CurrentLevel => currentLevel;
    
    public virtual void Awake()
    {
        owner = GetComponentInParent<HeroBrain>();
    }
    void OnEnable() => GameplayManager.Instance.Register(this);
    void OnDisable() => GameplayManager.Instance.Unregister(this);
    public void LevelUp() => currentLevel++;

    public void Apply(ModifierInfo.Upgrade upgrade)
    {
        switch(upgrade.type)
        {
            case EUpgradeType.None:
                break;
            case EUpgradeType.Damage:
                damage *= 1 + upgrade.value/100;
                break;
            case EUpgradeType.Area:
                area *= 1 + upgrade.value/100;
                break;
            case EUpgradeType.Duration:
                duration *= 1 + upgrade.value/100;
                break;
            case EUpgradeType.Speed:
                speed *= 1 + upgrade.value/100;
                break;
            case EUpgradeType.Amount:
                amount = Mathf.RoundToInt(amount + upgrade.value);
                OnAmountChanged();
                break;
        }
    }
    protected virtual void OnAmountChanged(){}
    public virtual void OnUpdate(float deltaTime)
    {

    }
    public virtual void OnFixedUpdate(float deltaTime) {}

}