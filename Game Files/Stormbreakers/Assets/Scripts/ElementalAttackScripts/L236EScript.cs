using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L236EScript : MonoBehaviour {

    private int duration; //In frames
    private bool active1;
    private bool active2;

    private CharacterMovement enemyCM;
    private Rigidbody2D enemyRB;

    private bool hasBeenTriggered;


	// Use this for initialization
	void Start () 
    {
        duration = 300;
        active1 = false;
        active2 = false;

        hasBeenTriggered = false;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != gameObject.layer && (col.gameObject.tag.Contains("Randolf") || col.gameObject.tag.Contains("Lynne")) && !hasBeenTriggered)
        {
            if (col.gameObject.GetComponent<CharacterMovement>() && !col.gameObject.GetComponent<CharacterMovement>().isinAir())
            {
                enemyCM = col.gameObject.GetComponent<CharacterMovement>();
                enemyCM.setCanMove(false);

                enemyRB = col.gameObject.GetComponent<Rigidbody2D>();
                enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;



                col.gameObject.transform.position = new Vector2(gameObject.transform.position.x, -3.45f);

                GetComponent<Animator>().Play("L236E");
                active1 = true;
                duration = (int)(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 60.0f);
                hasBeenTriggered = true;
            }

        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer != gameObject.layer && (col.gameObject.tag.Contains("Randolf") || col.gameObject.tag.Contains("Lynne")) && !hasBeenTriggered)
        {
            if (col.gameObject.GetComponent<CharacterMovement>() && !col.gameObject.GetComponent<CharacterMovement>().isinAir())
            {
                enemyCM = col.gameObject.GetComponent<CharacterMovement>();
                enemyCM.setCanMove(false);

                enemyRB = col.gameObject.GetComponent<Rigidbody2D>();
                enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;



                col.gameObject.transform.position = new Vector2(gameObject.transform.position.x, -3.45f);

                GetComponent<Animator>().Play("L236E");
                active1 = true;
                duration = (int)(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 60.0f);
                hasBeenTriggered = true;
            }

        }
    }
	
	// Update is called once per frame
	void Update () 
    {

        duration--;

        if (duration <= 0)
        {
            if (enemyCM)
            {
                enemyCM.setCanMove(true);
            }
            if (enemyRB)
            {
                enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            Destroy(gameObject);
        }
        else if (active2)
        {
            duration = (int)(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 60.0f);
            active1 = false;
            active2 = false;

            GetComponent<ProjectileScript>().setDamage(0);
        }
        else if (active1)
        {
            active2 = true;
        }

	}
}
