using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusSteelMaterial : BonusBase
{

    public override void SetUp()
    {
        spriteColor = Color.grey;
        textColor = Color.black;
        text = "Steel";
    }
    protected override void BonusActivate()
    {
        playerScript.ChangeBallsMaterial(BallMaterial.Steel);
    }
}
