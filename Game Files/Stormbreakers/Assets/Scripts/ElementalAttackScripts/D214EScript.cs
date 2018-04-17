using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D214EScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.transform.parent && col.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
