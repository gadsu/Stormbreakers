using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandInterpreter))]

public class RandolfSpecials : MonoBehaviour {

    private Animator anim;
    private CharacterMovement cm;
    private CharacterState cs;
    private Rigidbody2D rb;
    private CommandInterpreter ci;

    private bool isDashing = false;
    private int dashFrames = 0;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        cm = GetComponent<CharacterMovement>();
        cs = GetComponent<CharacterState>();
        rb = GetComponent<Rigidbody2D>();
        ci = GetComponent<CommandInterpreter>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("214") && col.gameObject.layer != gameObject.layer)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void special214C()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("214"))
        {
            anim.Play("214");
            isDashing = true;
            dashFrames = Time.frameCount;
            if (cs.isOnLeft())
            {
                rb.velocity = new Vector2(11.0f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-11.0f, rb.velocity.y);
            }
            ci.isInterpretingInputs = false;
        }
    }

    public void special214B()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("214"))
        {
            anim.Play("214");
            isDashing = true;
            dashFrames = Time.frameCount;
            if (cs.isOnLeft())
            {
                rb.velocity = new Vector2(13.0f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-13.0f, rb.velocity.y);
            }
        }
    }

    public void special214A()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("214"))
        {
            anim.Play("214");
            isDashing = true;
            dashFrames = Time.frameCount;
            if (cs.isOnLeft())
            {
                rb.velocity = new Vector2(15.0f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-15.0f, rb.velocity.y);
            }
        }
    }

    public void special623A()
    {
        //anim.Play("623");
        isDashing = true;
        dashFrames = Time.frameCount;
        //if (cs.isOnLeft())
        {
            rb.velocity = new Vector2(rb.velocity.x / 2.0f, 15.0f);
        }
    }

    public void special623B()
    {
        //anim.Play("623");
        isDashing = true;
        dashFrames = Time.frameCount;
        //if (cs.isOnLeft())
        {
            rb.velocity = new Vector2(rb.velocity.x / 2.0f, 17.0f);
        }
    }

    public void special623C()
    {
        //anim.Play("623");
        isDashing = true;
        dashFrames = Time.frameCount;
        //if (cs.isOnLeft())
        {
            rb.velocity = new Vector2(rb.velocity.x / 2.0f, 19.0f);
        }
        /*else
        {
            rb.velocity = new Vector2(-15.0f, rb.velocity.y);
        }*/
    }
	
	// Update is called once per frame
	void Update () 
    {
        /*if (!anim.GetCurrentAnimatorStateInfo(0).IsName("214"))
        {
            cm.canMove = true;
        }*/
        if (isDashing && Time.frameCount > dashFrames + 1)
        {
            isDashing = false;
            ci.isInterpretingInputs = false;
        }
	}
}
