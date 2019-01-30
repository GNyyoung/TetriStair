using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    public Module[] controlBlock = new Module[4];
    int controlBlockType;
    int currentRotation = 0;
    public static int blockStartPosX = Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2);
    public static int blockStartPosY = 3;

    public struct Module
    {
        public int posX, posY;
        public bool isControlBlock;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //조종중인 블럭을 회전시킨다.
    //수정 필요!!!
    //1. controlblock을 gameArray에서 지운다.
    //2. 모듈 하나씩 rotation대로 위치시켜서 다른 모듈이나 캐릭터랑 충돌하는게 있는지 확인
    //      충돌하면 처음 상태로 돌리고 return한다.
    //3. 바뀐 위치대로 gameArray에 집어넣고 displayController에서 오브젝트 위치 변경
    //      관련 메서드가 없어서 새로 짜야함. 회전할 때 사용한 blockRotation.blockMove 정보 필요
    public void RotateBlock()
    {
        Module[] fixedBlock = controlBlock;
        int standardPosX = fixedBlock[0].posX;
        int standardPosY = fixedBlock[0].posY;

        int outOfBorderX = 0;           //블럭이 경계 밖으로 삐져나간 정도

        if (currentRotation + 1 > 3)
            currentRotation = 0;
        else
            currentRotation += 1;

        for (int i = 1; i < fixedBlock.Length; i++)     //기준좌표는 바뀌면 안되므로 빼고 회전시킨다.
        {
            //새로 위치할 좌표가 비어있어야만 인덱스 수정
            if (GetComponent<BlockArrayManager>().GetModuleContent(
                fixedBlock[i].posX + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0],
                fixedBlock[i].posY + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1]) != 0)
            {
                fixedBlock[i].posX += BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0];
                fixedBlock[i].posY += BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1];

                //블럭이 얼마나 밖으로 삐져나갔는지 체크
                if (fixedBlock[i].posX > BlockArrayManager.RowCount && fixedBlock[i].posX - BlockArrayManager.RowCount > outOfBorderX)
                    outOfBorderX = fixedBlock[i].posX - BlockArrayManager.RowCount;
                else if (fixedBlock[i].posX < 0 && fixedBlock[i].posX < outOfBorderX)
                    outOfBorderX = fixedBlock[i].posX;
            }
        }

        //삐져나간 만큼 이동시켰을 때 다른 모듈과 겹치는지 확인
        for(int i = 1; i < fixedBlock.Length; i++)
        {
            if (GetComponent<BlockArrayManager>().GetModuleContent(fixedBlock[i].posX - outOfBorderX, fixedBlock[i].posY) != 0)
                return;
            else
                fixedBlock[i].posX -= outOfBorderX;
        }

        for(int i = 0; i < fixedBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
            GetComponent<BlockArrayManager>().SetModuleContent(fixedBlock[i].posX, fixedBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        }
        controlBlock = fixedBlock;
        //BlockArrayManager의 gameArray를 업데이트하는 메서드 추가할것
    }

    //블럭이 시간에 따라 자동으로 내려갈 때 사용하는 메서드
    public void FallBlock()
    {
        for (int i = 0; i < controlBlock.Length; i++)
        {
            if(controlBlock[i].posY + 1 >= BlockArrayManager.RowCount)
            {
                //블럭이 가장 밑바닥으로 빠질 때
                for (int k = 0; k < controlBlock.Length; k++)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[k].posX, controlBlock[k].posY, (int)BlockArrayManager.Content.Empty);
                }
                GameObject.Find("Main Camera").GetComponent<DisplayController>().DestroyControlBlockObject();
                Invoke("ChangeControlBlock", 0.5f);
                GetComponent<BlockArrayManager>().ShowContent();
                return;
            }
            else if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY + 1) == (int)BlockArrayManager.Content.Block)
            {
                //블럭이 다른 모듈에 막혀서 내려갈 수 없을 때
                Invoke("ChangeControlBlock", 0.5f);
                //Invoke("ChangeControlBlock()", EventManager.fallPeriod); //블럭 낙하주기만큼 기다린 후에 다음 블럭을 생성한다.
                return;
            }
            else if(GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY + 1) == (int)BlockArrayManager.Content.Character)
            {
                //캐릭터가 떨어지는 블럭에 맞아 게임오버
                //블럭이 다른 모듈에 막혀서 내려갈 수 없을 때
                Time.timeScale = 0;
                return;
            }
        }
        //controlBlock을 1만큼 이동시키는 메서드 실행
        for (int i = 0; i < controlBlock.Length; i++)
        {
            //print("모듈 제거" + i);
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }
        for (int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posY += 1;
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        }

        //블럭 오브젝트를 이동시킴
        GameObject.Find("Main Camera").GetComponent<DisplayController>().MoveBlock(0, 1);
        GetComponent<BlockArrayManager>().ShowContent();

    }

    //블럭을 빠른 낙하시킬 때 사용하는 메서드
    //FallBlock이랑 합쳐도 될듯? 인수 하나 첨가해서 isFast 뭐 이런거
    public void FastFallBlock()
    {
        int moveDistance = GetComponent<BlockArrayManager>().GetCollisionDistance(controlBlock);
        
        //controlBlock이 추락해서 제거되었을 때
        if(moveDistance == int.MaxValue)
        {
            //controlBlock이 추락해서 제거되었을 때
            for (int i = 0; i < controlBlock.Length; i++)
            {
                GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
            }
            GameObject.Find("Main Camera").GetComponent<DisplayController>().DestroyControlBlockObject();
            Invoke("ChangeControlBlock", 0.5f);
            GetComponent<BlockArrayManager>().ShowContent();
            return;
        }
        else if(moveDistance == -1)
        {
            //게임오버될때
            return;
        }

        //controlBlock을 moveDistance만큼 이동시킴
        for (int i = 0; i < controlBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }
        for (int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posY += moveDistance;
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Block);
        }
        
        //블럭 오브젝트를 이동시킴
        GameObject.Find("Main Camera").GetComponent<DisplayController>().MoveBlock(0, moveDistance);

        GameObject.Find("Main Camera").GetComponent<EventManager>().ResetFallColltime();
        Invoke("ChangeControlBlock", 0.5f);
        //Invoke("ChangeControlBlock()", EventManager.fallPeriod); //블럭 낙하주기만큼 기다린 후에 다음 블럭을 생성한다.
        GetComponent<BlockArrayManager>().ShowContent();

    }

    //블럭을 새로 생성하여 조종 가능하도록 바꿈
    public void ChangeControlBlock()
    {
        Debug.Log("블럭 교체");
        controlBlockType = Random.Range(0, 6);
        controlBlock = new Module[4];
        controlBlock[0].posX = blockStartPosX;
        controlBlock[0].posY = blockStartPosY;
        GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[0].posX, controlBlock[0].posY, (int)BlockArrayManager.Content.ControlBlock);

        for (int i = 1; i < controlBlock.Length; i++)
        {
            //print(BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0] + "," + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1]);
            controlBlock[i].posX = controlBlock[0].posX + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0];
            controlBlock[i].posY = controlBlock[0].posY + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1];
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        }

        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewBlock(controlBlock);
    }

    //블럭을 좌우로 이동시키는 메서드
    public void BlockHorzMove(int direction)
    {
        for (int i = 0; i < controlBlock.Length; i++)
        {
            if(controlBlock[i].posX + direction < 0 || controlBlock[i].posX + direction >= BlockArrayManager.ColumnCount)
            {
                //이동하면 블럭의 일부가 배열 밖으로 나가는 경우
                return;
            }
            else if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX + direction, controlBlock[i].posY) == (int)BlockArrayManager.Content.Block ||
                GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX + direction, controlBlock[i].posY) == (int)BlockArrayManager.Content.Character)
            {
                //이동하려는 방향에 다른 모듈이나 캐릭터가 있을 때
                return;
            }
        }
        //controlBlock을 1만큼 이동시킴
        for (int i = 0; i < controlBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }
        for (int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posX += direction;
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        }
        GameObject.Find("Main Camera").GetComponent<DisplayController>().MoveBlock(direction, 0);
        GetComponent<BlockArrayManager>().ShowContent();
    }

    public void DownControlBlockPosition(int directionVert)
    {
        for (int i = 0; i < controlBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }
        for (int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posY += directionVert;
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        }
    }
}
