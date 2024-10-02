using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public GameObject textObject;
    TMP_Text textComponent;
    public int hitsToDestroy;
    public int points;
    PlayerScript playerScript;
    public GameObject bonusPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<TMP_Text>();
            textComponent.text = hitsToDestroy.ToString();
        }
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hitsToDestroy--;
        if (hitsToDestroy == 0)
        {
            if (gameObject.name.StartsWith("GreenBlock"))
            {
                var bonus = Instantiate(bonusPrefab, transform.position, Quaternion.identity);
                var bonusBase = bonus.AddComponent<BonusBase>();
            }
            Destroy(gameObject);
            playerScript.BlockDestroyed(points);
        }
        else if (textComponent != null)
            textComponent.text = hitsToDestroy.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
