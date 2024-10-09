using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusBase : MonoBehaviour
{
    public Color spriteColor;
    public Color textColor;
    public string text;
    private float speed = -5;
    TMP_Text textComponent;
    protected PlayerScript playerScript;

    virtual public void SetUp()
    {
        spriteColor = Color.yellow;
        textColor = Color.black;
        text = "+100";
    }
    
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
