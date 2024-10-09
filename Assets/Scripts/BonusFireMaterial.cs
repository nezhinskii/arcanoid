using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusFireMaterial : BonusBase
{

    public override void SetUp()
    {
        spriteColor = Color.red;
        textColor = Color.white;
        text = "Fire";
    }
    protected override void BonusActivate()
    {
        playerScript.ChangeBallsMaterial(BallMaterial.Fire);
    }
}
