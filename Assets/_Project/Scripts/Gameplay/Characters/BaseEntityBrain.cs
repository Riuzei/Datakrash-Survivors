using System;
using PrimeTween;
using UnityEngine;

public class BaseEntityBrain : MonoBehaviour, IDamageable, IUpdatable
{
    [field: SerializeField] public float currentHealth {get; protected set;} = 100;
    [field: SerializeField] public float maxHealth {get; protected set;} = 100;
    public SpriteRenderer spriteRenderer;

    [SerializeField] protected ParticleSystem deathVFX;
    
    protected Animator animator; 
    protected Rigidbody2D rb;

    void OnEnable() => GameplayManager.Instance.Register(this);
    void OnDisable() => GameplayManager.Instance.Unregister(this);
    
    public virtual void TakeDamage(float damage) 
    {
        currentHealth = Mathf.Max(0, currentHealth -= damage);

    }
    public virtual void OnFixedUpdate(float fixedDeltaTime){}
    public virtual void OnUpdate(float deltaTime){}
    public virtual void FlashColor(Color _color, float _duration)
    {
        spriteRenderer.color = _color;
        Tween.Color(spriteRenderer, endValue: Color.white, duration: _duration, ease: Ease.OutSine);
    }
    public virtual void Die()
    {
        if(deathVFX != null)
        {
            GameplayManager.Instance.vfxSpawner.SpawnVFX(deathVFX, transform.position);
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
