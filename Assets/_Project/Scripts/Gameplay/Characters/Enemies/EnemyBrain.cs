using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBrain : BaseEntityBrain
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] private EGemType dropTypes;
    
    public IObjectPool<EnemyBrain> EnemyPool { set => enemyPool = value; }
    public float DespawnDistanceSqr { get; set; }

    protected HeroBrain target;

    private float attackTimer;
    private bool isKnockedBack;
    private IObjectPool<EnemyBrain> enemyPool;
    private EnemyMovement movement;
    private CircleCollider2D circleCollider2D;

#region Monobehavior    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();
        target = GameplayManager.Instance.player.currentHero;
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        base.OnFixedUpdate(fixedDeltaTime);
        Vector2 offset = (Vector2)target.transform.position - rb.position;

        if (offset.sqrMagnitude > DespawnDistanceSqr)
        {
            Despawn();
            return;
        }
        if(isKnockedBack) return;
        Vector2 direction = movement.GetDirection(offset);
        if(direction.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        rb.MovePosition(rb.position + direction * moveSpeed * fixedDeltaTime);
    }
#endregion

#region Loot Drop RNG
    private EGemType PickDrop()
    {
        int count = 0;

        if (dropTypes.HasFlag(EGemType.NoDrop))
            count++;
        if (dropTypes.HasFlag(EGemType.Common))
            count++;
        if (dropTypes.HasFlag(EGemType.Rare))
            count++;
        if (dropTypes.HasFlag(EGemType.Legendary))
            count++;

        if (count == 0)
            return EGemType.None;

        int roll = Random.Range(0, count);

        if (dropTypes.HasFlag(EGemType.NoDrop))
        {
            if (roll == 0)
                return EGemType.None;
            roll--;
        }
        if (dropTypes.HasFlag(EGemType.Common))
        {
            if (roll == 0)
                return EGemType.Common;
            roll--;
        }
        if (dropTypes.HasFlag(EGemType.Rare))
        {
            if (roll == 0)
                return EGemType.Rare;
            roll--;
        }
        if (dropTypes.HasFlag(EGemType.Legendary))
        {
            if (roll == 0)
                return EGemType.Legendary;
        }

        return EGemType.None;
    }
#endregion

#region Movement
    protected virtual Vector2 GetMoveDirection(Vector2 toTarget)
    {
        return toTarget.normalized;
    }

#endregion

#region Combat
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            attackTimer = 0f;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            target.TakeDamage(damage);
            attackTimer = attackCooldown;
        }
    }

    public override void TakeDamage(float damage)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.enemyHit);
        GameplayManager.Instance.ShowDamageNumber(transform.position, damage);
        base.TakeDamage(damage);
        FlashColor(Color.red, 0.4f);
        StartCoroutine(Knockback());
    }
    
    private IEnumerator Knockback()
    {
        if(circleCollider2D != null)
            circleCollider2D.enabled = false;
        Vector2 direction = (target.transform.position - transform.position).normalized;
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(-direction * 10f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        isKnockedBack = false;
        if(circleCollider2D != null)
            circleCollider2D.enabled = true;
        if(currentHealth <= 0)
            Die();
    }
    public void Despawn()
    {
        enemyPool.Release(this);
    } 
    public override void Die()
    {
        base.Die();
        AudioManager.Instance.PlaySfx(AudioManager.Instance.enemyDeath);
        EGemType gem = PickDrop();
        if (gem != EGemType.None)
        {
            XPGem prefab = GameplayManager.Instance.GetGemObject(gem);
            GameplayManager.Instance.pickupSpawner.Spawn(prefab, transform.position);
        }
        Despawn();
    }
#endregion
}