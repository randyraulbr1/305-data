using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f, sprintSpeed = 8f, aimSpeed = 2f, jumpSpeed = 6f;
    public float sensitivity = 1f, aimSensitivityMul = 0.35f;
    [HideInInspector] public float Yaw, Pitch;
    [HideInInspector] public bool IsAiming;

    CharacterController cc;
    float vy;

    void Awake() { cc = GetComponent<CharacterController>(); Yaw = transform.eulerAngles.y; }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        IsAiming = GameInput.AimHeld;
        Vector2 look = GameInput.Look * sensitivity * (IsAiming ? aimSensitivityMul : 1f);
        Yaw += look.x;
        Pitch = Mathf.Clamp(Pitch - look.y, -60f, 70f);

        Vector2 m = GameInput.Move;
        Vector3 dir = Quaternion.Euler(0, Yaw, 0) * new Vector3(m.x, 0, m.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();
        float speed = IsAiming ? aimSpeed : (GameInput.SprintHeld ? sprintSpeed : walkSpeed);

        if (cc.isGrounded) { vy = -1f; if (GameInput.JumpPressed) vy = jumpSpeed; }
        else vy += Physics.gravity.y * Time.deltaTime;

        cc.Move((dir * speed + Vector3.up * vy) * Time.deltaTime);

        // Apuntando el cuerpo mira a la camara; si no, mira hacia donde camina.
        if (IsAiming || dir.sqrMagnitude > 0.01f)
        {
            float target = IsAiming ? Yaw : Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, target, 0), 15f * Time.deltaTime);
        }
    }
}
