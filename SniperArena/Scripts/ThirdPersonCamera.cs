using UnityEngine;

// Sin apuntar: 3a persona sobre el hombro. Apuntando: 1a persona con zoom de mira.
[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    public PlayerController player;
    public Vector3 hipOffset = new Vector3(0.7f, 0f, -3.2f);
    public float hipFov = 60f, scopeFov = 12f, eyeHeight = 1.6f;

    Camera cam;
    float aimT;
    Renderer[] rends;

    void Start() { cam = GetComponent<Camera>(); rends = player.GetComponentsInChildren<Renderer>(); }

    void LateUpdate()
    {
        if (player == null) return;
        aimT = Mathf.MoveTowards(aimT, player.IsAiming ? 1f : 0f, Time.unscaledDeltaTime * 6f);

        Quaternion rot = Quaternion.Euler(player.Pitch, player.Yaw, 0f);
        Vector3 pivot = player.transform.position + Vector3.up * eyeHeight;
        Vector3 hipPos = pivot + rot * hipOffset;

        Vector3 d = hipPos - pivot;
        if (Physics.SphereCast(pivot, 0.2f, d.normalized, out var hit, d.magnitude, ~0, QueryTriggerInteraction.Ignore)
            && !hit.transform.IsChildOf(player.transform))
            hipPos = pivot + d.normalized * Mathf.Max(0.3f, hit.distance);

        Vector3 eyePos = pivot + rot * Vector3.forward * 0.3f;
        transform.SetPositionAndRotation(Vector3.Lerp(hipPos, eyePos, aimT), rot);
        cam.fieldOfView = Mathf.Lerp(hipFov, scopeFov, aimT);

        bool show = aimT < 0.9f;
        foreach (var r in rends) if (r != null) r.enabled = show;
    }
}
