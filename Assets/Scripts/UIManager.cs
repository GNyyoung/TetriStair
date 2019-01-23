using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickMoveButton(int direction)
    {
        GetComponent<CharacterAction>().Move(direction);
    }

    public void OnClickRotateButton()
    {
        GetComponent<BlockController>().RotateBlock();
    }
}
