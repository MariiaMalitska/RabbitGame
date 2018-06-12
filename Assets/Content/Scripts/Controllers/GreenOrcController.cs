using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenOrcController : MonoBehaviour
{

    public float speed = 1;
    Rigidbody2D body = null;
    bool isGrounded = false;
    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;
    Transform parent = null;
    Vector3 pointA;
    Vector3 pointB;
    public Vector3 MoveBy;
    public float start_time_to_wait = 1f;
    float time_to_wait = 1f;
    public enum Mode
    {
        GoToA,
        GoToB,
        Attack,
        Idle
    }
    Mode mode = Mode.GoToA;

    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
        LevelController.current.setStartPosition(transform.position);
        this.parent = this.transform.parent;
        this.pointA = this.transform.position;
        this.pointB = this.pointA + MoveBy;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Animator animator = GetComponent<Animator>();
        Vector3 my_pos = this.transform.position;
        Vector3 rabit_pos = HeroController.lastRabbit.transform.position;
        float value = this.getDirection();

        // Rabbit check

        if (rabit_pos.x > Mathf.Min(pointA.x, pointB.x) && rabit_pos.x < Mathf.Max(pointA.x, pointB.x))
        {
            mode = Mode.Attack;
        }


        // Move

        if (mode == Mode.GoToA)
        {
            if (isArrived(my_pos, pointA))
            {
                value = 0;
                time_to_wait -= Time.deltaTime;
                if (time_to_wait <= 0)
                {
                    time_to_wait = start_time_to_wait;
                    mode = Mode.GoToB;
                }
            }
        }
        else if (mode == Mode.GoToB)
        {
            if (isArrived(my_pos, pointB))
            {
                value = 0;
                time_to_wait -= Time.deltaTime;
                if (time_to_wait <= 0)
                {
                    time_to_wait = start_time_to_wait;
                    mode = Mode.GoToA;
                }
            }
        }
        else if (mode == Mode.Attack)
        {

        }

        BoxCollider2D main_col = GetComponent<BoxCollider2D>();
        CapsuleCollider2D head_col=GetComponent<CapsuleCollider2D>();
        BoxCollider2D rabbit_col = HeroController.lastRabbit.GetComponent<BoxCollider2D>();

        if (main_col.IsTouching(rabbit_col)) 
        {
                value = 0;
                animator.SetTrigger("attack");
        }

        Transform transform = this.transform;
        my_pos.x += speed * value;
        transform.position = my_pos;


        // Sprite flipping
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (value > 0)
        {
            sr.flipX = true;
        }
        else if (value < 0)
        {
            sr.flipX = false;
        }

        // Grounding
        Vector3 from = my_pos + Vector3.up * 0.3f;
        Vector3 to = my_pos + Vector3.down * 0.1f;
        int layer_id = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
        if (hit)
        {
            isGrounded = true;
            if (hit.transform != null && hit.transform.GetComponent<MovingPlatform>() != null)
            {
                SetNewParent(this.transform, hit.transform);
            }
        }
        else
        {
            isGrounded = false;
            SetNewParent(this.transform, this.parent);
        }

        // Animtion part

        // Walk
        if (Mathf.Abs(value) > 0 && mode != Mode.Attack)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        if (mode == Mode.Attack)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        // Jump
        if (this.isGrounded)
        {
            animator.SetBool("jump", false);
        }
        else
        {
            animator.SetBool("jump", true);
        }

    }

    static void SetNewParent(Transform obj, Transform new_parent)
    {
        if (obj.transform.parent != new_parent)
        {
            Vector3 pos = obj.transform.position;
            obj.transform.parent = new_parent;
            obj.transform.position = pos;
        }
    }

    float getDirection()
    {
        Vector3 my_pos = this.transform.position;
        Vector3 rabit_pos = HeroController.lastRabbit.transform.position;

        if (mode == Mode.GoToA)
        {
            if (my_pos.x < pointA.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        if (mode == Mode.GoToB)
        {
            if (my_pos.x < pointB.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        if (mode == Mode.Idle)
        {
            return 0;
        }
        if (mode == Mode.Attack)
        {
            if (my_pos.x < rabit_pos.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        else return 0;

    }

    bool isArrived(Vector3 pos, Vector3 target)
    {
        pos.z = 0;
        target.z = 0;
        return Vector3.Distance(pos, target) < 0.02f;
    }

    IEnumerator die(float sec)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("die", true);
        // Debug.LogWarning("Starting");
        yield return new WaitForSeconds(sec);
        // Debug.LogWarning("End");
        animator.SetBool("die", false);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GetComponent<Collider2D>().GetType() == typeof(CapsuleCollider2D))
        {
            Debug.Log("die");
            StartCoroutine(die(0.7f));
        }
        if (GetComponent<Collider2D>().GetType() == typeof(BoxCollider2D))
        {
            HeroController rabbit = collider.GetComponent<HeroController>();
            if (rabbit != null)
                LevelController.current.onRabbitDeath(rabbit);
            //mode=Mode.GoToB;
        }
        
    }

}
