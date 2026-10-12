using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Crea y abre la escena con el Bootstrap lista para dar Play.
public static class SniperArenaSetup
{
    public static void Run()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        new GameObject("Bootstrap").AddComponent<Bootstrap>();
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/SniperArena.unity");
        EditorApplication.isPlaying = true;
    }
}
