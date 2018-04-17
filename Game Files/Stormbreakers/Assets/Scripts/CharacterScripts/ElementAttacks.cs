/*
 * File that contains the logic for all the elemental attacks and effects.
 * Reads from a file for damage and zone, just like the character Attacks.
 * This file is going to be massive after it's done... ;_;
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CommandInterpreter))]

public class ElementAttacks : MonoBehaviour 
{
    
    Dictionary<string,string> dic;

    private CharacterState cs;
    private Animator anim;
    private Rigidbody2D rb;
    private CommandInterpreter ci;

    private GameObject l236EPrefab;
    private GameObject l236E;

    private GameObject l214EPrefab;
    private GameObject l214E;

    private GameObject lSuperPrefab;
    private GameObject lSuper;

    private GameObject d236EPrefab;
    private GameObject d236E;

    private GameObject d214EPrefab;
    private GameObject d214E;

    private GameObject dSuperPrefab;
    private GameObject dSuper;

    private GameObject t214EPrefab;
    private GameObject t214E;

	private GameObject sSuperPrefab;
	private GameObject sSuper;

	private AudioClip deathLaserSound;
	private AudioClip healingFieldSound;
	private AudioClip acidRainSound;
	private AudioClip vineSound;

    private bool isRushing;
    private float rushFrames;
    // Use this for initialization
    void Start () 
    {
        //Read the contents of text files with the format 'name = content' into a 2d array. 
        // To use, dic["name"] will return the string associated, dic["name"][0] will return the first character.
        //string fileName;
        TextAsset elementData = new TextAsset();

        cs = GetComponent<CharacterState>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ci = GetComponent<CommandInterpreter>();
        //fileName = "Assets/Text Files/ElementData.txt";
        elementData = Resources.Load("ElementData", typeof(TextAsset)) as TextAsset;
 //       dic = File.ReadAllLines(fileName).Select(l => l.Split(new[] { '=' })).ToDictionary( s => s[0].Trim(), s => s[1].Trim());
        dic = elementData.text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(new[] { '=' })).ToDictionary( s => s[0].Trim(), s => s[1].Trim());

		deathLaserSound = Resources.Load ("Sounds/Death_Laser", typeof(AudioClip)) as AudioClip;
		healingFieldSound = Resources.Load ("Sounds/Healing_Field", typeof(AudioClip)) as AudioClip;
		acidRainSound = Resources.Load ("Sounds/Acid_Rain", typeof(AudioClip)) as AudioClip;
		vineSound = Resources.Load ("Sounds/Vines", typeof(AudioClip)) as AudioClip;

        l236EPrefab = Resources.Load("ElementPrefabs/l236EPrefab", typeof(GameObject)) as GameObject;
        l214EPrefab = Resources.Load("ElementPrefabs/l214EPrefab", typeof(GameObject)) as GameObject;
        lSuperPrefab = Resources.Load("ElementPrefabs/lSuperPrefab", typeof(GameObject)) as GameObject;

        d236EPrefab = Resources.Load("ElementPrefabs/d236EPrefab", typeof(GameObject)) as GameObject;
        d214EPrefab = Resources.Load("ElementPrefabs/d214EPrefab", typeof(GameObject)) as GameObject;
        dSuperPrefab = Resources.Load("ElementPrefabs/dSuperPrefab", typeof(GameObject)) as GameObject;

        t214EPrefab = Resources.Load("ElementPrefabs/t214EPrefab", typeof(GameObject)) as GameObject;

		sSuperPrefab = Resources.Load("ElementPrefabs/sSuperPrefab", typeof(GameObject)) as GameObject;
    }

    public bool superPrefabActive()
    {
        if (lSuper == null && dSuper == null && sSuper == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool special214PrefabActive()
    {
        if (l214E == null && d214E == null && t214E == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool special236PrefabActive()
    {
        if (l236E == null && d236E == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    //-------------------------------------ATTACKS-------------------------------------//
    //Each attack is named after it's button combination, designed facing the other player.
    //So if player one is on the left and presses 236C, and player 2 is on the right and presses 214C, they will execute the same function.



    //-------------------------------------LIFE-------------------------------------//

    public void L236E() //Vine Garden
    {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("L236E"))
        {
            if (!GetComponent<CharacterMovement>().isinAir() && l236E == null)
            {
                //anim.Play("L236E");
                AudioSource.PlayClipAtPoint(vineSound, GameObject.Find("Camera").transform.position);

                if (GetComponent<CharacterState>().isOnLeft())
                {
                    l236E = Instantiate(l236EPrefab, new Vector3(transform.position.x + 1.5f, transform.position.y + 2.0f, transform.position.z), transform.rotation);
                }
                else
                {
                    l236E = Instantiate(l236EPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y + 2.0f, transform.position.z), transform.rotation);
                }

                l236E.GetComponent<ProjectileScript>().setDamage(6);
                l236E.GetComponent<ProjectileScript>().setChipDamage(2);
                l236E.GetComponent<ProjectileScript>().setHitstun(120);
                l236E.GetComponent<ProjectileScript>().setPushback(new Vector2(0.0f, 0.0f));
                l236E.layer = gameObject.layer;
            }
        }
    }

    public void L214E() //Vine Transport
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("L214E"))
        {
            if (GetComponent<CharacterMovement>().canWalk() && l214E == null)
            {
                //anim.Play("L214E");
                AudioSource.PlayClipAtPoint(vineSound, GameObject.Find("Camera").transform.position);

                l214E = Instantiate(l214EPrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation);

                //l214E.layer = gameObject.layer;

                l214E.GetComponent<L214EScript>().setPlayer(gameObject.transform);

            }
        }
    }

	public void LSuper() //Soul Transfusion (Healing Field)
    {
        if (cs.special >= 100 || cs.specialLevel > 0)
        {
            if (!GetComponent<CharacterMovement>().isinAir() && lSuper == null)
            {
                cs.useSpecial(-100);

                lSuper = Instantiate(lSuperPrefab, transform.position, transform.rotation, gameObject.transform);

                lSuper.layer = gameObject.layer;
            }
        }
    }

    //-------------------------------------DEATH-------------------------------------//

    public void D236E() //Acid Rain
    {
        //if (cs.special >= 5)
        {
            if (!GetComponent<CharacterMovement>().isinAir() && d236E == null)
            {
                //cs.useSpecial(-5);

                //anim.Play("D236E");
                AudioSource.PlayClipAtPoint(acidRainSound, GameObject.Find("Camera").transform.position);


                d236E = Instantiate(d236EPrefab, transform.position, transform.rotation);
                d236E.GetComponent<ProjectileScript>().setVerticalVelocity(30);

                d236E.GetComponent<ProjectileScript>().setDamage(7);
                d236E.GetComponent<ProjectileScript>().setChipDamage(2);
                d236E.GetComponent<ProjectileScript>().setHitstun(20);
                d236E.GetComponent<ProjectileScript>().setPushback(new Vector2(3.0f, 0.0f));

                d236E.layer = gameObject.layer;
            }
        }
    }

	public void D214E() //Plague Beam (Death Laser)
    {
        Debug.Log("214E");

        //if (cs.special >= 10)
        {
            if (!GetComponent<CharacterMovement>().isinAir() && d214E == null)
            {
                //cs.useSpecial(-10);

                //anim.Play("D214E");
                AudioSource.PlayClipAtPoint(deathLaserSound, GameObject.Find("Camera").transform.position);

                if (GetComponent<CharacterState>().isOnLeft())
                {
                    d214E = Instantiate(d214EPrefab, new Vector3(transform.position.x + 5.0f, transform.position.y, transform.position.z), transform.rotation);
                    d214E.GetComponent<ProjectileScript>().setHorizontalVelocity(30);
                }
                else
                {
                    d214E = Instantiate(d214EPrefab, new Vector3(transform.position.x - 5.0f, transform.position.y, transform.position.z), transform.rotation);
                    d214E.GetComponent<ProjectileScript>().setHorizontalVelocity(-30);
                }

                d214E.GetComponent<ProjectileScript>().setDamage(5);
                d214E.GetComponent<ProjectileScript>().setChipDamage(2);
                d214E.GetComponent<ProjectileScript>().setHitstun(20);
                d214E.GetComponent<ProjectileScript>().setPushback(new Vector2(20.0f, 0.0f));

                d214E.layer = gameObject.layer;
            }
        }

    }

	public void DSuper() //Impending Death (Ball of gross)
    {
        Debug.Log("DSuper");

        if (cs.special >= 100 || cs.specialLevel > 0)
        {
            if (!GetComponent<CharacterMovement>().isinAir() && dSuper == null)
            {
                cs.useSpecial(-100);

                if (cs.isOnLeft())
                {
                    dSuper = Instantiate(dSuperPrefab, new Vector3(transform.position.x + 3.0f, transform.position.y), transform.rotation);
                }
                else
                {
                    dSuper = Instantiate(dSuperPrefab, new Vector3(transform.position.x - 3.0f, transform.position.y), transform.rotation);
                }


                dSuper.GetComponent<ProjectileScript>().setDamage(2);
                dSuper.GetComponent<ProjectileScript>().setChipDamage(1);
                dSuper.GetComponent<ProjectileScript>().setHitstun(5);
                dSuper.GetComponent<ProjectileScript>().setPushback(new Vector2(0.5f, 0.0f));

                Debug.Log("Player Number is: " + gameObject.GetComponent<PlayerSettings>().getPlayerNumber());
                dSuper.GetComponent<DSuperScript>().setAttributes(gameObject.GetComponent<PlayerSettings>().getPlayerNumber());
                Debug.Log("Player Number is: " + gameObject.GetComponent<PlayerSettings>().getPlayerNumber());

                dSuper.layer = gameObject.layer;
            }
        }
    }

    //-------------------------------------SPACE-------------------------------------//

    public void S236E() //Gravity Armor next special move used has 1 hit of armor during starttup
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("S236E"))
        {
            anim.Play("S236E"); //Armor applied in animation
        }
    }

    public void S214E() //Reality Distortion dodge away from screen
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("S214E"))
        {
            anim.Play("S214E"); //Hitboxes are enabled in disabled in Animation
        }
    }

	public void SSuper() //Spatial Rejection (Kamehameha) explodes out frim player
    {
		Debug.Log("SSuper");

        if (cs.special >= 100 || cs.specialLevel > 0)
		{
			if (!GetComponent<CharacterMovement>().isinAir())
			{
				cs.useSpecial(-100);

				

				sSuper = Instantiate(sSuperPrefab, transform.position, transform.rotation);

				sSuper.GetComponent<ProjectileScript>().setDamage(10);
				sSuper.GetComponent<ProjectileScript>().setChipDamage(5);
				sSuper.GetComponent<ProjectileScript>().setHitstun(5);
				sSuper.GetComponent<ProjectileScript>().setPushback(new Vector2(13f, 10.0f));

				sSuper.layer = gameObject.layer;
			}
		}
    }

    //-------------------------------------TIME-------------------------------------//

    public void T236E()  //Reverse Strike
    {
        anim.Play("T236E");
    }

    public void T214E() //Alter Perception -- Deals no damage, makes opponent move slower
    {


        if (!GetComponent<CharacterMovement>().isinAir() && t214E == null)
        {


            //anim.Play("236"); //change later
       //     AudioSource.PlayClipAtPoint(Sound, GameObject.Find("Camera").transform.position);

            if (GetComponent<CharacterState>().isOnLeft())
            {
                t214E = Instantiate(t214EPrefab, new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z), transform.rotation);
                t214E.GetComponent<ProjectileScript>().setHorizontalVelocity(3);
            }
            else
            {
                t214E = Instantiate(t214EPrefab, new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z), transform.rotation);
                t214E.GetComponent<ProjectileScript>().setHorizontalVelocity(-3);
            }

            t214E.GetComponent<ProjectileScript>().setDamage(0);
            t214E.GetComponent<ProjectileScript>().setChipDamage(0);
            t214E.GetComponent<ProjectileScript>().setHitstun(10);
            t214E.GetComponent<ProjectileScript>().setPushback(new Vector2(6.0f, 0.0f));

            t214E.layer = gameObject.layer;
        }


    }

	public void TSuper() //Time Skip (rush)
    {
        if (cs.special >= 100 || cs.specialLevel > 0)
        {
            if (!GetComponent<CharacterMovement>().isinAir() && dSuper == null)
            {
                cs.useSpecial(-100);
                anim.Play("TSuper");
                isRushing = true;
                rushFrames = Time.frameCount;
                if (cs.isOnLeft())
                {
                    rb.velocity = new Vector2(30.0f, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-30.0f, rb.velocity.y);
                }
                ci.isInterpretingInputs = false;
            }
        }
    }









    public void fiveE()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("5E"))
        {
            anim.Play("5A");
            GetComponent<CharacterState>().setDamage(int.Parse(dic["dmg5A"]));
            GetComponent<CharacterState>().setZone(dic["zone5A"][0]);
        }
    }
	// Update is called once per frame
	void Update () {
        if (isRushing && Time.frameCount > rushFrames + 1)
        {
            isRushing = false;
            ci.isInterpretingInputs = false;
        }
	}
}
