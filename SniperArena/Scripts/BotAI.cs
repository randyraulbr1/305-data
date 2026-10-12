using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Health))]
public class BotAI : MonoBehaviour
{
    public Transform target;
    public float speed = 3.5f, preferred = 20f, fireInterval = 1.8f, damage = 18f, hitChance = 0.35f, sight = 80f;

    CharacterController cc;
    Health hp, targetHp;
    float nextFire, nextStrafe, strafeDir = 1f;

    void Start() { cc = GetComponent<CharacterController>(); hp = GetComponent<Health>(); targetHp = target.GetComponent<Health>(); }

    void Update()
    {
        if (hp.IsDead || target == null || Time.timeScale == 0f) return;

        Vector3 to = target.position - transform.position; to.y = 0;
        float d = to.magnitude;
        Vector3 f = to.normalized;

        Vector3 move = Vector3.zero;
        if (d > preferred) move = f; else if (d < preferred * 0.6f) move = -f;
        if (Time.time > nextStrafe) { strafeDir = Random.value < 0.5f ? -1f : 1f; nextStrafe = Time.time + Random.Range(1f, 3f); }
        move += Vector3.Cross(Vector3.up, f) * strafeDir * 0.6f;
        cc.SimpleMove(move.normalized * speed);

        if (f.sqrMagnitude > 0.01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(f), 8f * Time.deltaTime);

        if (targetHp != null && !targetHp.IsDead && d < sight && Time.time >= nextFire)
        {
            Vector3 eye = transform.position + Vector3.up * 1.5f, tgt = target.position + Vector3.up * 1.2f;
            bool los = !Physics.Linecast(eye, tgt, out var hit, ~0, QueryTriggerInteraction.Ignore) || hit.transform.IsChildOf(target);
            if (los)
            {
                nextFire = Time.time + fireInterval;
                bool hitOk = Random.value < hitChance;
                Fx.Tracer(eye, hitOk ? tgt : tgt + Random.insideUnitSphere * 2f, new Color(1f, 0.4f, 0.1f));
                if (hitOk) targetHp.TakeDamage(damage, gameObject);
            }
        }
    }
}
