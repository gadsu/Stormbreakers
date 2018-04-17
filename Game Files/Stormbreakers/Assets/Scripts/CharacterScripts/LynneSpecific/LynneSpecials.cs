using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandInterpreter))]
[RequireComponent(typeof(PlayerSettings))]

public class LynneSpecials : MonoBehaviour {


	private Animator anim;
	private CharacterMovement cm;
	private CharacterState cs;
    private CommandInterpreter ci;
    private Rigidbody2D rb;
    private PlayerSettings ps;

    private bool isDashing = false;
    private int dashFrames = 0;
    public bool isSliding = false;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
		cm = GetComponent<CharacterMovement>();
		cs = GetComponent<CharacterState>();
        rb = GetComponent<Rigidbody2D>();
        ci = GetComponent<CommandInterpreter>();
        ps = GetComponent<PlayerSettings>();
	}
        
	public void special214C()
	{
        Debug.Log("Lynne214C");
        isSliding = true;
        ci.isInterpretingInputs = false;
        anim.Play("slide");
        if (cs.isOnLeft())
        {
            rb.velocity = new Vector2(11.0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-11.0f, rb.velocity.y);
        }
	}

    public void special214B()
    {
        Debug.Log("Lynne214B");
        isSliding = true;
        ci.isInterpretingInputs = false;
        anim.Play("slide");
        if (cs.isOnLeft())
        {
            rb.velocity = new Vector2(11.0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-11.0f, rb.velocity.y);
        }
    }

    public void special214A()
    {
        Debug.Log("Lynne214A");
        isSliding = true;
        ci.isInterpretingInputs = false;
        anim.Play("slide");
        if (cs.isOnLeft())
        {
            rb.velocity = new Vector2(11.0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-11.0f, rb.velocity.y);
        }
    }

    public void specialSlideA()
    {
        Debug.Log("SlideA");
        cs.setZone('l');
        cs.setDamage(5);
        anim.Play("L214A");
    }

    public void specialSlideB()
    {
        Debug.Log("SlideB");
        cs.setZone('l');
        cs.setDamage(10);
        anim.Play("L214B");
    }

    public void specialSlideC()
    {
        Debug.Log("SlideC");
        cs.setZone('h');
        rb.velocity = new Vector2(rb.velocity.x, 7.0f);
        cs.setDamage(5);
        anim.Play("L214C");
    }

    public void special623C()
    {
        //anim.Play("623");
        Debug.Log("Lynne Special");
        isDashing = true;
        dashFrames = Time.frameCount;
        {
            rb.velocity = new Vector2(rb.velocity.x / 2.0f, 19.0f);
        }
    }

    public void special623B()
    {
        //anim.Play("623");
        Debug.Log("Lynne Special");
        isDashing = true;
        dashFrames = Time.frameCount;
        {
            rb.velocity = new Vector2(rb.velocity.x / 2.0f, 17.0f);
        }
    }

    public void special623A()
    {
        //anim.Play("623");
        Debug.Log("Lynne Special");
        isDashing = true;
        dashFrames = Time.frameCount;
        {
            rb.velocity = new Vector2(rb.velocity.x / 2.0f, 15.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (isDashing && Time.frameCount > dashFrames + 1)
        {
            isDashing = false;
            ci.isInterpretingInputs = false;
        }
        if (isSliding)
        {
            if (Input.GetButtonDown("P" + ps.getPlayerNumber() + "A"))
            {
                specialSlideA();
                isSliding = false;
            }
            else if (Input.GetButtonDown("P" + ps.getPlayerNumber() + "B"))
            {
                specialSlideB();
                isSliding = false;
            }
            else if (Input.GetButtonDown("P" + ps.getPlayerNumber() + "C"))
            {
                specialSlideC();
                isSliding = false;
            }
        }
	}
}
