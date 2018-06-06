using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    protected override void OnRabbitHit(HeroController rabbit)
    {
        LevelController.current.addCoins(1);
        this.CollectedHide();
    }
}
