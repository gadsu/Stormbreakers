using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class hitboxScript : MonoBehaviour {

    private BoxCollider2D hitbox;

	// Use this for initialization
	void Start () 
    {
        hitbox = GetComponent<BoxCollider2D>();
	}

    public void enableHitbox()
    {
        hitbox.enabled = true;
    }

    public void disableHitbox()
    {
        hitbox.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
