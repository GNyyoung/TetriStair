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

    //캐릭터 좌우 이동
    public void OnClickCharacterMove(int direction)
    {
        GameObject.Find("Character").GetComponent<CharacterAction>().Move(direction);
    }

    //블럭 회전
    public void OnClickRotate()
    {
        GameObject.Find("BackgroundPanel").GetComponent<BlockController>().RotateBlock();
    }

    //블럭 좌우 이동
    public void OnClickBlockMove(int direction)
    {
        GameObject.Find("BackgroundPanel").GetComponent<BlockController>().BlockHorzMove(direction);
    }

    public void OnClickBlockFall()
    {
        GameObject.Find("BackgroundPanel").GetComponent<BlockController>().FastFallBlock();
    }
}
