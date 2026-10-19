using UnityEngine;

public class BlackICE : Weapon
{
    [SerializeField] private Transform[] blades;
    
    private float timer;
    private float angle;
    private Rigidbody2D rb;
    private int ActiveCount => Mathf.Clamp(amount, 1, blades.Length);

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        UpdateBlades();
    }

    protected override void OnAmountChanged()
    {
        UpdateBlades();
    }

    private void UpdateBlades()
    {
        int active = ActiveCount;
        for (int i = 0; i < blades.Length; i++)
        {
            blades[i].gameObject.SetActive(i < active);
        }
    }
    private void DisableBlades()
    {
        for (int i = 0; i < blades.Length; i++)
        {
            blades[i].gameObject.SetActive(false);
        }
    }
    public override void OnUpdate(float deltaTime) 
    {
        timer -= deltaTime;
        if(timer > 0) return;

        if (isAttacking)
        {
            isAttacking = false;
            DisableBlades();
            timer = cooldown;
        }
        else
        {
            isAttacking = true;
            UpdateBlades();
            timer = duration;
        }
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        angle += speed * fixedDeltaTime;

        rb.MoveRotation(angle);

        int active = ActiveCount;
        for (int i = 0; i < active; i++)
        {
            float offsetAngle = 360f * i / active;
            float rad = offsetAngle * Mathf.Deg2Rad;

            blades[i].localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * area;
            blades[i].localRotation = Quaternion.Euler(0f, 0f, offsetAngle);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target == null)
            return;
        target.TakeDamage(damage);
    }
}