using UnityEngine;

public class MoveBall_sc : MonoBehaviour
{
    public AudioSource blip;
    public AudioSource blop;

    Vector3 ballStartPosition;
    Rigidbody2D rb;
    float speed = 400;
    Brain_sc brain;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        brain = FindFirstObjectByType<Brain_sc>();
        ballStartPosition = transform.position;
        ResetBall();
    }

    // Top kırmızı duvara çarparsa kaçırılmış, rakete çarparsa yakalanmış sayılır.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("backwall"))
        {
            blop.Play();
            brain.numMissed++;
        }
        else
        {
            blip.Play();
            if (col.gameObject.name == "Paddle")
                brain.numSaved++;
        }
    }

    // Top başlangıç noktasına konur ve sağa doğru rastgele bir açıyla itilir.
    public void ResetBall()
    {
        transform.position = ballStartPosition;
        rb.linearVelocity = Vector2.zero;
        Vector3 dir = new Vector3(Random.Range(100, 300), Random.Range(-100, 100), 0).normalized;
        rb.AddForce(dir * speed);
    }
}
