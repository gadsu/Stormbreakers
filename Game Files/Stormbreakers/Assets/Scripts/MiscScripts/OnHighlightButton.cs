using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OnHighlightButton : MonoBehaviour {

    Image img;

	// Use this for initialization
	void Start () {
        img = GameObject.Find("Selected Stage").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(BaseEventData data)
    {
        img.sprite = gameObject.GetComponent<Image>().sprite;
        Debug.Log(gameObject + "Selected");
    }
}
