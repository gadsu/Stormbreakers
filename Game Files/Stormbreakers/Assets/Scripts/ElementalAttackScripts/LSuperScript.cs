using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSuperScript : MonoBehaviour {

    private int duration = 240; //4 Seconds
    private const int tickRate = 10; //Gives health every 20 frames
    private const int healAmount = 1;

	// Use this for initialization
	void Start () 
    {
            
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (duration % tickRate == 0)
        {
            if (!(transform.parent.gameObject.GetComponent<CharacterState>().healthBar.value + healAmount > 100))
            {
                transform.parent.gameObject.GetComponent<CharacterState>().takeDamage(-1 * healAmount);
                transform.parent.gameObject.GetComponent<CharacterState>().healthBar.value += healAmount; //Fix this later. DON'T RELY ON PUBLIC VARIABLES!! >:O
                }
            else
            {
                transform.parent.gameObject.GetComponent<CharacterState>().takeDamage(-1 * (100 - (int)transform.parent.gameObject.GetComponent<CharacterState>().healthBar.value));
                transform.parent.gameObject.GetComponent<CharacterState>().healthBar.value += 100 - transform.parent.gameObject.GetComponent<CharacterState>().healthBar.value; //Fix this later. DON'T RELY ON PUBLIC VARIABLES!! >:O
            }

        }
        else if (duration <= 0)
        {
            Destroy(gameObject);
        }



        duration--;
	}
}
