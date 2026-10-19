using System.Collections;
using UnityEngine;

public class Monowire : Weapon
{
    [SerializeField] ParticleSystem[] atkVFX;
    [SerializeField] Collider2D[] hitBoxes;
    
    private float timer;

    public override void Awake()
    {
        base.Awake();

    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target == null) return;

        target.TakeDamage(damage);
    }
    
    public override void OnUpdate(float deltaTime)
    {
        timer -= deltaTime;
        if(timer > 0) return;

        if (isAttacking)
        {
            hitBoxes[0].enabled = false;
            hitBoxes[1].enabled = false;
            isAttacking = false;
            timer = cooldown;
        }
        else
        {
            transform.localScale = owner.spriteRenderer.flipX ? -1 * Vector3.one : Vector3.one;
            StartCoroutine(Attack());
            isAttacking = true;
            timer = duration;
        }
    }
    public IEnumerator Attack()
    {
        hitBoxes[0].enabled = true;
        atkVFX[0].Play();
        if(amount > 1)
        {
            yield return new WaitForSeconds(0.3f);
            hitBoxes[1].enabled = true;
            atkVFX[1].Play();
        }
    }
}