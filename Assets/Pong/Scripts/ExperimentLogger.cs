using System.IO;
using UnityEngine;

// Deneyler için. Komut satırından öğrenme hızı, iyileştirme, seed ve süre alır,
// her 30 saniyede yakalanan ve kaçırılan top sayısını CSV'ye yazar.
public class ExperimentLogger : MonoBehaviour
{
    public Brain_sc brain;
    float duration = 600;
    float nextLog = 30;
    string outPath;

    void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        if (System.Array.IndexOf(args, "-experiment") < 0) { enabled = false; return; }
        int seed = 0;
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-lr") brain.learningRate = double.Parse(args[i + 1], System.Globalization.CultureInfo.InvariantCulture);
            if (args[i] == "-reflect") brain.reflectOffTops = args[i + 1] == "1";
            if (args[i] == "-seed") seed = int.Parse(args[i + 1]);
            if (args[i] == "-duration") duration = float.Parse(args[i + 1]);
            if (args[i] == "-out") outPath = args[i + 1];
        }
        Random.InitState(seed);
        Time.captureFramerate = 60;  // her kare 1/60 saniye sayılır, oyun bekleme olmadan hızlı akar
        File.WriteAllText(outPath, "time,saved,missed\n");
    }

    void Update()
    {
        if (Time.time >= nextLog)
        {
            File.AppendAllText(outPath, $"{nextLog},{brain.numSaved},{brain.numMissed}\n");
            nextLog += 30;
        }
        if (Time.time >= duration) Application.Quit();
    }
}
