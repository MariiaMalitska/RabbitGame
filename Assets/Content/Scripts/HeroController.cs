using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

	public float speed=1;
	Rigidbody2D myBody = null;
	bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;

	void Start () {
		myBody=this.GetComponent<Rigidbody2D>();
		LevelController.current.setStartPosition (transform.position);
	}
	
	void Update () {
		
	}

	void FixedUpdate(){
		float value = Input.GetAxis("Horizontal");

		// Run
		if (Mathf.Abs (value)>0)
		{
			Vector2 vel=myBody.velocity;
			vel.x=value*speed;
			myBody.velocity=vel;
		}

		// Sprite flipping
		SpriteRenderer sr=GetComponent<SpriteRenderer>();
		if (value<0)
		{
			sr.flipX=true;
		}
		else if (value>0)
		{
			sr.flipX=false;
		}

		// Jump
		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer ("Ground");
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if(hit) 
		{
			isGrounded = true;
		} else 
		{
			isGrounded = false;
		}
		Debug.DrawLine (from, to, Color.red);

		if(Input.GetButtonDown("Jump") && isGrounded) 
		{
			this.JumpActive = true;
		}
		if(this.JumpActive) 
		{
			if(Input.GetButton("Jump")) 
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
		Animator animator = GetComponent<Animator> ();

		// Run
		if(Mathf.Abs(value) > 0) {
			animator.SetBool ("run", true);
		} else {
			animator.SetBool ("run", false);
		}

		// Jump
		if(this.isGrounded) 
		{
			animator.SetBool ("jump", false);
		}
		else 
		{
			animator.SetBool ("jump", true);
		}
	}
}
