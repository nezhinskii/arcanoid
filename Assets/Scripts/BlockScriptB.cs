using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockScriptB : BlockScript
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbody2d;
    private Vector2 direction = Vector2.left;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    override public void Start()
    {
        base.Start();
        rigidbody2d.velocity = direction * speed;
    }

    override public void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Block");
        base.OnCollisionEnter2D(collision);
    }

    private void ChangeDirection(bool makeChange)
    {
        if (makeChange)
        {
            direction = -direction;
            Vector2 velocity = rigidbody2d.velocity;
            velocity.x = direction.x * speed;
            rigidbody2d.velocity = velocity;
        }
    }
}
