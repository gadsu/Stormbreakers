/*
 * Script to process movement commands it receives from the CommandInterpreter.
 * PHYSICS ARE CHANGED HERE!! If something's flying all over the place or moving
 * faster than it should, this is a good place to check.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandInterpreter))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(PlayerSettings))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputGatherer))]
[RequireComponent(typeof(CharacterAttacks))]

public class CharacterMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private CommandInterpreter ci;
    private CharacterState cs;
    private PlayerSettings ps;
    private Animator anim;
    private InputGatherer ig;
    private CharacterAttacks ca;

    private AudioClip jumpSound;
    private AudioClip dashSound;
    private AudioClip runningSound;

    public bool canMove;
    private bool tempCanMove;
    private bool tempCanMoveHasBeenSet;

    public bool isHit;

    private int airAction; //Remaining air actions
    public char dashDirection;
    public bool isInAir;
    private Vector2 hitStunVector;
    public int hitStunDuration;

    private bool isSlowed;
    private float slowDuration;

    private const int airDashIgnoreGravityFrames = 20; //How long air dashes last
    private int airDashIgnoreGravityCount; 

    private const int gravityScale = 1;

    private int verticalJumpVelocity = 17;  //How high you can jump
    private int horizontalJumpVelocity = 8; //How far you can jump
    private int walkSpeed = 5;
    private int dashSpeed = 10;
    //private int backdashLength = 15;

    private int initialLayer;
    private const int airCollisionFixLayer = 16;
    private const int airCollisionDownardFixLayer = 17;

    public int comboHits;
    private GameObject comboCounter;
    private bool comboVisible;

	// Use this for initialization
	void Start () 
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (GetComponent<CommandInterpreter>())
        {
            ci = GetComponent<CommandInterpreter>();
        }
        if (GetComponent<CharacterState>())
        {
            cs = GetComponent<CharacterState>();
        }
        if (GetComponent<PlayerSettings>())
        {
            ps = GetComponent<PlayerSettings>();
        }
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        if (GetComponent<InputGatherer>())
        {
            ig = GetComponent<InputGatherer>();
        }
        if (GetComponent<CharacterAttacks>())
        {
            ca = GetComponent<CharacterAttacks>();
        }

        if (gameObject.name.Contains("1"))
        {
            comboCounter = GameObject.Find("P2ComboCounter");
        }
        else
        {
            comboCounter = GameObject.Find("P1ComboCounter");
        }

        hitStunVector = new Vector2();
        hitStunDuration = 0;
        airAction = 1;
        isInAir = true;
        airDashIgnoreGravityCount = 0;
        dashDirection = '5';
        canMove = true;
        isHit = false;
        isSlowed = false;

        slowDuration = 60.0f;

        tempCanMove = true;
        tempCanMoveHasBeenSet = false;

        comboHits = 0;

        initialLayer = gameObject.layer;

        jumpSound = Resources.Load("Sounds/Jump") as AudioClip;
        dashSound = Resources.Load("Sounds/Dash") as AudioClip;
        runningSound = Resources.Load("Sounds/Running") as AudioClip;
	}

    void OnCollisionEnter2D(Collision2D col) //Checks when you hit the ground
    {
        if (col.gameObject.name == "Floor")
        {
            isInAir = false;
            if (!cs.isKnockedDown && !anim.GetCurrentAnimatorStateInfo(0).IsName("death") && !anim.GetCurrentAnimatorStateInfo(0).IsName("deathIdle"))
            {
                anim.Play("idle");
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == "Floor")
        {
            isInAir = true;
        }
    }

    public void setCanMove(bool cm)
    {
        canMove = cm;
    }

    public bool getPlayerCanMove()
    {
        return canMove;
    }
	
    public void walk(char direction)
    {
        
        if (canWalk() && canMove && !ci.crouching)
        {
            
            if (direction == '4')
            {
                rb.velocity = new Vector2(-1 * walkSpeed, rb.velocity.y);
                if (isIdle())
                {
                    anim.Play("walk");
                }
            }
            else if (direction == '6')
            {
                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
                if (isIdle())
                {
                    anim.Play("walk");
                }
            }
            else
            {
                if (direction == '5')
                {
                    dashDirection = '5';
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
                    {
                        anim.Play("idle");
                    }

                }
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    public bool canWalk()
    {
        return (!isinAir() && dashDirection == '5' && !ci.hasUpInput());/*(Input.GetKey(KeyCode.UpArrow) ||
                                                        Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") > 0 || 
                                                        Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") > 0) && rb.velocity.y == 0);*/
    }

    private bool isIdle()
    {
        return(anim.GetCurrentAnimatorStateInfo(0).IsName("idle"));
    }

    public char getDashDirection()
    {
        return dashDirection;
    }

    public void dash(char direction)
    {
        if (canMove)
        {
            if (isinAir())
            {
                if (airAction > 0)
                {
                    gameObject.layer = initialLayer;
                    if (direction == '4')
                    {
                        if (!cs.isOnLeft())
                        {
                            anim.Play("airDash");
                        }
                        rb.velocity = new Vector2(-1 * dashSpeed, 0);
                        airDashIgnoreGravityCount = airDashIgnoreGravityFrames;
                        airAction--;
                    }
                    else if (direction == '6')
                    {
                        if (cs.isOnLeft())
                        {
                            anim.Play("airDash");
                        }
                        rb.velocity = new Vector2(dashSpeed, 0);
                        airDashIgnoreGravityCount = airDashIgnoreGravityFrames;
                        airAction--;
                    }
                }
            }
            else if(canWalk())
            {
                if (direction == '4')
                {
                    if (cs.isOnLeft())
                    {
                        anim.Play("backDash");
                        rb.velocity = new Vector2(-1 * (dashSpeed), 0);
                        dashDirection = '5';
                    }
                    else
                    {
                        rb.velocity = new Vector2(-1 * dashSpeed, 0);
                        dashDirection = '4';
                    }
                }
                else if (direction == '6')
                {
                    if (cs.isOnLeft())
                    {
                        rb.velocity = new Vector2(dashSpeed, 0);
                        dashDirection = '6';
                    }
                    else
                    {
                        anim.Play("backDash");
                        rb.velocity = new Vector2(dashSpeed, 0);
                        dashDirection = '5';
                    }
                }

                AudioSource.PlayClipAtPoint(dashSound, GameObject.Find("Camera").transform.position);
            }
        }

    }

    public bool isinAir()
    {
        return isInAir;
    }

    public void applyPushback(Vector2 amount, bool onLeft)
    {

        isHit = true;

        if (onLeft)
        {
            hitStunVector = new Vector2(-1.0f * amount.x, amount.y);
        }
        else
        {
            hitStunVector = amount;
        }

        canMove = false;
    }

    public void applyPushback(Vector2 amount, int duration, bool onLeft)
    {
        isHit = true;
        if (hitStunDuration < (Time.frameCount + duration))
        {

            hitStunDuration = Time.frameCount + duration;
        }

        if (onLeft)
        {
            hitStunVector = new Vector2(-1.0f * amount.x, amount.y);
        }
        else
        {
            hitStunVector = amount;
        }

        canMove = false;
    }

    public void jump()
    {
        if (canMove)
        {
            if (isinAir())
            {
                if (airAction > 0)
                {
                    gameObject.layer = airCollisionFixLayer;
                    anim.Play("dJump");
                    AudioSource.PlayClipAtPoint(jumpSound, GameObject.Find("Camera").transform.position);

                    if(Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < 0)
                    {
                        airAction--;
                        rb.velocity = new Vector2(-1 * horizontalJumpVelocity, verticalJumpVelocity);
                    }
                    else if(Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > 0)
                    {
                        airAction--;
                        rb.velocity = new Vector2(horizontalJumpVelocity, verticalJumpVelocity);
                    }
                    else
                    {
                        airAction--;
                        rb.velocity = new Vector2(0, verticalJumpVelocity);
                    }
                }
            }
            else //if you haven't already jumped)
            {
                gameObject.layer = airCollisionFixLayer;
                anim.Play("jump");
                AudioSource.PlayClipAtPoint(jumpSound, GameObject.Find("Camera").transform.position);

                if (dashDirection == '5')
                {
                    if (Input.GetKey(KeyCode.LeftArrow)  || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < 0)
                    {
                        /*if (ci.getElementChoice() == 't')
                        {
                            airAction = 2;
                        }
                        else*/
                        {
                            airAction = 1;
                        }
                        rb.velocity = new Vector2(-1 * horizontalJumpVelocity, verticalJumpVelocity);
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > 0)
                    {
                        /*if (ci.getElementChoice() == 't')
                        {
                            airAction = 2;
                        }
                        else*/
                        {
                            airAction = 1;
                        }
                        rb.velocity = new Vector2(horizontalJumpVelocity, verticalJumpVelocity);
                    }
                    else
                    {
                        /*if (ci.getElementChoice() == 't')
                        {
                            airAction = 2;
                        }
                        else*/
                        {
                            airAction = 1;
                        }
                        rb.velocity = new Vector2(0, verticalJumpVelocity);
                    }
                }
                else
                {
                    /*if (ci.getElementChoice() == 't')
                        {
                            airAction = 2;
                        }
                        else*/
                    {
                        airAction = 1;
                    }
                    rb.velocity = new Vector2(rb.velocity.x, verticalJumpVelocity);
                }
            }
        }
    }

    public void slowDown()
    {
        walkSpeed = 2;
        dashSpeed = 5;
        horizontalJumpVelocity = 4;

        slowDuration = Time.frameCount + 120;
        isSlowed = true;
    }

    void speedUp()
    {
        walkSpeed = 5;
        dashSpeed = 10;
        horizontalJumpVelocity = 8;
        isSlowed = false;
    }

    void FixedUpdate()
    {

        if (canMove)
        {
            if (airDashIgnoreGravityCount > 0 && rb.gravityScale > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                rb.gravityScale = 0;
            }
            else if (airDashIgnoreGravityCount <= 0)
            {
                if (rb.gravityScale != gravityScale)
                {
                    rb.gravityScale = gravityScale;
                }

            }

            if (airDashIgnoreGravityCount > 0)
            {
                airDashIgnoreGravityCount--;
            }
        }

        if (isSlowed && Time.frameCount > slowDuration)
        {
            speedUp();
        }

        //The entire block from here downwards used to be in Update() instead of FixedUpdate(). If stuff starts to break, that might not be a bad place to start checking.

        {
                

            if (isPlayingImmobileAnimation())
            {
                if (!tempCanMoveHasBeenSet)
                {
                    tempCanMove = canMove;
                    tempCanMoveHasBeenSet = true;
                    canMove = false;
                }
            }
            else if(tempCanMoveHasBeenSet && hitStunDuration < Time.frameCount)
            {
                canMove = tempCanMove;
                tempCanMoveHasBeenSet = false;
            }

            if(canMove)
            {
                if (dashDirection != '5' && !isinAir())
                {
                    if (dashDirection == '4' && (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < 0))
                    {
                        rb.velocity = new Vector2(-1 * dashSpeed, rb.velocity.y);
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
                        {
                            anim.Play("dash");
                            AudioSource.PlayClipAtPoint(runningSound, GameObject.Find("Camera").transform.position);
                        }
                    }
                    else if (dashDirection == '6' && (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > 0))
                    { 
                        rb.velocity = new Vector2(dashSpeed, rb.velocity.y);
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
                        {
                            anim.Play("dash");
                            AudioSource.PlayClipAtPoint(runningSound, GameObject.Find("Camera").transform.position);
                        }
                    }
                    else
                    {
                        dashDirection = '5';
                    }

                }
                if (!isinAir())
                {
                    airDashIgnoreGravityCount = 0;
                }
            }
            else
            {
                if (isHit) /*(rb.velocity != Vector2.zero && !(isHit || anim.GetCurrentAnimatorStateInfo(0).IsName("hit")))*/ 
                {
                    rb.velocity = hitStunVector;
                    isHit = false;
                }
                else
                {
                    if (Time.frameCount > hitStunDuration  && (!isPlayingImmobileAnimation() || anim.GetCurrentAnimatorStateInfo(0).IsName("gettingUp")))
                    {
                        canMove = true;
                        if (comboHits > 0)
                        {
                            comboHits = 0;
                        }
                       
                        //tempCanMoveHasBeenSet = false;
                    }
                }
            }
            if (gameObject.name.Contains("1"))
            {
            }
        }

    }

    //The most beautiful code I've ever written.
    //It's so ugly... ;_;
    public bool isPlayingImmobileAnimation()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit0"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit4"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchHit0"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchHit1"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchHit2"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchHit3"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchHit4"))
        {
            return true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hitThrow"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("block"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchBlock"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("airBlock"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("236"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("214"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5A"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5B"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5C"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2A"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2B"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2C"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("knockedDown"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("knockdownDelay"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("gettingUp"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("backDash"))
        {
            return true;
        }

        else if (ci.charging) //|| anim.GetCurrentAnimatorStateInfo(0).IsName("charge"))
        {
            return true;
        }
      //  else if (anim.GetCurrentAnimatorStateInfo(0).IsName("stand"))
     //   {
     //       return true;
    //    }
       // else if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouch"))
      //  {
       //     return true;
      //  }
        /*else if (anim.GetCurrentAnimatorStateInfo(0).IsName("jA"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("jB"))
        {
            return true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("jC"))
        {
            return true;
        }*/

        return false;
    }

	// Update is called once per frame
	void Update () 
    {
        if (gameObject.name.Contains("L"))
        {
        }
            
        if (gameObject.layer != initialLayer && !isinAir() && !ci.isJumping())
        {
            gameObject.layer = initialLayer;
        }
        else if (gameObject.layer != initialLayer && rb.velocity.y < 0)
        {
            gameObject.layer = airCollisionDownardFixLayer;
        }

        if (comboHits > 1 && !comboVisible)
        {
            comboCounter.GetComponent<Animator>().Play("ComboAppear");
            comboCounter.GetComponent<Text>().text = comboHits.ToString();
            comboVisible = true;
        }
        else if (comboHits < 2 && comboVisible)
        {
            comboCounter.GetComponent<Animator>().Play("ComboDisappear");
            comboVisible = false;
        }
        else if(comboHits > 1)
        {
            comboCounter.GetComponent<Text>().text = comboHits.ToString();
        }

       
	}
}
