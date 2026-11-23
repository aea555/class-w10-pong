using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SetupScene
{
    public static void Run()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Pong/Scenes/Pong.unity");
        var ball = GameObject.Find("Ball");
        var paddle = GameObject.Find("Paddle");

        var mb = ball.GetComponent<MoveBall_sc>() ?? ball.AddComponent<MoveBall_sc>();
        foreach (var a in ball.GetComponents<AudioSource>())
        {
            if (a.clip.name == "blip") mb.blip = a;
            if (a.clip.name == "blop") mb.blop = a;
        }

        var brainGo = GameObject.Find("Brain") ?? new GameObject("Brain");
        var brain = brainGo.GetComponent<Brain_sc>() ?? brainGo.AddComponent<Brain_sc>();
        brain.paddle = paddle;
        brain.ball = ball;
        var log = brainGo.GetComponent<ExperimentLogger>() ?? brainGo.AddComponent<ExperimentLogger>();
        log.brain = brain;

        EditorSceneManager.SaveScene(scene);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Pong/Scenes/Pong.unity", true) };
        AssetDatabase.SaveAssets();
    }

    public static void Build()
    {
        Run();
        BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { "Assets/Pong/Scenes/Pong.unity" },
            locationPathName = "Build/Pong.app",
            target = BuildTarget.StandaloneOSX,
        });
    }
}
