using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSuperScript : MonoBehaviour {

    private char ownerID;
    private Transform enemyTransform;
    private int numberOfHits;
    private float speed;
    private int duration;
    private int timeBetweenHits; // in frames

	// Use this for initialization
	void Start () 
    {
        ownerID = '0';
        numberOfHits = 9;
        speed = 0.06f;
        duration = 360; //6 seconds
        timeBetweenHits = 5;
	}

    public void setAttributes(char ownID)
    {
        ownerID = ownID;

        if (ownerID == '1')
        {
            if (GameObject.Find("player2L"))
            {
                enemyTransform = GameObject.Find("player2L").transform;
            }
            else if (GameObject.Find("player2R"))
            {
                enemyTransform = GameObject.Find("player2R").transform;
            }
        }
        else if (ownerID == '2')
        {
            if (GameObject.Find("player1L"))
            {
                enemyTransform = GameObject.Find("player1L").transform;
            }
            else if (GameObject.Find("player1R"))
            {
                enemyTransform = GameObject.Find("player1R").transform;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.layer != col.gameObject.layer) 
        {
            if (col.gameObject.name.Contains("player"))
            {
                numberOfHits--;
            }
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (gameObject.GetComponent<BoxCollider2D>().enabled)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if(Time.frameCount % timeBetweenHits == 0)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (enemyTransform)
        {
            if (enemyTransform.position.x > transform.position.x)
            {
                transform.position = new Vector2(transform.position.x + speed, transform.position.y);
            }
            else if (enemyTransform.position.x < transform.position.x)
            {
                transform.position = new Vector2(transform.position.x - speed, transform.position.y);
            }

            if (enemyTransform.position.y > transform.position.y)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + speed);
            }
            else if (enemyTransform.position.y < transform.position.y)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - speed);
            }
        }

        duration--;

        if (numberOfHits == 0 || duration <= 0)
        {
            Destroy(gameObject);
        }

       
	}
}
