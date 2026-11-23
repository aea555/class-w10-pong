# results/ altındaki CSV'lerden öğrenme eğrisi grafiğini çizer.
import csv, sys
import matplotlib.pyplot as plt

out = sys.argv[1]
configs = [("0.11", "0", "öğrenme hızı 0.11"), ("0.11", "1", "öğrenme hızı 0.11 + yansıma"),
           ("0.001", "0", "öğrenme hızı 0.001"), ("0.001", "1", "öğrenme hızı 0.001 + yansıma")]
colors = ["#1f77b4", "#1f77b4", "#d62728", "#d62728"]
styles = ["-", "--", "-", "--"]
plt.figure(figsize=(8, 4.5))
for (lr, r, label), c, ls in zip(configs, colors, styles):
    rates = None
    for s in range(1, 31):
        rows = list(csv.DictReader(open(f"results/lr{lr}_reflect{r}_seed{s}.csv")))
        prev = (0.0, 0.0)
        cur = []
        for row in rows:
            S, M = float(row["saved"]), float(row["missed"])
            dS, dM = S - prev[0], M - prev[1]
            cur.append(dS / (dS + dM) if dS + dM else None)
            prev = (S, M)
        rates = [[] for _ in cur] if rates is None else rates
        for i, v in enumerate(cur):
            if v is not None:
                rates[i].append(v)
    t = [0.5 * (i + 1) for i in range(len(rates))]
    plt.plot(t, [100 * sum(x) / len(x) for x in rates], ls, color=c, label=label, marker="o", ms=3)
plt.xlabel("Oyun süresi (dakika)")
plt.ylabel("Yakalanan topların oranı (%)")
plt.title("30 saniyelik aralıklarda yakalama oranı (30 seed ortalaması)")
plt.ylim(0, 102)
plt.grid(alpha=0.3)
plt.legend(loc="lower right")
plt.tight_layout()
plt.savefig(out, dpi=150)
