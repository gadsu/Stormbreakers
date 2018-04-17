using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L214EScript : MonoBehaviour {

    int duration;
    GameObject player;
    Transform playerTransform;
    CharacterMovement cm;
    PlayerSettings ps;
    Rigidbody2D rb;
    float playerYPos;
    bool ended;
    bool playingEndedAnim;

    bool hasPlayerSet;



	// Use this for initialization
	void Start () 
    {
        //GetComponent<Animator>().Play("LifeTeleportStart0");

        duration = 240;

        /*player = transform.parent.gameObject;
        playerTransform = player.transform;
        cm = player.GetComponent<CharacterMovement>();
        ps = player.GetComponent<PlayerSettings>();*/

        //playerYPos = playerTransform.position.y;

        rb = GetComponent<Rigidbody2D>();

        ended = false;
	
        playingEndedAnim = false;

        hasPlayerSet = false;
    }

    public void setPlayer(Transform t)
    {
        player = t.gameObject;
        playerTransform = player.transform;
        cm = player.GetComponent<CharacterMovement>();
        ps = player.GetComponent<PlayerSettings>();

        cm.setCanMove(false);

        /*if (t.gameObject.name.Contains("player1"))
        {
            GameObject.Find("Camera").GetComponent<CameraScript>().setPlayer1(gameObject);
        }
        else if (t.gameObject.name.Contains("player2"))
        {
            GameObject.Find("Camera").GetComponent<CameraScript>().setPlayer2(gameObject);
        }*/

        //disableEverything();

        hasPlayerSet = true;
    }

    private void disableEverything()
    {
        cm.setCanMove(false);

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; //Ghetto way of making the player ignore gravity
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Animator>().enabled = false; //Band-aid fix. Stops the player from being able to attack while underground.

        foreach (BoxCollider2D bc in player.GetComponents<BoxCollider2D>())
        {
            bc.enabled = false;
        }

        foreach (Transform child in player.transform)
        {
            if (child.tag == "Pushbox")
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    private void enableEverything()
    {
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Animator>().enabled = true;

        foreach (BoxCollider2D bc in player.GetComponents<BoxCollider2D>())
        {
            bc.enabled = true;
        }

        foreach (Transform child in player.transform)
        {
            if (child.tag == "Pushbox")
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (hasPlayerSet)
        {
            /*if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart0") && player.GetComponent<SpriteRenderer>().enabled == true)
            {
                disableEverything();
            }*/
            /*else */if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart") &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart0") &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart1") &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportEnd0") &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportEnd1") &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportEnd2"))
            {
                if (Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > 0)
                {
                    rb.velocity = new Vector2(7.0f, 0.0f);
                }
                else if (Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < 0)
                {
                    rb.velocity = new Vector2(-7.0f, 0.0f);
                }
                else
                {
                    rb.velocity = new Vector2(0.0f, 0.0f);
                }

                playerTransform.position = new Vector2(transform.position.x, playerTransform.position.y);
            }
            else
            {
                rb.velocity = new Vector2(0.0f, 0.0f);
            }


            if ((!Input.GetButton("P" + ps.getPlayerNumber() + "E") && 
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart")  &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart1") &&
                !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportStart2")) || 
                    duration <= 0)
            {
                ended = true;
            }

            if (ended && !playingEndedAnim)
            {

                GetComponent<Animator>().Play("LifeTeleportEnd0");
                playingEndedAnim = true;
            }

            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportEnd1") && player.GetComponent<SpriteRenderer>().enabled == false)
            {
                //cm.setCanMove(true);
                playerTransform.position = new Vector2(transform.position.x, transform.position.y);

                enableEverything();


                /*if (player.name.Contains("player1"))
                {
                    GameObject.Find("Camera").GetComponent<CameraScript>().setPlayer1(player);
                }
                else if (player.name.Contains("player2"))
                {
                    GameObject.Find("Camera").GetComponent<CameraScript>().setPlayer2(player);
                }*/
            }

            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LifeTeleportEnd2"))
            {
                cm.setCanMove(true);
                //playerTransform.position = new Vector2(transform.position.x, transform.position.y);
                Destroy(gameObject);
            }


            duration--;
        }
        else //Band-aid solution. Make this better later.
        {
            setPlayer(player.transform);
        }

	}
}
