using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHere : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
	{
		HeroController rabbit = collider.GetComponent<HeroController>();
		if (rabbit!=null)
			LevelController.current.onRabbitDeath(rabbit);
	}
}
