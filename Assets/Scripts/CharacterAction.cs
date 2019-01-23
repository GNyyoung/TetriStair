using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour {

    private int posX, posY; //기준좌표는 캐릭터 하체부분. 캐릭터는 element 두개를 차지한다.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //blockArrayManager에 있는 캐릭터 좌표를 이동 가능한 경우에 이동시킴.
    public void Move(int direction)
    {
        int platformPosY;

        if (GetComponent<BlockArrayManager>().GetElementContent(posX + direction, posY - 1) == 0)
        {
            if (GetComponent<BlockArrayManager>().GetElementContent(posX + direction, posY) == 1)
            {
                platformPosY = posY;
            }
            else if (GetComponent<BlockArrayManager>().GetElementContent(posX + direction, posY + 1) == 1)
            {
                platformPosY = posY + 1;
            }
            else if (GetComponent<BlockArrayManager>().GetElementContent(posX + direction, posY + 2) == 1)
            {
                platformPosY = posY + 2;
            }
            else
                return;
        }
        else
            return;

        CharacterMove(direction, platformPosY);
        //캐릭터 오브젝트가 이동하는 애니메이션 필요
    }

    //씬에 있는 캐릭터 오브젝트를 이동시킴
    public void CharacterMove(int direction, int platformPosY)
    {
        transform.position = new Vector3(transform.position.x + direction * BlockArrayManager.ElementsDistance, ((platformPosY - 1) + 0.5f) * BlockArrayManager.ElementsDistance);

        GetComponent<BlockArrayManager>().SetElementContent(posX, posY, (int)BlockArrayManager.Content.Empty);
        GetComponent<BlockArrayManager>().SetElementContent(posX, posY - 1, (int)BlockArrayManager.Content.Empty);

        GetComponent<BlockArrayManager>().SetElementContent(posX + direction, platformPosY - 1, (int)BlockArrayManager.Content.Block);
        GetComponent<BlockArrayManager>().SetElementContent(posY + direction, platformPosY - 2, (int)BlockArrayManager.Content.Block);
    }
    
}
