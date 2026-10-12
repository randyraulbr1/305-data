using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Lee teclado/mouse con el Input System nuevo o con el viejo, segun lo que tenga el proyecto.
public static class GameInput
{
#if ENABLE_INPUT_SYSTEM
    static bool K(System.Func<Keyboard, UnityEngine.InputSystem.Controls.KeyControl> f, bool down)
    {
        var k = Keyboard.current; if (k == null) return false;
        var c = f(k); return down ? c.wasPressedThisFrame : c.isPressed;
    }
    public static Vector2 Move => new Vector2((K(k => k.dKey, false) ? 1 : 0) - (K(k => k.aKey, false) ? 1 : 0),
                                              (K(k => k.wKey, false) ? 1 : 0) - (K(k => k.sKey, false) ? 1 : 0));
    public static Vector2 Look => Mouse.current != null ? Mouse.current.delta.ReadValue() * 0.1f : Vector2.zero;
    public static bool SprintHeld => K(k => k.leftShiftKey, false);
    public static bool JumpPressed => K(k => k.spaceKey, true);
    public static bool ReloadPressed => K(k => k.rKey, true);
    public static bool SkinPressed => K(k => k.kKey, true);
    public static bool PausePressed => K(k => k.escapeKey, true);
    public static bool FirePressed => Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
    public static bool AimHeld => Mouse.current != null && Mouse.current.rightButton.isPressed;
#else
    public static Vector2 Move => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public static Vector2 Look => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 2f;
    public static bool SprintHeld => Input.GetKey(KeyCode.LeftShift);
    public static bool JumpPressed => Input.GetKeyDown(KeyCode.Space);
    public static bool ReloadPressed => Input.GetKeyDown(KeyCode.R);
    public static bool SkinPressed => Input.GetKeyDown(KeyCode.K);
    public static bool PausePressed => Input.GetKeyDown(KeyCode.Escape);
    public static bool FirePressed => Input.GetMouseButtonDown(0);
    public static bool AimHeld => Input.GetMouseButton(1);
#endif
}
