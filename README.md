# Pong

Pin-pon oynayan yapay sinir ağı. Proje Unity 6000.6.2f1 ile açılır, sahne `Assets/Pong/Scenes/Pong.unity`.

Deneyler için Editor kapalıyken macOS build alınır:

```bash
/Applications/Unity/Hub/Editor/6000.6.2f1/Unity.app/Contents/MacOS/Unity -batchmode -quit -projectPath . -executeMethod SetupScene.Build  # Build/Pong.app oluşur
```

Her koşu ayrı çalıştırılır. Aşağıdaki komut 0.11 öğrenme hızıyla, yansıma açıkken, 1 numaralı seed ile 10 dakikalık bir oyun oynatır:

```bash
Build/Pong.app/Contents/MacOS/class-w10-pong -batchmode -nographics -experiment -lr 0.11 -reflect 1 -seed 1 -duration 600 -out $PWD/results/lr0.11_reflect1_seed1.csv
```

Grafik için:

```bash
uv run --with matplotlib python tools/plot.py ogrenme-egrisi.png  # results/ klasöründeki 120 koşudan grafiği çizer
```
