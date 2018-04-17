using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHitboxScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.GetComponentInParent<CharacterState>().zone);
        if (col.gameObject != transform.parent && col.GetComponent<CharacterState>() && !col.GetComponent<CharacterState>().isInvuln)
        {
            Debug.Log("Throw?");
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
