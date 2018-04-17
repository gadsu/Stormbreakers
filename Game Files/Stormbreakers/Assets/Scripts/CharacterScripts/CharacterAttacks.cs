/*
 * Script to process attack commands it receives from the CommandInterpreter.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CommandInterpreter))]
[RequireComponent(typeof(CharacterState))]

public class CharacterAttacks : MonoBehaviour 
{
    Dictionary<string,string> dic;

    public GameObject Fireball;
    public GameObject fb;
    public CommandInterpreter ci;

    public int frames;
    public bool hasHit;

    public int cancelWindow;
    private const int cancelWindowSize = 30;

    private Animator an;
    private CharacterMovement cm;
    private CharacterState cs;

	// Use this for initialization
	void Start () 
    {
        //Read the contents of text files with the format 'name = content' into a 2d array. 
        // To use, dic["name"] will return the string associated, dic["name"][0] will return the first character.
        TextAsset PlayerFile = new TextAsset();
        hasHit = false;
        if (gameObject.tag == "Randolf")
        {
            PlayerFile = Resources.Load("RandolfData", typeof(TextAsset)) as TextAsset;
        }
        else if (gameObject.tag == "Lynne")
        {
            PlayerFile = Resources.Load("LynneData",typeof(TextAsset)) as TextAsset;
        }

        an = GetComponent<Animator>();
        cm = GetComponent<CharacterMovement>();
        ci = GetComponent<CommandInterpreter>();
        cs = GetComponent<CharacterState>();
//        dic = File.ReadAllLines(fileName).Select(l => l.Split(new[] { '=' })).ToDictionary( s => s[0].Trim(), s => s[1].Trim());
        dic = PlayerFile.text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(new[] { '=' })).ToDictionary( s => s[0].Trim(), s => s[1].Trim());
        //Fireball = Resources.Load("MiscPrefabs/Fireball") as GameObject;


	}


    public bool canCancel(string newMove)
    {
        if ((cs.inBlockableState() ||
            (an.GetCurrentAnimatorStateInfo(0).IsName("airDash")) || an.GetCurrentAnimatorStateInfo(0).IsName("dash")) &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("block") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("airBlock"))
        {
            return true;
        }
        else if (an.GetCurrentAnimatorStateInfo(0).IsName("6B"))
        {
            return false;
        }
        else if (hasHit)
        {
            if (newMove.Contains("236") || newMove.Contains("214") || newMove.Contains("623") || newMove.Contains("super"))
            {
                return true;
            }
            if (newMove[1] == 'A')
            {
                if (an.GetCurrentAnimatorStateInfo(0).IsName("5A") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("2A") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("jA"))
                {
                    return true;
                }

                return false;
            }
            else if (newMove[1] == 'B')
            {
                if ((an.GetCurrentAnimatorStateInfo(0).IsName("5A") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("2A") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("jA") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("5B") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("2B") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("jB")) && 
                    !an.GetCurrentAnimatorStateInfo(0).IsName(newMove))
                {
                    return true;
                }

                return false;
            }
            else if (newMove[1] == 'C')
            {
                if ((an.GetCurrentAnimatorStateInfo(0).IsName("5A") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("2A") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("jA") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("5B") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("2B") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("jB") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("5C") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("2C") ||
                    an.GetCurrentAnimatorStateInfo(0).IsName("jC")) && 
                    !an.GetCurrentAnimatorStateInfo(0).IsName(newMove))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void hitConnect()
    {
        cancelWindow = Time.frameCount + cancelWindowSize;
        hasHit = true;
    }

    //-------------------------------------ATTACKS-------------------------------------//
    //Each attack is named after it's button combination, designed facing the other player.
    //So if player one is on the left and presses 236C, and player 2 is on the right and presses 214C, they will execute the same function.

    public void special236C()
    {
        if (!GetComponent<CharacterMovement>().isinAir() && fb == null)
        {
            cm.dashDirection = '5';
            if (cs.isOnLeft())
            {
                fb = Instantiate(Fireball, new Vector2(gameObject.transform.position.x + 1.0f, gameObject.transform.position.y + 0.65f), gameObject.transform.rotation);
                fb.layer = gameObject.layer;
            }
            else
            {
                fb = Instantiate(Fireball, new Vector2(gameObject.transform.position.x + -1.0f, gameObject.transform.position.y + 0.65f), gameObject.transform.rotation);
                fb.layer = gameObject.layer;
            }

            int vel = 7;

            if (!cs.isOnLeft())
            {
                vel *= -1;
            }

            fb.GetComponent<ProjectileScript>().setHorizontalVelocity(vel);
            fb.GetComponent<ProjectileScript>().setDamage(8);
            fb.GetComponent<ProjectileScript>().setChipDamage(2);
            fb.GetComponent<ProjectileScript>().setHitstun(20);
            fb.GetComponent<ProjectileScript>().setPushback(new Vector2(6.0f, 0.0f));
        }
    }

    public void special236B()
    {
        if (!GetComponent<CharacterMovement>().isinAir() && fb == null)
        {
            cm.dashDirection = '5';
            if (cs.isOnLeft())
            {
                fb = Instantiate(Fireball, new Vector2(gameObject.transform.position.x + 1.0f, gameObject.transform.position.y + 0.65f), gameObject.transform.rotation);
                fb.layer = gameObject.layer;
            }
            else
            {
                fb = Instantiate(Fireball, new Vector2(gameObject.transform.position.x + -1.0f, gameObject.transform.position.y + 0.65f), gameObject.transform.rotation);
                fb.layer = gameObject.layer;
            }

            int vel = 8;

            if (!cs.isOnLeft())
            {
                vel *= -1;
            }

            fb.GetComponent<ProjectileScript>().setHorizontalVelocity(vel);
            fb.GetComponent<ProjectileScript>().setDamage(7);
            fb.GetComponent<ProjectileScript>().setChipDamage(1);
            fb.GetComponent<ProjectileScript>().setHitstun(20);
            fb.GetComponent<ProjectileScript>().setPushback(new Vector2(6.0f, 0.0f));
        }
    }

    public void special236A()
    {
        if (!GetComponent<CharacterMovement>().isinAir() && fb == null)
        {
            cm.dashDirection = '5';
            if (cs.isOnLeft())
            {
                fb = Instantiate(Fireball, new Vector2(gameObject.transform.position.x + 1.0f, gameObject.transform.position.y + 0.65f), gameObject.transform.rotation);
                fb.layer = gameObject.layer;
            }
            else
            {
                fb = Instantiate(Fireball, new Vector2(gameObject.transform.position.x + -1.0f, gameObject.transform.position.y + 0.65f), gameObject.transform.rotation);
                fb.layer = gameObject.layer;
            }

            int vel = 9;

            if (!cs.isOnLeft())
            {
                vel *= -1;
            }

            fb.GetComponent<ProjectileScript>().setHorizontalVelocity(vel);
            fb.GetComponent<ProjectileScript>().setDamage(6);
            fb.GetComponent<ProjectileScript>().setChipDamage(1);
            fb.GetComponent<ProjectileScript>().setHitstun(20);
            fb.GetComponent<ProjectileScript>().setPushback(new Vector2(6.0f, 0.0f));
        }
    }


    public void special214C()
    {
		if (GetComponent<RandolfSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
		{
            cm.dashDirection = '5';
			cs.setDamage(int.Parse(dic["dmg214C"]));
			cs.setZone(dic["zone214C"][0]);
            cs.setAttribute(dic["att214C"][0]);
            cs.setLevel(int.Parse(dic["lvl214C"]));
			GetComponent<RandolfSpecials>().special214C();
		}
		else if (GetComponent<LynneSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
		{
            cm.dashDirection = '5';
			cs.setDamage(int.Parse(dic["dmg214C"]));
			cs.setZone(dic["zone214C"][0]); 
            cs.setAttribute(dic["att214C"][0]);
            cs.setLevel(int.Parse(dic["lvl214C"]));
			GetComponent<LynneSpecials>().special214C();
		}

    }

    public void special214B()
    {
        if (GetComponent<RandolfSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg214B"]));
            cs.setZone(dic["zone214B"][0]); 
            cs.setAttribute(dic["att214B"][0]);
            cs.setLevel(int.Parse(dic["lvl214B"]));
            GetComponent<RandolfSpecials>().special214B();
        }
        else if (GetComponent<LynneSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg214B"]));
            cs.setZone(dic["zone214B"][0]); 
            cs.setAttribute(dic["att214B"][0]);
            cs.setLevel(int.Parse(dic["lvl214B"]));
            GetComponent<LynneSpecials>().special214B();
        }
    }

    public void special214A()
    {
        if (GetComponent<RandolfSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg214A"]));
            cs.setZone(dic["zone214A"][0]); 
            cs.setAttribute(dic["att214A"][0]);
            cs.setLevel(int.Parse(dic["lvl214A"]));
            GetComponent<RandolfSpecials>().special214A();
        }
        else if (GetComponent<LynneSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg214A"]));
            cs.setZone(dic["zone214A"][0]); 
            cs.setAttribute(dic["att214A"][0]);
            cs.setLevel(int.Parse(dic["lvl214A"]));
            GetComponent<LynneSpecials>().special214A();
        }
    }

    public void special623C()
    {
        if (GetComponent<RandolfSpecials>()/* && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir()*/)
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg623C"]));
            cs.setZone(dic["zone623C"][0]);
            cs.setAttribute(dic["att623C"][0]);
            cs.setLevel(int.Parse(dic["lvl623C"]));

            GetComponent<RandolfSpecials>().special623C();
        }
        else if (GetComponent<LynneSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg623C"]));
            cs.setZone(dic["zone623C"][0]); 
            cs.setAttribute(dic["att623C"][0]);
            cs.setLevel(int.Parse(dic["lvl623C"]));
            GetComponent<LynneSpecials>().special623C();
        }
    }

    public void special623B()
    {
        if (GetComponent<RandolfSpecials>()/* && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir()*/)
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg623B"]));
            cs.setZone(dic["zone623B"][0]);
            cs.setAttribute(dic["att623B"][0]); 
            cs.setLevel(int.Parse(dic["lvl623B"]));
            GetComponent<RandolfSpecials>().special623B();
        }
        else if (GetComponent<LynneSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg623B"]));
            cs.setZone(dic["zone623B"][0]);
            cs.setAttribute(dic["att623B"][0]);
            cs.setLevel(int.Parse(dic["lvl623B"]));
            GetComponent<LynneSpecials>().special623B();
        }
    }

    public void special623A()
    {
        if (GetComponent<RandolfSpecials>()/* && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir()*/)
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg623A"]));
            cs.setZone(dic["zone623A"][0]);
            cs.setAttribute(dic["att623A"][0]);
            cs.setLevel(int.Parse(dic["lvl623A"]));
            GetComponent<RandolfSpecials>().special623A();
        }
        else if (GetComponent<LynneSpecials>() && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir())
        {
            cm.dashDirection = '5';
            cs.setDamage(int.Parse(dic["dmg623A"]));
            cs.setZone(dic["zone623A"][0]);
            cs.setAttribute(dic["att623A"][0]); 
            cs.setLevel(int.Parse(dic["lvl623A"]));
            GetComponent<LynneSpecials>().special623A();
        }
    }

    public void nThrow()
    {
        cm.dashDirection = '5';
        cs.setDamage(0);
        cs.setZone(dic["zonenThrow"][0]); 
        cs.setAttribute(dic["attnThrow"][0]);
        cs.setLevel(int.Parse(dic["lvlnThrow"]));
        an.Play("nThrow");

    }

    public void nThrowHit()
    {
        cs.setDamage(int.Parse(dic["dmgnThrow"]));
        an.Play("nThrowHit");
    }


    public void fiveA()
    {
        if (canCancel("5A"))
        {
            cm.dashDirection = '5';
            an.Play("5A");
            cs.setDamage(int.Parse(dic["dmg5A"]));
            cs.setZone(dic["zone5A"][0]);
            cs.setAttribute(dic["att5A"][0]);
            cs.setLevel(int.Parse(dic["lvl5A"]));
        }
    }

    public void twoA()
    {
        if (canCancel("2A"))
        {
            cm.dashDirection = '5';
            an.Play("2A");
            cs.setDamage(int.Parse(dic["dmg2A"]));
            cs.setZone(dic["zone2A"][0]);
            cs.setAttribute(dic["att2A"][0]);
            cs.setLevel(int.Parse(dic["lvl2A"]));
        }
    }

    public void jA()
    {
        if (canCancel("jA"))
        {
            cm.dashDirection = '5';
            an.Play("jA");
            cs.setDamage(int.Parse(dic["dmgjA"]));
            cs.setZone(dic["zonejA"][0]); 
            cs.setAttribute(dic["attjA"][0]);
            cs.setLevel(int.Parse(dic["lvljA"]));
        }
    }

    public void sixB()
    {
        if (canCancel("6B"))
        {
            //cm.dashDirection = '5';
            if (cs.isOnLeft())
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x / 2.0f) + 3.0f, 9.0f);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x / 2.0f) - 3.0f, 9.0f);
            }
            an.Play("6B");
            cs.setDamage(int.Parse(dic["dmg6B"]));
            cs.setZone(dic["zone6B"][0]);
            cs.setAttribute(dic["att6B"][0]);
            cs.setLevel(int.Parse(dic["lvl6B"]));
        }
    }

    public void fiveB()
    { 
        if (canCancel("5B"))
        {
            cm.dashDirection = '5';
            an.Play("5B");
            cs.setDamage(int.Parse(dic["dmg5B"]));
            cs.setZone(dic["zone5B"][0]);
            cs.setAttribute(dic["att5B"][0]);

        }
    }

    public void twoB()
    {
        if (canCancel("2B"))
        {
            cm.dashDirection = '5';
            an.Play("2B");
            cs.setDamage(int.Parse(dic["dmg2B"]));
            cs.setZone(dic["zone2B"][0]);
            cs.setAttribute(dic["att2B"][0]);
            cs.setLevel(int.Parse(dic["lvl2B"]));
        }
    }

    public void jB()
    {
        if (canCancel("jB"))
        {
            cm.dashDirection = '5';
            an.Play("jB");
            cs.setDamage(int.Parse(dic["dmgjB"]));
            cs.setZone(dic["zonejB"][0]);
            cs.setAttribute(dic["attjB"][0]);
            cs.setLevel(int.Parse(dic["lvljB"]));
        }
    }

    public void fiveC()
    { 
        if (canCancel("5C"))
        {
            cm.dashDirection = '5';
            an.Play("5C");
            cs.setDamage(int.Parse(dic["dmg5C"]));
            cs.setZone(dic["zone5C"][0]);
            cs.setAttribute(dic["att5C"][0]);
            cs.setLevel(int.Parse(dic["lvl5C"]));
        }
    }

    public void twoC()
    {
        if (canCancel("2C"))
        {
            cm.dashDirection = '5';
            an.Play("2C");
            cs.setDamage(int.Parse(dic["dmg2C"]));
            cs.setZone(dic["zone2C"][0]); 
            cs.setAttribute(dic["att2C"][0]);
            cs.setLevel(int.Parse(dic["lvl2C"]));
        }
    }

    public void jC()
    {
        if (canCancel("jC"))
        {
            an.Play("jC");
            cs.setDamage(int.Parse(dic["dmgjC"]));
            cs.setZone(dic["zonejC"][0]);
            cs.setAttribute(dic["attjC"][0]);
            cs.setLevel(int.Parse(dic["lvljC"]));
        }
    }

    public void fiveE()
    { 
        if (!an.GetCurrentAnimatorStateInfo(0).IsName("5A") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("2A") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("5B") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("2B") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("5C") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("2C") &&
 //           !an.GetCurrentAnimatorStateInfo(0).IsName("charge") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("236") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("214") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("623"))
        {
            an.Play("charge");
            if (((frames % 10) == 9) && Time.timeScale != 0)
            {
                cs.useSpecial(1);
            }
            frames++; 
        }
    }

    public void twoE()
    {
        /*if (cs.specialBar.value > 10)
        {
            cs.useSpecial(-10);
        }*/
    }

    public void jE()
    {
        /*if (cs.specialBar.value > 15)
        {
            cs.useSpecial(-15);
        }*/
    }

    public bool isAttacking()
    {
        if(an.GetCurrentAnimatorStateInfo(0).IsName("5A") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("2A") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("jA") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("5B") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("2B") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("jB") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("5C") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("2C") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("jC") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("214")||
            an.GetCurrentAnimatorStateInfo(0).IsName("236C") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("236B") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("236A") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("623C") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("623B") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("623A") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("T236E") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("S236E") ||
            an.GetCurrentAnimatorStateInfo(0).IsName("S214E")||
            an.GetCurrentAnimatorStateInfo(0).IsName("TSuper")  )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void crouch()
    {
        if (!an.GetCurrentAnimatorStateInfo(0).IsName("crouchIdle") && !an.GetCurrentAnimatorStateInfo(0).IsName("charge") &&
            !an.GetCurrentAnimatorStateInfo(0).IsName("crouch") && !an.GetCurrentAnimatorStateInfo(0).IsTag("CrouchAttack") && !ci.crouching && cm.canMove)
        {
            an.Play("crouch");
        }
        else if(!an.GetCurrentAnimatorStateInfo(0).IsTag("CrouchAttack") && !an.GetCurrentAnimatorStateInfo(0).IsName("charge") && cm.canMove)
        {
            an.Play("crouchIdle");
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if ((hasHit && Time.frameCount >= cancelWindow) /*TODO: Put an additional condition to see if the animation has changed since the attack hit*/)//hasHit && cs.inBlockableState())
        {
            hasHit = false;
        }
    }
}
