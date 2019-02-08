using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    GameObject character;
    public bool isAllowFall = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        KeyBoardControl();
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

    //블럭 빠른추락
    public void OnClickBlockFall()
    {
        if(isAllowFall == true)
        {
            isAllowFall = false;
            GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FastFallBlock();
        }
    }

    public void SetCharacter(GameObject character)
    {
        this.character = character;
    }

    public void KeyBoardControl()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            OnClickRotate();
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            OnClickBlockFall();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            OnClickBlockMove(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            OnClickBlockMove(1);
        else if (Input.GetKeyDown(KeyCode.A))
            OnClickCharacterMove(-1);
        else if (Input.GetKeyDown(KeyCode.D))
            OnClickCharacterMove(1);
    }
}
