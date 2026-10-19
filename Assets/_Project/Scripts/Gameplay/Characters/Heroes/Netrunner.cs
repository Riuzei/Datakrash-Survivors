using PrimeTween;
using UnityEngine;

public class Netrunner : HeroBrain
{
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); 
        healthBar.value = currentHealth / maxHealth;
        Tween.UISliderValue(healthBarShadow, endValue: healthBar.value, duration: 0.5f, ease: Ease.OutCubic);
        spriteRenderer.color = Color.red;
        Tween.Color(spriteRenderer, endValue: Color.white, duration: 0.5f, ease: Ease.OutSine);
        Tween.ShakeLocalPosition(healthUICanvas.transform, new Vector2(0.2f,0.1f), 0.3f);
    }

}
