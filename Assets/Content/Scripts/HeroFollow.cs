using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFollow : MonoBehaviour {

	public HeroController rabbit;
	
	void Update () {
		
		Transform rabit_transform = rabbit.transform;

		Transform camera_transform = this.transform;

		Vector3 rabit_position = rabit_transform.position;
		Vector3 camera_position = camera_transform.position;

		camera_position.x = rabit_position.x;
		camera_position.y = rabit_position.y;

		camera_transform.position = camera_position;
	}
}
