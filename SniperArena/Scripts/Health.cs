using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float max = 100f;
    public float current = 100f;
    public bool IsDead => current <= 0f;
    public event Action<Health, GameObject> Died;

    public void TakeDamage(float amount, GameObject source)
    {
        if (IsDead) return;
        current = Mathf.Max(0f, current - amount);
        if (IsDead) Died?.Invoke(this, source);
    }

    public void ResetHealth() { current = max; }
}
