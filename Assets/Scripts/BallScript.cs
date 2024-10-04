using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 ballInitialForce;
    Rigidbody2D rb;
    GameObject playerObj;
    float deltaX;
    AudioSource audioSrc;
    public AudioClip hitSound;
    public AudioClip loseSound;
    public GameDataScript gameData;

    public Sprite normSprite;
    public Sprite fireSprite;
    public Sprite steelSprite;

    public int damage;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        deltaX = transform.position.x;
        audioSrc = Camera.main.GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic)
            if (Input.GetButtonDown("Fire1"))
            {
                rb.isKinematic = false;
                rb.AddForce(ballInitialForce);
            }
            else
            {
                var pos = transform.position;
                pos.x = playerObj.transform.position.x + deltaX;
                transform.position = pos;
            }
        if (!rb.isKinematic && Input.GetKeyDown(KeyCode.J))
        {
            var v = rb.velocity;
            if (Random.Range(0, 2) == 0)
                v.Set(v.x - 0.1f, v.y + 0.1f);
            else
                v.Set(v.x + 0.1f, v.y - 0.1f);
            rb.velocity = v;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameData.sound)
            audioSrc.PlayOneShot(loseSound, 5);
        Destroy(gameObject);
        playerObj.GetComponent<PlayerScript>().BallDestroyed(GetInstanceID());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameData.sound)
            audioSrc.PlayOneShot(hitSound, 5);
    }

    public void ChangeMaterial(BallMaterial material)
    {
        switch (material)
        {
            case BallMaterial.Norm:
                spriteRenderer.sprite = normSprite;
                damage = 1;
                break;
            case BallMaterial.Fire:
                spriteRenderer.sprite = fireSprite;
                damage = 4;
                break;
            case BallMaterial.Steel:
                spriteRenderer.sprite = steelSprite;
                damage = 40;
                break;
        }
    }
}
