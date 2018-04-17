using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {

    void Start () 
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != gameObject.layer)
        {
            if (!(col.gameObject.GetComponent<CharacterState>() && col.gameObject.GetComponent<CharacterState>().isInvuln))
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () 
    {

    }
}
