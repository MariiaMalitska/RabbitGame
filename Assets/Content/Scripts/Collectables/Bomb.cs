using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable
{
    protected override void OnRabbitHit(HeroController rabbit)
    {
        rabbit.becomeSmall();
        this.CollectedHide();
    }
}
