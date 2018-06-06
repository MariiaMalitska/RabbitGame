using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectable
{
    protected override void OnRabbitHit(HeroController rabbit)
    {
        LevelController.current.addFruit(1);
        this.CollectedHide();
    }
}
