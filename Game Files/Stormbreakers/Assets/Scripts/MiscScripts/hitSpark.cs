using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitSpark : MonoBehaviour {

    private int duration = 100;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        duration--;

        if (duration < 0)
        {
            Destroy(gameObject);
        }
	}
}
