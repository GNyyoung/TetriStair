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
        public bool isStandardPoint;
        //isStandardPoint가 true인걸 controlBlock의 0번 인덱스에 넣자 그러면 굳이 이거 필요없을듯?
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
            if(fixedBlock[i].isStandardPoint == false)
            {
                //새로 위치할 좌표가 비어있어야만 인덱스 수정
                if(GetComponent<BlockArrayManager>().GetElementContent(
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
        }

        //삐져나간 만큼 이동시켰을 때 다른 모듈과 겹치는지 확인
        for(int i = 1; i < fixedBlock.Length; i++)
        {
            if (GetComponent<BlockArrayManager>().GetElementContent(fixedBlock[i].posX - outOfBorderX, fixedBlock[i].posY) != 0)
                return;
            else
                fixedBlock[i].posX -= outOfBorderX;
        }

        for(int i = 0; i < fixedBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
            GetComponent<BlockArrayManager>().SetModuleContent(fixedBlock[i].posX, fixedBlock[i].posY, (int)BlockArrayManager.Content.Block);
        }
        controlBlock = fixedBlock;
        //BlockArrayManager의 gameArray를 업데이트하는 메서드 추가할것
    }

    //블럭이 시간에 따라 자동으로 내려갈 때 사용하는 메서드
    public void FallBlock()
    {
        for(int i = 0; i < controlBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }
        for (int i = 0; i < controlBlock.Length; i++)
        {
            int belowElementContent = GetComponent<BlockArrayManager>().GetElementContent(controlBlock[i].posX, controlBlock[i].posY + 1);
            if (belowElementContent != 0)
            {
                for (int k = 0; k < controlBlock.Length; k++)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[k].posX, controlBlock[k].posY, (int)BlockArrayManager.Content.Block);
                }
                Invoke("ChangeControlBlock", 0.5f);
                //Invoke("ChangeControlBlock()", EventManager.fallPeriod); //블럭 낙하주기만큼 기다린 후에 다음 블럭을 생성한다.
                return;
            }
        }
        //controlBlock을 1만큼 이동시키는 메서드 실행
        for (int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posY += 1;
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Block);
        }

        //블럭 오브젝트를 이동시킴
        GameObject.Find("Main Camera").GetComponent<DisplayController>().MoveBlock(0, 1);
        GetComponent<BlockArrayManager>().ShowContent();

    }

    //블럭을 빠른 낙하시킬 때 사용하는 메서드
    //FallBlock이랑 합쳐도 될듯? 인수 하나 첨가해서 isFast 뭐 이런거
    public void FastFallBlock()
    {
        int moveDistance = int.MaxValue;

        for (int i = 0; i < controlBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }

        //for(int i = 0; i < controlBlock.Length; i++)
        //{
        //    int distance = GetComponent<BlockArrayManager>().GetCollisionDistance(controlBlock[i].posX, controlBlock[i].posY);
        //    int distance = GetComponent<BlockArrayManager>().GetCollisionDistance2(controlBlock);
        //    if (distance < moveDistance) moveDistance = distance;
        //}
        moveDistance = GetComponent<BlockArrayManager>().GetCollisionDistance2(controlBlock);
        if(moveDistance == -1)
        {
            return;
        }

        //controlBlock을 moveDistance만큼 이동시키는 메서드 실행
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

    public void ChangeControlBlock()
    {
        controlBlockType = Random.Range(0, 6);
        controlBlock = new Module[4];
        controlBlock[0].posX = blockStartPosX;
        controlBlock[0].posY = blockStartPosY;

        for(int i = 1; i < controlBlock.Length; i++)
        {
            //print(BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0] + "," + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1]);
            controlBlock[i].posX = controlBlock[0].posX + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0];
            controlBlock[i].posY = controlBlock[0].posY + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1];
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Block);
        }

        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewBlock(controlBlock);
    }

    //블럭을 좌우로 이동시키는 메서드
    public void BlockHorzMove(int direction)
    {
        for(int i = 0; i < controlBlock.Length; i++)
        {
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        }
        for (int i = 0; i < controlBlock.Length; i++)
        {
            if(controlBlock[i].posX + direction < 0 || controlBlock[i].posX + direction >= BlockArrayManager.ColumnCount)
            {
                print("보드 밖으로 벗어남");
                for (int k = 0; k < controlBlock.Length; k++)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[k].posX, controlBlock[k].posY, (int)BlockArrayManager.Content.Block);
                }
                return;
            }
            //이동하려는 방향에 다른 모듈이나 캐릭터가 있을 때
            if (GetComponent<BlockArrayManager>().GetElementContent(controlBlock[i].posX + direction, controlBlock[i].posY) != 0)
            {
                print("다른 블럭 또는 캐릭터에 걸림");
                for (int k = 0; k < controlBlock.Length; k++)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[k].posX, controlBlock[k].posY, (int)BlockArrayManager.Content.Block);
                }
                return;
            }
        }
        //controlBlock을 1만큼 이동시키는 메서드 실행
        for (int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posX += direction;
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Block);
        }
        GameObject.Find("Main Camera").GetComponent<DisplayController>().MoveBlock(direction, 0);
    }
}
