using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusNormMaterial : BonusBase
{

    public override void SetUp()
    {
        spriteColor = Color.white;
        textColor = Color.black;
        text = "Norm";
    }
    protected override void BonusActivate()
    {
        playerScript.ChangeBallsMaterial(BallMaterial.Norm);
    }
}
