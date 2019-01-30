using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    GameObject character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //캐릭터 좌우 이동
    public void OnClickCharacterMove(int direction)
    {
        character.GetComponent<CharacterAction>().CharacterMove(direction);
    }

    //블럭 회전
    public void OnClickRotate()
    {
        GameObject.Find("GameBoardPanel").GetComponent<BlockController>().RotateBlock();
    }

    //블럭 좌우 이동
    public void OnClickBlockMove(int direction)
    {
        GameObject.Find("GameBoardPanel").GetComponent<BlockController>().BlockHorzMove(direction);
    }

    public void OnClickBlockFall()
    {
        GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FastFallBlock();
    }

    public void SetCharacter(GameObject character)
    {
        this.character = character;
    }
}
