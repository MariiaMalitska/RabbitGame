using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{

    public float speed = 1;
    Rigidbody2D myBody = null;
    bool isGrounded = false;
    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;
    Transform heroParent = null;
    bool isBig = false;
    public static HeroController lastRabbit = null;

    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();
        LevelController.current.setStartPosition(transform.position);
        this.heroParent = this.transform.parent;
    }

    void Awake()
    {
        lastRabbit=this;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Animator animator = GetComponent<Animator>();

        float value = Input.GetAxis("Horizontal");

        if (!animator.GetBool("die"))
        {
            // Run
            if (Mathf.Abs(value) > 0)
            {
                Vector2 vel = myBody.velocity;
                vel.x = value * speed;
                myBody.velocity = vel;
            }

            // Sprite flipping
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (value < 0)
            {
                sr.flipX = true;
            }
            else if (value > 0)
            {
                sr.flipX = false;
            }

            // Grounding
            Vector3 from = transform.position + Vector3.up * 0.3f;
            Vector3 to = transform.position + Vector3.down * 0.1f;
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
                SetNewParent(this.transform, this.heroParent);
            }
            Debug.DrawLine(from, to, Color.red);

            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                this.JumpActive = true;
            }
            if (this.JumpActive)
            {
                if (Input.GetButton("Jump"))
                {
                    this.JumpTime += Time.deltaTime;
                    if (this.JumpTime < this.MaxJumpTime)
                    {
                        Vector2 vel = myBody.velocity;
                        vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
                        myBody.velocity = vel;
                    }
                }
                else
                {
                    this.JumpActive = false;
                    this.JumpTime = 0;
                }
            }

            // Animtion part

            // Run
            if (Mathf.Abs(value) > 0)
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

    public void becomeBig()
    {
        if (!isBig)
        {
            Transform transform = this.transform;
            Vector3 scale = transform.localScale;
            scale.x *= 2;
            scale.y *= 2;
            this.transform.localScale = scale;
            isBig = true;
        }
    }

    public void becomeSmall()
    {
        if (isBig)
        {
            Transform transform = this.transform;
            Vector3 scale = transform.localScale;
            scale.x /= 2;
            scale.y /= 2;
            this.transform.localScale = scale;
        }
        else
        {
            LevelController.current.onRabbitDeath(this);
        }
    }
}
