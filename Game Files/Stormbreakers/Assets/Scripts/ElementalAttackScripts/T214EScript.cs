using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T214EScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != gameObject.layer)
        {
            if (!(col.gameObject.GetComponent<CharacterState>() && col.gameObject.GetComponent<CharacterState>().isInvuln))
            {
                if (col.gameObject.layer == 11 || col.gameObject.layer == 10)
                {
                    col.gameObject.GetComponent<CharacterMovement>().slowDown();
                }

                Destroy(gameObject);
            }
        }
    }
}
