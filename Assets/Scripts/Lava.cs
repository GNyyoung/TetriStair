using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

    int lavaHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateLavaHeight()
    {
        GetComponent<RectTransform>().localPosition += Vector3.up * BlockArrayManager.ModuleDistance;
    }

    public void CheckFallLava()
    {
        if(GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().localPosition.y - GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().sizeDelta.y / 2 + 3 * BlockArrayManager.ModuleDistance
            < GetComponent<RectTransform>().localPosition.y + GetComponent<RectTransform>().sizeDelta.y / 2)
        {
            GameObject.Find("Main Camera").GetComponent<EventManager>().GameOver();
        }
    }
}
