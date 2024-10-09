using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusBase : MonoBehaviour
{
    private float speed = -5;
    public Color spriteColor = Color.yellow;
    public Color textColor = Color.black;
    public string text = "+100";
    TMP_Text textComponent;
    PlayerScript playerScript;
    void Start()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        textComponent.text = text;
        textComponent.color = textColor;
        GetComponent<SpriteRenderer>().color = spriteColor;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, speed, 0);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall") {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            BonusActivate();
            Destroy(gameObject);
        }
    }

    virtual protected void BonusActivate()
    {
        playerScript.AddPoints(100);
    }
}
