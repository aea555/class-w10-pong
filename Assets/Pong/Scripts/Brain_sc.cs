using System.Collections.Generic;
using UnityEngine;

public class Brain_sc : MonoBehaviour
{
    public GameObject paddle;
    public GameObject ball;
    public double learningRate = 0.11;
    public bool reflectOffTops = true;  // iyileştirme: tavandan seken topu da hesaba kat
    public float numSaved = 0;
    public float numMissed = 0;

    Rigidbody2D brb;
    float yvel;
    float paddleMinY = 8.8f;
    float paddleMaxY = 17.4f;
    float paddleMaxSpeed = 15;
    ANN ann;

    void Start()
    {
        // 6 giriş, 1 çıkış, 1 gizli katman, gizli katmanda 4 nöron
        ann = new ANN(6, 1, 1, 4, learningRate);
        brb = ball.GetComponent<Rigidbody2D>();
    }

    List<double> Run(double bx, double by, double bvx, double bvy,
                     double px, double py, double pv, bool train)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(bx);
        inputs.Add(by);
        inputs.Add(bvx);
        inputs.Add(bvy);
        inputs.Add(px);
        inputs.Add(py);
        outputs.Add(pv);
        if (train)
            return ann.Train(inputs, outputs);
        else
            return ann.CalcOutput(inputs, outputs);
    }

    void Update()
    {
        // Ağın çıktısı (-1, 1) aralığında. Raket bu oranla en fazla paddleMaxSpeed hızında
        // yalnızca y ekseninde hareket eder ve sahanın dışına çıkmaz.
        float posy = Mathf.Clamp(paddle.transform.position.y + (yvel * Time.deltaTime * paddleMaxSpeed),
                                 paddleMinY, paddleMaxY);
        paddle.transform.position = new Vector3(paddle.transform.position.x, posy, paddle.transform.position.z);

        // Topun gidiş yönünde bir ışın gönderilir. Işın yalnızca 9. katmana (backwall) çarpar.
        List<double> output = new List<double>();
        int layerMask = 1 << 9;
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.linearVelocity, 1000, layerMask);

        if (hit.collider != null)
        {
            // Işın üst ya da alt sınıra çarparsa yansıtılıp bir kez daha gönderilir.
            if (reflectOffTops && hit.collider.gameObject.CompareTag("tops"))
            {
                Vector3 reflection = Vector3.Reflect(brb.linearVelocity, hit.normal);
                hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
            }

            if (hit.collider != null && hit.collider.gameObject.CompareTag("backwall"))
            {
                // Topun kırmızı duvara varacağı nokta ile raketin arasındaki fark
                // ağa istenen çıktı olarak verilir ve ağ bu adımda eğitilir.
                float dy = hit.point.y - paddle.transform.position.y;

                output = Run(ball.transform.position.x,
                             ball.transform.position.y,
                             brb.linearVelocity.x, brb.linearVelocity.y,
                             paddle.transform.position.x,
                             paddle.transform.position.y,
                             dy, true);
                yvel = (float)output[0];
            }
        }
        else
            yvel = 0;
    }
}
