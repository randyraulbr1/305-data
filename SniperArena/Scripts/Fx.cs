using UnityEngine;

// Helpers visuales. Usa shaders que siempre existen para evitar materiales magenta.
public static class Fx
{
    public static Material MakeMat(Color c)
    {
        var sh = Shader.Find("Universal Render Pipeline/Lit");
        if (sh == null) sh = Shader.Find("Standard");
        if (sh == null) sh = Shader.Find("Sprites/Default");
        var m = new Material(sh);
        m.color = c;
        return m;
    }

    public static Renderer Part(PrimitiveType t, Transform parent, Vector3 pos, Vector3 scale, Color c)
    {
        var go = GameObject.CreatePrimitive(t);
        Object.Destroy(go.GetComponent<Collider>());
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        var r = go.GetComponent<Renderer>();
        r.sharedMaterial = MakeMat(c);
        return r;
    }

    public static void Tracer(Vector3 a, Vector3 b, Color c)
    {
        var go = new GameObject("Tracer");
        var lr = go.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = c;
        lr.startWidth = lr.endWidth = 0.03f;
        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
        Object.Destroy(go, 0.08f);
    }
}
