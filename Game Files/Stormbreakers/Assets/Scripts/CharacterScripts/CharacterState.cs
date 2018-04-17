/*
 * Script to maintain a character's state.
 * This should cover all the following states:
 *      Standing
 *      Crouching
 *      Counterhit
 *      Blocking
 *          (Mid/Low)
 *      Throw Invulnerability
 * Throw invuln is the only one that needs to be handled here, since
 * most other invuln states will be handled by changing hurtboxes, but
 * throw invuln is a special type of hurtbox modifier. Movement and all
 * that stuff is covered in the CharacterMovement script.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputGatherer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandInterpreter))]

public class CharacterState : MonoBehaviour {
    
    private bool facingLeft;
    private Transform enemyTransform; //location of other player
    private int health = 100;
    public int special = 0;
    private float hitStopEndTime;
    public int specialLevel = 0;
    private Image specialFill;

    private float dmg = 0.0f;
    private char zone = ' ';
    private char attribute = ' '; //k for knockdown. Not sure what else could go here.
    private int level = 0;
    private const float damageScalingValue = 0.8f; //Higher number means less scaling. Lower number means less damaage.

    private InputGatherer ig;
    private Animator an;
    private CharacterMovement cm;
    private Rigidbody2D rb;
    private CommandInterpreter ci;

    private string hitAnim;
    private string blockAnim;
    private string airBlockAnim;
    private string crouchAnim;
    private string standAnim;
    private string knockdownAnim;
    private string crouchBlockAnim;
    private string crouchHitAnim;

    public Motion standingIdle;
    public Motion crouchingIdle;

    public Slider healthBar;
    public Slider specialBar;
    public char blockZone;
    private AudioClip blockedhitSound;
    private AudioClip lighthitSound;
    private AudioClip heavyhitSound;
    private AudioClip fireballSound;

    public GameObject hitSparkPrefab;
    private GameObject hitSpark;

    public bool isStanding;
	public bool isInvuln;
	public bool hasArmor;
    public bool isKnockedDown;
    public bool isTechingThrow;

	// Use this for initialization
	void Start () {
        //See which side the player starts on for purpose of flipping sprites
        if (gameObject.name.Contains("player1"))
        {
            healthBar = GameObject.Find("Player1Slider").GetComponent<Slider>();
            specialBar = GameObject.Find("Player1Special").GetComponent<Slider>();
            specialFill = GameObject.Find("P1Fill").GetComponent<Image>();
            facingLeft = false;
            if (GameObject.Find("player2R"))
            {
                enemyTransform = GameObject.Find("player2R").transform;
            }
            else if (GameObject.Find("player2L"))
            {
                enemyTransform = GameObject.Find("player2L").transform;
            }
        }
        else if (gameObject.name.Contains("player2"))
        {
            healthBar = GameObject.Find("Player2Slider").GetComponent<Slider>();
            specialBar = GameObject.Find("Player2Special").GetComponent<Slider>();
            specialFill = GameObject.Find("P2Fill").GetComponent<Image>();
            facingLeft = true;
            if (GameObject.Find("player1R"))
            {
                enemyTransform = GameObject.Find("player1R").transform;
            }
            else if (GameObject.Find("player1L"))
            {
                enemyTransform = GameObject.Find("player1L").transform;
            }
        }

        an = GetComponent<Animator>();
        cm = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody2D>();
        ci = GetComponent<CommandInterpreter>();

        hitAnim = "hit";
        blockAnim = "block";
        airBlockAnim = "airBlock";
        crouchAnim = "crouch";
        standAnim = "stand";
        //knockdownAnim = "kd";
        crouchBlockAnim = "crouchBlock";
        crouchHitAnim = "crouchHit";

        blockedhitSound = Resources.Load("Sounds/Blocked_Hit") as AudioClip;;
        lighthitSound = Resources.Load("Sounds/Light_Hit") as AudioClip;;
        heavyhitSound = Resources.Load("Sounds/Heavy_Hit") as AudioClip;;
        fireballSound = Resources.Load("Sounds/Fireball") as AudioClip;;

        //Flip if you're on the left
        if (facingLeft && gameObject.GetComponent<SpriteRenderer>())
        {
            Vector3 flip = gameObject.transform.localScale;
            flip.x = flip.x * -1;
            gameObject.transform.localScale = flip;
        }

        if (gameObject.GetComponent<InputGatherer>())
        {
            ig = GetComponent<InputGatherer>();
        }

        blockZone = ' ';

        isStanding = true;
		isInvuln = false;
        isTechingThrow = false;

        hitSparkPrefab = Resources.Load("MiscPrefabs/Hitspark") as GameObject;
	}


    //When two boxcolliders....collide.  Used for fireball and basic damage.
    void OnTriggerEnter2D(Collider2D col)
    {
       

        if (isInvuln)
        {
            Debug.Log("IsInvuln!");
        }

		else if (col.gameObject.tag == "Projectile" && col.GetComponent<ProjectileScript>() && col.gameObject.layer != gameObject.layer)
        { 
			if(blockZone == ' ')
            {
                dmg = col.gameObject.GetComponent<ProjectileScript>().getDamage() / (cm.comboHits + 1);
                //healthBar.value -= dmg;
                useSpecial(Mathf.RoundToInt(dmg / 2));
				if (hasArmor)
				{
					hasArmor = false;
				}
				else
				{
                    if (ci.isCrouching())
                    {
                        an.Play(crouchHitAnim + "2");
                    }
                    else
                    {
                        an.Play(hitAnim + "2");
                    }
                	cm.applyPushback(col.GetComponent<ProjectileScript>().getPushback(), col.GetComponent<ProjectileScript>().getHitstun(), isOnLeft()); 
                    cm.comboHits++;
                }

                applyHitStop(0.15f);
                takeDamage(Mathf.RoundToInt(dmg));
            }
            else
            {
                dmg = col.gameObject.GetComponent<ProjectileScript>().getChipDamage() / (cm.comboHits + 1);
                //healthBar.value -= dmg;
                useSpecial(Mathf.RoundToInt(dmg / 2));

				/*if (hasArmor)
				{
					hasArmor = false;
				}*/
				//else
                if (!cm.isinAir())
                {
                    if (blockZone == 'l')
                    {
                        an.Play(crouchBlockAnim);
                    }
                    else
                    {
                        an.Play(blockAnim);
                    }

                    cm.applyPushback(new Vector2(2.0f, rb.velocity.y), isOnLeft()); 
                }
                else
                {
                    an.Play(airBlockAnim);
                    cm.applyPushback(new Vector2(2.0f, rb.velocity.y), isOnLeft()); 
                }

                //col.gameObject.GetComponentInParent<CharacterState>().useSpecial(dmg);

                takeDamage(Mathf.RoundToInt(dmg));
                applyHitStop(0.05f);
            }
        }

        else if (col.gameObject.tag == "Hitbox" && ((gameObject.name.Contains("player2") && col.gameObject.layer != 11) || (gameObject.name.Contains("player1") && col.gameObject.layer != 10)))
        {
            if (an.GetCurrentAnimatorStateInfo(0).IsName("hitThrow"))
            {
                cm.applyPushback(new Vector2(6.0f, 0), isOnLeft());
            }
            else if (col.GetComponentInParent<Rigidbody2D>() && col.GetComponentInParent<CharacterMovement>())
            {
                col.GetComponentInParent<CharacterMovement>().applyPushback(new Vector2(3.0f, col.GetComponentInParent<Rigidbody2D>().velocity.y), !isOnLeft());
            }

            if (cm.isinAir() && (blockZone == 'h' || blockZone == 'l'))
            {
                AudioSource.PlayClipAtPoint(blockedhitSound, GameObject.Find("Camera").transform.position);
                an.Play(airBlockAnim);
                useSpecial(Mathf.RoundToInt(col.gameObject.GetComponentInParent<CharacterState>().dmg) / 3);
                col.gameObject.GetComponentInParent<CharacterAttacks>().hitConnect();
                cm.applyPushback(new Vector2(2.0f, rb.velocity.y), isOnLeft());
                applyHitStop(0.1f);
            }
            else if (col.gameObject.GetComponentInParent<CharacterState>().zone == blockZone || ((blockZone == 'h' || blockZone == 'l') && col.gameObject.GetComponentInParent<CharacterState>().zone == 'm'))
            {
                AudioSource.PlayClipAtPoint(blockedhitSound, GameObject.Find("Camera").transform.position);

                if (blockZone == 'l')
                {
                    an.Play(crouchBlockAnim);
                }
                else
                {
                    an.Play(blockAnim);
                }

                useSpecial(Mathf.RoundToInt(col.gameObject.GetComponentInParent<CharacterState>().dmg) / 3);
                col.gameObject.GetComponentInParent<CharacterAttacks>().hitConnect();
                cm.applyPushback(new Vector2(2.0f, rb.velocity.y), isOnLeft());
                applyHitStop(0.1f);
            }
            else
            {
                
                hitSpark = Instantiate(hitSparkPrefab, transform.position, transform.rotation);

                if (Mathf.Pow(damageScalingValue, (float)cm.comboHits) > 0)
                {
                    dmg = col.gameObject.GetComponentInParent<CharacterState>().dmg * Mathf.Pow(damageScalingValue, (float)cm.comboHits);
                }
                else
                {
                    dmg = col.gameObject.GetComponentInParent<CharacterState>().dmg;
                }

                //dmg = col.gameObject.GetComponentInParent<CharacterState>().dmg / (cm.comboHits + 1);
                col.gameObject.GetComponentInParent<CharacterAttacks>().hitConnect();
                //healthBar.value -= dmg;
                useSpecial(Mathf.RoundToInt(dmg / 2));


                col.gameObject.GetComponentInParent<CharacterState>().useSpecial(Mathf.RoundToInt(dmg));
                AudioSource.PlayClipAtPoint(lighthitSound, GameObject.Find("Camera").transform.position);


				if (hasArmor)
				{
					hasArmor = false;
				}
                else if (col.gameObject.GetComponentInParent<CharacterState>().attribute == 'k')
                {
                    Debug.Log("KD!");
                    an.Play("knockedDown");
                    knockDown();
                    cm.comboHits++;
                }
				else
				{
                    if (ci.isCrouching())
                    {
                        an.Play(crouchHitAnim + col.gameObject.transform.parent.GetComponent<CharacterState>().getLevel().ToString());
                    }
                    else
                    {
                        an.Play(hitAnim + col.gameObject.transform.parent.GetComponent<CharacterState>().getLevel().ToString());
                    }
					cm.applyPushback(new Vector2(4.0f, rb.velocity.y), isOnLeft()); 
                    cm.comboHits++;
				}

                takeDamage(Mathf.RoundToInt(dmg));
                applyHitStop(0.2f);
            }

            //applyHitStop(0.2f);

  /*          if (col.gameObject.transform.parent.name.Contains("R"))
            {
                //col.gameObject.GetComponentInParent<RandolfHitboxEnabler>().stuff
            }
            else if (col.gameObject.transform.parent.name.Contains("L"))
            {
                col.gameObject.GetComponentInParent<LynneHitboxEnabler>().disableAllHitboxes();
            }

            if (gameObject.name.Contains("R"))
            {
            }*/

		}
		else if (col.gameObject.tag == "ThrowHitbox" && col.gameObject.GetComponentInParent<CharacterState>().zone == 't' &&
            !gameObject.GetComponent<CharacterMovement>().isinAir())
		{
            if (isTechingThrow)
            {
                Debug.Log("Throw Tech!");
            }
            else
            {
                col.gameObject.GetComponentInParent<CharacterAttacks>().nThrowHit();
                dmg = col.gameObject.GetComponentInParent<CharacterState>().dmg / (cm.comboHits + 1);
                takeDamage(Mathf.RoundToInt(dmg));

                if (isOnLeft())
                {
                    transform.position = new Vector3(enemyTransform.position.x - 1.2f, transform.position.y, enemyTransform.position.z);
                }
                else
                {
                    transform.position = new Vector3(enemyTransform.position.x + 1.2f, transform.position.y, enemyTransform.position.z);
                }

                Debug.Log("HitThrowPlay!");
                an.Play("hitThrow");
                //healthBar.value -= dmg;
            }
            //an.Play("KD");
            //changeInvuln(1);
		}
    }

    public void knockDown()
    {
        Debug.Log("knockDown Function");
        ci.isInterpretingInputs = false;
        cm.canMove = false;
        isKnockedDown = true;
       // cm.hitStunDuration = int.MaxValue; //Set this properly as an animation event on the getUp animation.
    }

    public void changeTechState(int i)
    {
        if (i == 0)
        {
            isTechingThrow = false;
        }
        else if (i > 0)
        {
            isTechingThrow = true;
        }
    }

    public void getUp()
    {
        ci.clearInputQueue();
        ci.isInterpretingInputs = true;
        cm.canMove = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isKnockedDown = false;
    }
        

    //Used for the pushboxes when you land on someone's head
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Pushbox" && gameObject.tag != "Hitbox" && (col.gameObject.GetComponentInParent<CharacterMovement>().gameObject.layer != gameObject.layer))/*((gameObject.name.Contains("player2") && col.gameObject.layer != 11) || (gameObject.name.Contains("player1") && col.gameObject.layer != 10)))*/
        {
            Debug.Log(col.gameObject.name + ", " + gameObject.name);
            //if (gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                if (gameObject.transform.position.x < enemyTransform.position.x)
                {
                    //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * 3, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    //col.gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(7, gameObject.GetComponent<Rigidbody2D>().velocity.y);

                    enemyTransform.position = new Vector2(enemyTransform.position.x + 0.1f, enemyTransform.position.y);
                    transform.position = new Vector2(transform.position.x - 0.1f, transform.position.y);
                }
                else
                {
                    //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(3, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    //col.gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * 7, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    enemyTransform.position = new Vector2(enemyTransform.position.x - 0.1f, enemyTransform.position.y);
                    transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
                }
            }
        }
    }

    public void setDamage(int damage)
    {
        dmg = damage;
    }

    public void setZone(char z)
    {
        zone = z;
    }

    public void setAttribute(char a)
    {
        attribute = a;
    }

    public void setLevel(int l)
    {
        level = l;
    }

    public int getLevel()
    {
        return level;
    }

    public void takeDamage(int damage)
    {
        if (damage > 1 || damage < 0)
        {
            health -= damage;
            healthBar.value -= damage;
        }
        else
        {
            health -= 1;
            healthBar.value -= 1;
        }
        if (health <= 0)
        {
            gameObject.GetComponent<Animator>().Play("death");
            if (gameObject.name.Contains("player1"))
            {
                endGame(1);
            }
            else if (gameObject.name.Contains("player2"))
            {
                endGame(2);
            }
            Debug.Log(gameObject.name + " died.");
        }
    }

	public void giveArmor()
	{
		hasArmor = true;
	}

	public void changeInvuln(int val)
	{
		if (val == 1) 
		{
			isInvuln = true;
		} 
		else 
		{
			isInvuln = false;
		}
	}

    public void fillHealth()
    {
        health = 100;
        healthBar.value = 100;
        special = 100;
        specialBar.value = 100;
    }

    public void useSpecial(int val)
    {
        if (special + val <= 0)
        {
            if (specialLevel > 0)
            {
                specialLevel--;
                special = (special + val) + 100;
                specialBar.value = (special + val) + 100;
            }
            else if (special + val == 0)
            {
                special = 0;
                specialBar.value = 0;
            }
            else
            {
                Debug.Log("Crying in useSpecial");
            }
        }
        else if ((special + val) < 100)
        {
            special += val;
            specialBar.value = special;
        }
        else if (specialLevel == 2)
        {
            special = 100;
            specialBar.value = 100;
        }
        else
        {
            specialLevel++;
            special = (special + val) - 100;
            specialBar.value = (special + val) - 100;
        }

        if (specialLevel == 0)
        {
            specialFill.color = Color.green; //new Color(47, 255, 160, 255);
        }
        else if (specialLevel == 1)
        {
            specialFill.color = Color.yellow;  //new Color(212, 192, 32, 255);
        }
        else if (specialLevel == 2)
        {
            specialFill.color = Color.red;  //new Color(255, 91, 47, 255);
        }
    }

    //Helper function checking which x is larger
    public bool isOnLeft()
    {
        if (enemyTransform && gameObject.transform)
        {
            if (gameObject.transform.position.x > enemyTransform.position.x)
            {
                return false;
            }

            return true;
        }
        return false;
    }

    public void crouch()
    {
        an.Play(crouchAnim);
        //GetComponent<SpriteRenderer>().sprite = crouchingSprite;
        isStanding = false;
    }

    public void stand()
    {
        an.Play(standAnim);
        //GetComponent<SpriteRenderer>().sprite = standingSprite;
        isStanding = true;
    }

    private void applyHitStop(float duration)
    {
        /*Time.timeScale = 0;
        Time.fixedDeltaTime = 0;*/

        hitStopEndTime = Time.realtimeSinceStartup + duration;


    }

    private void endHitStop()
    {
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1;
    }

    //More beautiful code ^_^ We are fucking talented
    public bool inBlockableState()
    {
        if (an.GetCurrentAnimatorStateInfo(0).IsName("block")) 
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("airBlock"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("crouch"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("crouchIdle"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("stand"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("dJump"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("airIdle"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            return true;
        }


        return false;
    }

    public void endGame(int player)
    {
		if (SceneManager.GetActiveScene ().name == "TrainingMode") 
		{
			if (player == 1) 
			{
				GameObject.Find ("P2Text").GetComponent<Text> ().text = "DEAD";

			}
			else if(player == 2)
			{
				GameObject.Find ("P1Text").GetComponent<Text> ().text = "DEAD";
			}
		} 
		else 
		{
            if (player == 1) 
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 2 Wins!";

            }
            else if(player == 2)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 1 Wins!";
            }
                
            GameObject.Find("GameOverAnimationText").GetComponent<Animator>().Play("FadeInText");
	    }
	}

	
	// Update is called once per frame
	void Update () 
    {
        /*if (Time.realtimeSinceStartup > hitStopEndTime && !GameObject.Find("Timer").GetComponent<PauseMenu>().paused)
        {
            endHitStop();
        }*/

        //If they're facing the wrong way and on the ground, flip!
        if (!cm.isPlayingImmobileAnimation())
        {
            if (isOnLeft() && gameObject.GetComponent<CharacterMovement>() && !GetComponent<CharacterMovement>().isinAir() && facingLeft)
            {
                /*if (gameObject.name == "player1R")
                {
                    Debug.Log("Flip1!" + Time.frameCount.ToString() + " " + facingLeft);
                }*/

                if (GetComponent<CharacterMovement>().getDashDirection() == '5' && gameObject.transform.localScale.x < 0)
                {
                    facingLeft = false; // This just got moved in here to fix a bug. Take a look at it if things with side detection and sprite flipping start to break.
                    Vector3 flip = gameObject.transform.localScale;
                    flip.x = flip.x * -1.0f;
                    gameObject.transform.localScale = flip;
                }
            }
            else if(!isOnLeft() && gameObject.GetComponent<CharacterMovement>() && !GetComponent<CharacterMovement>().isinAir() && !facingLeft)
            {
                /*if (gameObject.name == "player1R")
                {
                    Debug.Log("Flip2! " + Time.frameCount.ToString() + " " + facingLeft);
                }*/


                if (GetComponent<CharacterMovement>().getDashDirection() == '5' && gameObject.transform.localScale.x > 0)
                {
                    facingLeft = true; // This just got moved in here to fix a bug. Take a look at it if things with side detection and sprite flipping start to break.
                    Vector3 flip = gameObject.transform.localScale;
                    flip.x = flip.x * -1;
                    gameObject.transform.localScale = flip;
                }
            }
        }

        if (isKnockedDown && !cm.isinAir())
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
            

        //If they are holding a button, pick which zone they're blocking
        if (inBlockableState())
        {
            
            if (facingLeft)
            {
                if (ig.currentMove == '3')
                {
                    blockZone = 'l';
                }
                else if (ig.currentMove == '6' || ig.currentMove == '9')
                {
                    blockZone = 'h';
                }
                else
                {
                    blockZone = ' ';
                }
            }
            else if (!facingLeft)
            {
                if (ig.currentMove == '1')
                {
                    blockZone = 'l';
                }
                else if (ig.currentMove == '4' || ig.currentMove == '7')
                {
                    blockZone = 'h';
                }
                else
                {
                    blockZone = ' ';
                }
            }
        }
        else
        {
            blockZone = ' ';
        }

	}
}
