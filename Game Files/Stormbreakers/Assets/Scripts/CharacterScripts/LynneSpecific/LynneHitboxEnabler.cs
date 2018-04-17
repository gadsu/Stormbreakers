using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LynneHitboxEnabler : MonoBehaviour {

    private List<GameObject> hitboxObjects;

	// Use this for initialization
	void Start () 
    {
        hitboxObjects = new List<GameObject>();
        initializeHitboxList();
        //printHitboxList();
	}
	

    //The order in this list will be the same as in the editor (I hope... ;_;)

    /*
     * As of writing this, the order is:
     * 0: 5A
     * 1: 5B
     * 2: 5C
     * 3: 2A
     * 4: 2B
     * 5: 2C
     * 6: jA
     * 7: jB
     * 8: jC
     * 9: 623
     * 10: nThrow
     * 11: 214A
     * 12: 214B
     * 13: 214C
     */
    private void initializeHitboxList()
    {
        foreach (Transform child in transform)
        {
            if (child.name.ToLower().Contains("hitbox"))
            {
                hitboxObjects.Add(child.gameObject);
            }
        }
    }

    private void printHitboxList()
    {
        foreach (GameObject hitbox in hitboxObjects)
        {
            Debug.Log(hitbox.name);
        }
    }

    public void enableHitbox(int hitboxID) //See above for chart of hitboxIDs
    {
        if (hitboxObjects[hitboxID].GetComponent<hitboxScript>())
        {
            Debug.Log(hitboxObjects[hitboxID].name + " enabled!");
            hitboxObjects[hitboxID].GetComponent<hitboxScript>().enableHitbox();
        }
    }

    public void disableHitbox(int hitboxID)
    {
        if (hitboxObjects[hitboxID].GetComponent<hitboxScript>())
        {
            Debug.Log(hitboxObjects[hitboxID].name + " disabled!");
            hitboxObjects[hitboxID].GetComponent<hitboxScript>().disableHitbox();
        }
    }

    public void disableAllHitboxes()
    {
        foreach (GameObject hitbox in hitboxObjects)
        {
            if (hitbox.GetComponent<hitboxScript>())
            {
                hitbox.GetComponent<hitboxScript>().disableHitbox();
            }
        }
    }
        
	// Update is called once per frame
	void Update () 
    {
		
	}
}
