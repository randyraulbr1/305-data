using UnityEngine;

// HUD simple con OnGUI: vida, municion, skin, kills, mira y hitmarker.
public class Hud : MonoBehaviour
{
    public static int Kills, Deaths;
    static float hitUntil; static bool hitHead;
    public static void Hitmarker(bool head) { hitUntil = Time.unscaledTime + 0.25f; hitHead = head; }

    public Health playerHealth;
    public SniperRifle rifle;
    public PlayerController player;

    void Rect(float x, float y, float w, float h, Color c) { GUI.color = c; GUI.DrawTexture(new Rect(x, y, w, h), Texture2D.whiteTexture); GUI.color = Color.white; }

    void OnGUI()
    {
        float W = Screen.width, H = Screen.height, cx = W / 2, cy = H / 2;
        GUI.skin.label.fontSize = Mathf.RoundToInt(H / 32f);

        if (player != null && player.IsAiming)
        {
            // Mira de francotirador: bordes negros y cruz
            float s = H * 0.45f;
            Color k = Color.black;
            Rect(0, 0, W, cy - s, k); Rect(0, cy + s, W, cy - s, k);
            Rect(0, cy - s, cx - s, 2 * s, k); Rect(cx + s, cy - s, cx - s, 2 * s, k);
            Rect(cx - s, cy - 1, 2 * s, 2, Color.black); Rect(cx - 1, cy - s, 2, 2 * s, Color.black);
        }
        else
        {
            Rect(cx - 10, cy - 1, 20, 2, Color.white); Rect(cx - 1, cy - 10, 2, 20, Color.white);
        }

        if (Time.unscaledTime < hitUntil)
        {
            Color c = hitHead ? Color.red : Color.white;
            Rect(cx - 14, cy - 14, 8, 2, c); Rect(cx + 6, cy + 12, 8, 2, c);
        }

        // Barra de vida
        float bw = W * 0.25f, bh = H * 0.03f, bx = W * 0.03f, by = H - bh - H * 0.05f;
        Rect(bx, by, bw, bh, new Color(0, 0, 0, 0.6f));
        if (playerHealth != null) Rect(bx, by, bw * playerHealth.current / playerHealth.max, bh, new Color(0.2f, 0.9f, 0.3f));

        if (rifle != null)
        {
            string ammo = rifle.Reloading ? "RECARGANDO..." : rifle.ammo + " / " + rifle.reserve;
            GUI.Label(new Rect(W - W * 0.28f, H - H * 0.1f, W * 0.26f, H * 0.08f), ammo);
            GUI.Label(new Rect(W - W * 0.28f, H - H * 0.15f, W * 0.26f, H * 0.08f), "Skin [K]: " + rifle.SkinName);
        }
        GUI.Label(new Rect(W * 0.03f, H * 0.03f, W * 0.4f, H * 0.08f), "Kills: " + Kills + "   Muertes: " + Deaths);
        if (playerHealth != null && playerHealth.IsDead)
            GUI.Label(new Rect(cx - 120, cy - 20, 300, 60), "ELIMINADO - reapareciendo...");
        if (Time.timeScale == 0f)
            GUI.Label(new Rect(cx - 60, cy - 20, 200, 60), "PAUSA (Esc)");
    }
}
