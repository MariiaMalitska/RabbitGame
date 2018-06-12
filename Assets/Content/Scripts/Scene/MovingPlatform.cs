using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 MoveBy;
    Vector3 pointA;
    Vector3 pointB;
    int going_to_b = 1;
    public float speedx = 0.05f;
    public float speedy = 0;
    public float start_time_to_wait = 1f;
    float time_to_wait = 1f;

    void Start()
    {
        this.pointA = this.transform.position;
        this.pointB = this.pointA + MoveBy;
    }

    void Update()
    {
        Vector3 my_pos = this.transform.position;
        Vector3 target;
        if (going_to_b < 0)
        {
            target = this.pointA;
        }
        else
        {
            target = this.pointB;
        }
        Vector3 destination = target - my_pos;
        destination.z = 0;

        if (isArrived(my_pos, target))
        {
            time_to_wait -= Time.deltaTime;
            if (time_to_wait <= 0)
            {
                time_to_wait = start_time_to_wait;
                going_to_b *= -1;
            }
        }
        else
        {
            Transform transform = this.transform;
            Vector3 position = transform.position;
            position.x += speedx * going_to_b;
			position.y += speedy * going_to_b;
			transform.position=position;
        }
    }

    bool isArrived(Vector3 pos, Vector3 target)
    {
        pos.z = 0;
        target.z = 0;
        return Vector3.Distance(pos, target) < 0.02f;
    }
}
