using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed = 5.5f;
    public int hp = 37;
    public Rigidbody2D bulletPrefab;
    public float fireWait = 0.18f;
    public float bulletSpeed = 3;
    float lastShot;
    public HudStuff hud;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();


        DontDestroyOnLoad(this);
        hp = 37;
    }

    void Update()
    {
        // ============================================================
        // DIAGNOSTIKA DEV2-02 — POHYB CHÝBA (zámerne)
        // Doplň: Horizontal / Vertical (Input Manager OK na tento task)
        // alebo Input System. Posuň transform. Pozri README.
        // ============================================================
        /*
        */

        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);

        // streľba ostáva — overíš, že Play beží, aj keď sa ešte nehýbeš
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > lastShot + fireWait)
            {
                lastShot = Time.time;
                shoot();
            }
        }

        // also write hud from here because gm is laggy sometimes??
        var hpGo = GameObject.Find("HPText");
        if (hpGo != null)
        {
            hpGo.GetComponent<Text>().text = "hp " + hp;
        }
        hud = FindObjectOfType<HudStuff>();
        if (hud != null)
        {
            hud.upd("hp " + hp);
        }

        var g = FindObjectOfType<gm>();
        if (g != null)
        {
            g.HP = hp;
        }
    }
    void shoot()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.linearVelocity = direction.normalized * bulletSpeed;

        /*Camera camera = Camera.main;
        Vector2 targetPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(targetPos);
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.linearVelocity = targetPos;
        */

        /*try
        {
            var b = Instantiate(prefab, transform.position, Quaternion.identity);
            b.transform.parent = null;
        }
        catch
        {
            GameObject b = new GameObject("bullet");
            b.transform.position = transform.position;
            b.transform.parent = null;
            var sr = b.AddComponent<SpriteRenderer>();
            var my = GetComponent<SpriteRenderer>();
            if (my != null) sr.sprite = my.sprite;
            sr.color = new Color(1f, 1f, 0.2f, 1f);
            sr.sortingOrder = 10;
            var rb = b.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(lastDir * 12f, 0f);
            var col = b.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.12f;
            Destroy(b, 1.6f);
        }*/
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.GetComponent<eNemy>() != null || c.gameObject.GetComponent<eNemy2>() != null)
        {
            hp = hp - 4;
            var g = GameObject.FindObjectOfType<gm>();
            if (g != null) g.hitPlayer(0);
            var hpGo = GameObject.Find("HPText");
            if (hpGo != null) hpGo.GetComponent<Text>().text = "hp " + hp;
        }
    }
}
