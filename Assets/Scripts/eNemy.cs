using UnityEngine;

public class eNemy : MonoBehaviour
{
    public float speed = 2.4f;
    public int hp = 3;
    public int scoreToAdd;
    gm gameManager;
    float hitCd;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<gm>();
    }
    void Update()
    {
        var p = GameObject.Find("player");
        if (p == null)
        {
            p = FindAnyObjectByType<player>() != null ? FindAnyObjectByType<player>().gameObject : null;
        }
        if (p != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, p.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        if (other.gameObject.name == "player" || other.GetComponent<player>() != null)
        {
            if (Time.time < hitCd) return;
            hitCd = Time.time + 0.4f;
            var g = FindAnyObjectByType<gm>();
            if (g != null) g.hitPlayer(7);
        }

        if (other.gameObject.name == "Bullet(Clone)" || other.gameObject.name.Contains("Bullet(Clone)"))
        {
            hp = hp - 1;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                // ============================================================
                // DIAGNOSTIKA DEV2-05 — SKÓRE NENAPOJENÉ (zámerne)
                // Po opravenej kolízii (DEV2-03) enemy zomrie, ale score
                // nerastie, kým nezavoláš gm.addScore / napojíš ScoreText.
                // ============================================================
                gameManager.addScore(scoreToAdd);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.GetComponent<player>() != null)
        {
            if (Time.time < hitCd) return;
            hitCd = Time.time + 0.55f;
            var g = FindAnyObjectByType<gm>();
            if (g != null) g.hitPlayer(3);
        }
    }
}
