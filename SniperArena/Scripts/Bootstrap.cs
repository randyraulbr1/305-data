using System.Collections;
using UnityEngine;

// Pon este script en un GameObject vacio de una escena 3D y dale Play.
// Construye arena, jugador, camara, sniper, bots y HUD. Reemplaza luego los cubos por modelos reales.
public class Bootstrap : MonoBehaviour
{
    public int botCount = 5;
    public float arenaHalf = 40f;

    GameObject player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        BuildArena();
        player = BuildCharacter("Player", new Color(0.2f, 0.5f, 1f), out Renderer[] _);
        player.transform.position = new Vector3(0, 0.1f, -arenaHalf + 5);
        var pc = player.AddComponent<PlayerController>();
        var php = player.AddComponent<Health>();
        php.Died += OnDied;

        // Camara
        var cam = Camera.main;
        if (cam == null)
        {
            var cg = new GameObject("Main Camera"); cg.tag = "MainCamera";
            cam = cg.AddComponent<Camera>(); cg.AddComponent<AudioListener>();
        }
        var tpc = cam.gameObject.AddComponent<ThirdPersonCamera>();
        tpc.player = pc;

        // Sniper en la mano del jugador
        var hand = new GameObject("Hand").transform;
        hand.SetParent(player.transform, false);
        hand.localPosition = new Vector3(0.3f, 1.2f, 0.3f);
        Color c = Color.white;
        var parts = new[]
        {
            Fx.Part(PrimitiveType.Cube, hand, new Vector3(0, 0, 0.4f),    new Vector3(0.07f, 0.1f, 0.8f),  c),
            Fx.Part(PrimitiveType.Cube, hand, new Vector3(0, 0, 0.95f),   new Vector3(0.04f, 0.04f, 0.5f), c),
            Fx.Part(PrimitiveType.Cube, hand, new Vector3(0, 0.09f, 0.4f), new Vector3(0.05f, 0.06f, 0.25f), c),
        };
        var muzzle = new GameObject("Muzzle").transform;
        muzzle.SetParent(hand, false); muzzle.localPosition = new Vector3(0, 0, 1.25f);
        var rifle = player.AddComponent<SniperRifle>();
        rifle.cam = cam; rifle.owner = pc; rifle.muzzle = muzzle; rifle.tintParts = parts;

        var hud = gameObject.AddComponent<Hud>();
        hud.playerHealth = php; hud.rifle = rifle; hud.player = pc;

        for (int i = 0; i < botCount; i++) SpawnBot();
    }

    void Update()
    {
        if (GameInput.PausePressed)
        {
            bool pause = Time.timeScale != 0f;
            Time.timeScale = pause ? 0f : 1f;
            Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pause;
        }
    }

    void BuildArena()
    {
        if (FindFirstObjectByType<Light>() == null)
        {
            var l = new GameObject("Sun").AddComponent<Light>();
            l.type = LightType.Directional; l.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
        Block(new Vector3(0, -0.5f, 0), new Vector3(arenaHalf * 2, 1, arenaHalf * 2), new Color(0.35f, 0.4f, 0.35f));
        float t = 1f, h = 5f, s = arenaHalf * 2;
        Block(new Vector3(0, h / 2, arenaHalf), new Vector3(s, h, t), Color.gray);
        Block(new Vector3(0, h / 2, -arenaHalf), new Vector3(s, h, t), Color.gray);
        Block(new Vector3(arenaHalf, h / 2, 0), new Vector3(t, h, s), Color.gray);
        Block(new Vector3(-arenaHalf, h / 2, 0), new Vector3(t, h, s), Color.gray);
        Random.InitState(7);
        for (int i = 0; i < 14; i++)
        {
            var p = new Vector3(Random.Range(-arenaHalf + 6, arenaHalf - 6), 0, Random.Range(-arenaHalf + 12, arenaHalf - 6));
            float hh = Random.Range(1.5f, 4f);
            Block(p + Vector3.up * hh / 2, new Vector3(Random.Range(2f, 5f), hh, Random.Range(2f, 5f)), new Color(0.5f, 0.45f, 0.4f));
        }
    }

    void Block(Vector3 pos, Vector3 scale, Color col)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.SetParent(transform, false);
        go.transform.position = pos; go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = Fx.MakeMat(col);
    }

    GameObject BuildCharacter(string name, Color col, out Renderer[] rends)
    {
        var root = new GameObject(name);
        var cc = root.AddComponent<CharacterController>();
        cc.height = 1.8f; cc.radius = 0.4f; cc.center = new Vector3(0, 0.9f, 0);
        var body = Fx.Part(PrimitiveType.Capsule, root.transform, new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.9f, 0.7f), col);
        var head = Fx.Part(PrimitiveType.Sphere, root.transform, new Vector3(0, 1.62f, 0), Vector3.one * 0.3f, new Color(0.95f, 0.8f, 0.65f));
        rends = new[] { body, head };
        return root;
    }

    void SpawnBot()
    {
        var bot = BuildCharacter("Bot", new Color(0.85f, 0.2f, 0.2f), out Renderer[] _);
        Fx.Part(PrimitiveType.Cube, bot.transform, new Vector3(0.3f, 1.2f, 0.6f), new Vector3(0.06f, 0.08f, 1f), Color.black);
        var hp = bot.AddComponent<Health>(); hp.max = hp.current = 100f;
        hp.Died += OnDied;
        var ai = bot.AddComponent<BotAI>(); ai.target = player.transform;
        Teleport(bot, RandomSpawn());
    }

    Vector3 RandomSpawn() => new Vector3(Random.Range(-arenaHalf + 5, arenaHalf - 5), 0.1f, Random.Range(0, arenaHalf - 5));

    void Teleport(GameObject go, Vector3 pos)
    {
        var cc = go.GetComponent<CharacterController>();
        cc.enabled = false; go.transform.position = pos; cc.enabled = true;
    }

    void OnDied(Health victim, GameObject killer)
    {
        if (victim.gameObject == player) Hud.Deaths++;
        else if (killer == player) Hud.Kills++;
        StartCoroutine(Respawn(victim));
    }

    IEnumerator Respawn(Health v)
    {
        bool isPlayer = v.gameObject == player;
        v.gameObject.SetActive(false);
        yield return new WaitForSeconds(isPlayer ? 3f : 4f);
        v.ResetHealth();
        Teleport(v.gameObject, isPlayer ? new Vector3(0, 0.1f, -arenaHalf + 5) : RandomSpawn());
        v.gameObject.SetActive(true);
    }
}
