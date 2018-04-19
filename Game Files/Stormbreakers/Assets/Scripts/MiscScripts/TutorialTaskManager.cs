using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CommandInterpreter))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(Animator))]

public class TutorialTaskManager : MonoBehaviour {

	// Use this for initialization

    //Move forward/backward
    //Crouch
    //Jump
    //Doublejump
    //Dash forward
    //Dash backward
    //Airdash

    //Block mid
    //Block low
    //Standing, crouching, and jumping light
    //Stand crouch jump med
    //stand crouch jump heavy
    //throw
    //backthrow
    //throwbreak
    //(Forward and medium overhead)
    //Fireball
    //Slide
    //Uppercut
    //Charging with E
    //Regular super

    //CommandInterpreter ci;
    TextDisplayScript tds;
    //CharacterMovement cm;
    //CharacterState cs;
    Animator anim;

    bool[] completed;

    public int counter;

    void Start () 
    {
        //ci = GetComponent<CommandInterpreter>();
        tds = GameObject.Find("Camera").GetComponent<TextDisplayScript>();
        //cm = GetComponent<CharacterMovement>();
        //cs = GetComponent<CharacterState>();
        anim = GetComponent<Animator>();
        completed = new bool[3];


        for (int i = 0; i < completed.Length; i++)
        {
            completed[i] = false;
        }
	}

    void resetCompletedArray()
    {
        for (int i = 0; i < completed.Length; i++)
        {
            completed[i] = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!tds.taskComplete)
        {
            switch (counter)
            {

                //Wait... Why are we doing all this dumb stuff again? Why not just check to see if the respective animation is playing, instead of trying to check for all the conditions?
                /*case 0: //Move forward/backward
                    if(ci.isWalking())
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 1: //Crouch
                    if(ci.isCrouching())
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 2: //Jump
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 3: //Double Jump
                    if (cm.isinAir() && )
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 4: //Forward Dash
                    if ((cs.isOnLeft() && ci.hasDash() && cm.getDashDirection() == '6') || (!cs.isOnLeft() && ci.hasDash() && cm.getDashDirection() == '4'))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 5: //Backdash
                    if ((cs.isOnLeft() && ci.hasDash() && cm.getDashDirection() == '4') || (!cs.isOnLeft() && ci.hasDash() && cm.getDashDirection() == '6'))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 6: //Airdash
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 7: //Mid block
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 8: //Low Block
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 9: //Light Attacks
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 10: //Medium Attacks
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 11: //Heavy Attacks
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 12: //Throw
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 13: //Backthrow
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 14: //Throw Break
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 15: //(6M overhead??)
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 16: //Fireball
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 17: //Slide
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 18: //Uppercut
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 19: //Charging Energy
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 20: //Super Attack
                    if ()
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;*/

                case 0: //Move forward/backward
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 1: //Crouch
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("crouchIdle"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 2: //Jump
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 3: //Double Jump
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("dJump"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 4: //Forward Dash
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 5: //Backdash
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("backDash"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 6: //Airdash
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("airDash"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 7: //Mid block
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("block") || anim.GetCurrentAnimatorStateInfo(0).IsName("crouchBlock")) //Temporary, until we get Randolf's "AI" working in the tutorial.
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 8: //Low Block
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("crouchBlock"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 9: //Light Attacks
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("5A"))
                    {
                        completed[0] = true;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("2A"))
                    {
                        completed[1] = true;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("jA"))
                    {
                        completed[2] = true;
                    }

                    if (completed[0] == true && completed[1] == true && completed[2] == true)
                    {
                        resetCompletedArray();
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 10: //Medium Attacks
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("5B"))
                    {
                        completed[0] = true;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("2B"))
                    {
                        completed[1] = true;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("jB"))
                    {
                        completed[2] = true;
                    }

                    if (completed[0] == true && completed[1] == true && completed[2] == true)
                    {
                        resetCompletedArray();
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 11: //Heavy Attacks
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("5C"))
                    {
                        completed[0] = true;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("2C"))
                    {
                        completed[1] = true;
                    }
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("jC"))
                    {
                        completed[2] = true;
                    }

                    if (completed[0] == true && completed[1] == true && completed[2] == true)
                    {
                        resetCompletedArray();
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 12: //Throw
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("nThrowHit"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 13: //Backthrow
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("nThrowHit")) //Fix once we get a backthrow animation
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 14: //Throw Break
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("nThrowHit")) //Fix once we get a throw tech animation
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 15: //(6M overhead??)
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("6B"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 16: //Fireball
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("236A") || anim.GetCurrentAnimatorStateInfo(0).IsName("236B") || anim.GetCurrentAnimatorStateInfo(0).IsName("236C"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 17: //Slide
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("slide"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 18: //Uppercut
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("623A") || anim.GetCurrentAnimatorStateInfo(0).IsName("623B") || anim.GetCurrentAnimatorStateInfo(0).IsName("623C"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 19: //Charging Energy
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("charge"))
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
                case 20: //Super Attack
                    if (true) //Until we get a super animation.
                    {
                        counter++;
                        tds.taskComplete = true;
                    }
                    break;
            }
        }
        
    }
}
