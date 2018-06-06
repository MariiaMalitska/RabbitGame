using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	bool hideAnimation=false;
    protected virtual void OnRabbitHit(HeroController rabbit)
    {
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!this.hideAnimation)
        {
            HeroController rabbit = collider.GetComponent<HeroController>();
            if (rabbit != null)
            {
                this.OnRabbitHit(rabbit);
            }
        }
    }
    public void CollectedHide()
    {
        Destroy(this.gameObject);
    }
}
