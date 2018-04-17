using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class D236EScript : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator an;

    private float speedDecreasePerFrame;
    private bool hasHitFloor;
    private int duration;

	// Use this for initialization
	void Start () 
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();

        speedDecreasePerFrame = 0.6f;
        hasHitFloor = false;
        duration = 0;

        an.Play("DeathAcidRainStart0");
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor" && rb.velocity.y <= 0)
        {
            duration = 30;
            hasHitFloor = true;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y - speedDecreasePerFrame));

        if (rb.velocity.y <= 0 && !an.GetCurrentAnimatorStateInfo(0).IsName("DeathAcidRainEnd0"))
        {
            an.Play("DeathAcidRainEnd0");
        }
        /*else if(!an.GetCurrentAnimatorStateInfo(0).IsName("DeathAcidRainStart0"))
        {
            an.Play("DeathAcidRainStart0");
        }*/

        if (hasHitFloor)
        {
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                duration--;
            }
        }
	}
}
