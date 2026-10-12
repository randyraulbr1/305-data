using System;
using System.Collections;
using UnityEngine;

public class SniperRifle : MonoBehaviour
{
    [Serializable] public struct Skin { public string name; public Color color; }

    public Camera cam;
    public PlayerController owner;
    public Transform muzzle;
    public Renderer[] tintParts;

    public float damage = 70f, headshotMul = 2f, cycleTime = 1.3f, reloadTime = 2.2f, range = 500f, hipSpread = 2.5f;
    public int magSize = 5, ammo = 5, reserve = 30;
    public bool Reloading { get; private set; }

    public Skin[] skins =
    {
        new Skin { name = "Rosa Neon",   color = new Color(1f, 0.2f, 0.6f) },
        new Skin { name = "Oro",         color = new Color(1f, 0.78f, 0.2f) },
        new Skin { name = "Negro Mate",  color = new Color(0.08f, 0.08f, 0.1f) },
        new Skin { name = "Azul Hielo",  color = new Color(0.4f, 0.8f, 1f) },
        new Skin { name = "Rojo Sangre", color = new Color(0.7f, 0.05f, 0.05f) },
    };
    public int SkinIndex { get; private set; }
    public string SkinName => skins[SkinIndex].name;

    float nextShot;

    void Start() { ApplySkin(0); }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (GameInput.SkinPressed) ApplySkin(SkinIndex + 1);
        if (GameInput.ReloadPressed && !Reloading && ammo < magSize && reserve > 0) StartCoroutine(Reload());
        if (GameInput.FirePressed && Time.time >= nextShot && !Reloading)
        {
            if (ammo > 0) Fire();
            else if (reserve > 0) StartCoroutine(Reload());
        }
    }

    public void ApplySkin(int i)
    {
        SkinIndex = ((i % skins.Length) + skins.Length) % skins.Length;
        foreach (var r in tintParts) if (r != null) r.sharedMaterial.color = skins[SkinIndex].color;
    }

    void Fire()
    {
        ammo--;
        nextShot = Time.time + cycleTime;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!owner.IsAiming)
            ray.direction = Quaternion.Euler(UnityEngine.Random.Range(-hipSpread, hipSpread), UnityEngine.Random.Range(-hipSpread, hipSpread), 0) * ray.direction;

        Vector3 end = ray.origin + ray.direction * range;
        var hits = Physics.RaycastAll(ray, range, ~0, QueryTriggerInteraction.Ignore);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var h in hits)
        {
            if (h.collider.transform.IsChildOf(owner.transform)) continue;
            end = h.point;
            var hp = h.collider.GetComponentInParent<Health>();
            if (hp != null && !hp.IsDead)
            {
                bool head = h.point.y > h.collider.bounds.max.y - 0.35f;
                hp.TakeDamage(damage * (head ? headshotMul : 1f), owner.gameObject);
                Hud.Hitmarker(head);
            }
            break;
        }

        Fx.Tracer(muzzle != null ? muzzle.position : ray.origin, end, skins[SkinIndex].color);
        owner.Pitch -= 1.5f; // retroceso
        if (ammo == 0 && reserve > 0) StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        Reloading = true;
        yield return new WaitForSeconds(reloadTime);
        int take = Mathf.Min(magSize - ammo, reserve);
        ammo += take; reserve -= take;
        Reloading = false;
    }
}
