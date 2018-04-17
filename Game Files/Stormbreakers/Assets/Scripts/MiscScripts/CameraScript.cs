using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private float maxDistance;
    private float minDistance;
    private float distanceFromBottom;
    private float distanceZ;
    //private float maxZoom;
    //private float minZoom;
    private float zoomLevel; //cam z coord
    private float xL; //left screen x coord
    private float xR; //right screen x coord
    private float screenWidth; //exactly what it sounds like

    private GameObject player1;
    private GameObject player2;

    private Rigidbody2D p1Position;
    private Rigidbody2D p2Position;
    private Vector2 midPoint;

    void calcScreen(Transform p1, Transform p2)
    {
        if (p1.position.x < p2.position.x)
        {
            xL = p1.position.x - 1.5f;
            xR = p2.position.x + 1.5f;
        }
        else
        {
            xL = p2.position.x - 1.5f;
            xR = p1.position.x + 1.5f;
        }
    }


	// Use this for initialization
	void Start () 
    {
        minDistance = 8.5f; //Corresponds to a Z value of ~ -6.5
        //maxDistance = 13.0f; //Corresponds to a Z value of ~ -9.0
        maxDistance = 20.0f;
        distanceFromBottom = 2.0f;
        distanceZ = transform.position.z;
        //minZoom = 9;
        //maxZoom = 13;
        zoomLevel = -9;


        if (GameObject.Find("player1R"))
        {
            player1 = GameObject.Find("player1R");
        }
        else if (GameObject.Find("player1L"))
        {
            player1 = GameObject.Find("player1L");
        }

        p1Position = player1.GetComponent<Rigidbody2D>();

        if (GameObject.Find("player2L"))
        {
            player2 = GameObject.Find("player2L");
        }
        else if (GameObject.Find("player2R"))
        {
            player2 = GameObject.Find("player2R");
        }

        p2Position = player2.GetComponent<Rigidbody2D>();

        calcScreen(p1Position.transform, p2Position.transform);
        screenWidth = xR - xL;

        midPoint = new Vector2(((p1Position.position.x + p2Position.position.x) / 2), ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);
	}

    public void setPlayer1(GameObject go)
    {
        player1 = go;
        p1Position = go.GetComponent<Rigidbody2D>();
    }

    public void setPlayer2(GameObject go)
    {
        player2 = go;
        p2Position = go.GetComponent<Rigidbody2D>();
    }

	
	// Update is called once per frame
	void Update () 
    {
        //midPoint = new Vector2(((p1Position.position.x + p2Position.position.x) / 2), ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);

        //midPoint = new Vector2(midPoint.x, ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);
        midPoint = new Vector2(((p1Position.position.x + p2Position.position.x) / 2), ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);

        calcScreen(p1Position.transform, p2Position.transform);


        //If the distance between p1Position.x and p2Position.x is > minDistance and < maxDistance, modify distanceZ appropriately
        /*if (!(p1Position.position.x - p2Position.position.x > maxDistance || p1Position.position.x - p2Position.position.x < (-1.0f * maxDistance)))
        {
            midPoint = new Vector2(((p1Position.position.x + p2Position.position.x) / 2), ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);
            zoomLevel = -9.0f;
        }
        //zoom if they're farther apart
        else
        {
            midPoint = new Vector2(((p1Position.position.x + p2Position.position.x) / 2), ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);
            zoomLevel = -9 * (xR - xL) / screenWidth;
        }*/
        //else if(p1Position.position.x - p2Position.position.x < minDistance || p1Position.position.x - p2Position.position.x < (-1.0f * minDistance)

        if ((p1Position.position.x - p2Position.position.x < maxDistance && p1Position.position.x - p2Position.position.x > (-1.0f * maxDistance)) &&
            (p1Position.position.x - p2Position.position.x > minDistance || p1Position.position.x - p2Position.position.x < (-1.0f * minDistance)))
        {
            //midPoint = new Vector2(((p1Position.position.x + p2Position.position.x) / 2), ((p1Position.position.y + p2Position.position.y) / 2) + distanceFromBottom);
            zoomLevel = -9.0f * (xR - xL) / screenWidth;
        }

        /*Debug.Log(p1Position.position.x.ToString() + " - " + p2Position.position.x.ToString() + " = " + (p1Position.position.x - p2Position.position.x).ToString());
        Debug.Log(p1Position.position.x.ToString() + " + " + p2Position.position.x.ToString() + " = " + (p1Position.position.x + p2Position.position.x).ToString());*/
        transform.position = new Vector3(midPoint.x, midPoint.y, zoomLevel);

	}
}
