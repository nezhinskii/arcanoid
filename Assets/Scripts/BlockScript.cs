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
        if (collision.gameObject.tag == "Ball")
        {

            hitsToDestroy -= collision.gameObject.GetComponent<BallScript>().damage;
            if (hitsToDestroy <= 0)
            {
                if (gameObject.name.StartsWith("GreenBlock"))
                {
                    var bonus = Instantiate(bonusPrefab, transform.position, Quaternion.identity);
                    BonusBase bonusBase;
                    int i = Random.Range(0, 4);
                    switch (i)
                    {
                        case 0:
                            bonusBase = bonus.AddComponent<BonusBase>();
                            break;
                        case 1:
                            bonusBase = bonus.AddComponent<BonusNormMaterial>();
                            break;
                        case 2:
                            bonusBase = bonus.AddComponent<BonusFireMaterial>();
                            break;
                        case 3:
                            bonusBase = bonus.AddComponent<BonusSteelMaterial>();
                            break;
                        default:
                            bonusBase = bonus.AddComponent<BonusBase>();
                            break;
                    }
                    bonusBase.SetUp();
                }
                Destroy(gameObject);
                playerScript.BlockDestroyed(points);
            }
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
