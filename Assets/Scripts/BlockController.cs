using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    public Module[] controlBlock = new Module[4];
    int controlBlockType;
    int currentRotation = 0;
    public static int blockStartPosX = Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2);
    public static int blockStartPosY = BlockArrayManager.unusedTopRowCount;
    float blockChangeTime = 0.4f;       //블럭이 추락한 후 교체에 걸리는 시간

    public struct Module
    {
        public int posX, posY;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //조종중인 블럭을 회전시킨다.
    public void RotateBlock()
    {
        if(controlBlock != null)
        {
            int outOfBorderX = 0;           //블럭이 경계 밖으로 삐져나간 정도
            int rotation = currentRotation + 1;
            if (rotation >= BlockRotation.rotation)
                rotation = 0;

            for (int i = 1; i < controlBlock.Length; i++)
            {
                if (GetComponent<BlockArrayManager>().GetModuleContent(
                    controlBlock[i].posX + BlockRotation.blockMove[controlBlockType, rotation, i - 1, 0],
                    controlBlock[i].posY + BlockRotation.blockMove[controlBlockType, rotation, i - 1, 1]) == (int)BlockArrayManager.Content.Block)
                {
                    //print("다른 블럭에 걸림");
                    return;
                }
                else if (GetComponent<BlockArrayManager>().GetModuleContent(
                    controlBlock[i].posX + BlockRotation.blockMove[controlBlockType, rotation, i - 1, 0],
                    controlBlock[i].posY + BlockRotation.blockMove[controlBlockType, rotation, i - 1, 1]) == (int)BlockArrayManager.Content.Character)
                {
                    //print("캐릭터에 걸림");
                    return;
                }

                //블럭이 얼마나 밖으로 삐져나갔는지 체크
                if (controlBlock[i].posX > BlockArrayManager.RowCount && controlBlock[i].posX - BlockArrayManager.RowCount > outOfBorderX)
                {
                    outOfBorderX = controlBlock[i].posX - BlockArrayManager.RowCount;
                }
                else if (controlBlock[i].posX < 0 && -controlBlock[i].posX < outOfBorderX)
                {
                    outOfBorderX = -controlBlock[i].posX;
                }
            }

            //삐져나간 만큼 이동시켰을 때 다른 모듈과 겹치는지 확인
            for (int i = 1; i < controlBlock.Length; i++)
            {
                if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX - outOfBorderX, controlBlock[i].posY) == (int)BlockArrayManager.Content.Block)
                    return;
                else if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX - outOfBorderX, controlBlock[i].posY) == (int)BlockArrayManager.Content.Character)
                    return;
                //print("다른 모듈과 안겹침");
            }

            for (int i = 0; i < controlBlock.Length; i++)
            {
                if (i == 0)
                {
                    controlBlock[i].posX -= outOfBorderX;
                    print(controlBlock[i].posX + ", " + controlBlock[i].posY);
                    print(GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY));
                }
                else
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
                    controlBlock[i].posX = controlBlock[0].posX + BlockRotation.blockMove[controlBlockType, rotation, i - 1, 0] - outOfBorderX;
                    controlBlock[i].posY = controlBlock[0].posY + BlockRotation.blockMove[controlBlockType, rotation, i - 1, 1];
                    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
                    print(controlBlock[i].posX + ", " + controlBlock[i].posY);
                    print(GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY));
                }
            }
            currentRotation = rotation;

            //블럭 오브젝트 위치 업데이트 필요
            GameObject.Find("Main Camera").GetComponent<DisplayController>().UpdateRotation(controlBlock);
            GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().ShowContent();
        }
    }

    //블럭이 시간에 따라 자동으로 내려갈 때 사용하는 메서드
    public void FallBlock()
    {
        if(controlBlock != null)
        {
            for (int i = 0; i < controlBlock.Length; i++)
            {
                if (controlBlock[i].posY + 1 >= BlockArrayManager.RowCount)
                {
                    //블럭이 가장 밑바닥으로 빠질 때
                    for (int k = 0; k < controlBlock.Length; k++)
                    {
                        GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[k].posX, controlBlock[k].posY, (int)BlockArrayManager.Content.Empty);
                    }
                    GameObject.Find("Main Camera").GetComponent<DisplayController>().DestroyControlBlockObject();
                    controlBlock = null;
                    Invoke("ChangeControlBlock", blockChangeTime);
                    GetComponent<BlockArrayManager>().ShowContent();
                    return;
                }
                else if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY + 1) == (int)BlockArrayManager.Content.Block)
                {
                    //블럭이 다른 모듈에 막혀서 내려갈 수 없을 때
                    for (int k = 0; k < controlBlock.Length; k++)
                    {
                        GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[k].posX, controlBlock[k].posY, (int)BlockArrayManager.Content.Block);
                    }
                    controlBlock = null;
                    Invoke("ChangeControlBlock", blockChangeTime);
                    //Invoke("ChangeControlBlock()", EventManager.fallPeriod); //블럭 낙하주기만큼 기다린 후에 다음 블럭을 생성한다.
                    return;
                }
                else if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY + 1) == (int)BlockArrayManager.Content.Character)
                {
                    //캐릭터가 떨어지는 블럭에 맞아 게임오버
                    GameObject.Find("Main Camera").GetComponent<EventManager>().GameOver();
                    return;
                }
            }

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
    }

    //블럭을 빠른 낙하시킬 때 사용하는 메서드
    //FallBlock이랑 합쳐도 될듯? 인수 하나 첨가해서 isFast 뭐 이런거
    public void FastFallBlock()
    {
        int moveDistance = GetComponent<BlockArrayManager>().GetCollisionDistance(controlBlock, true);
        
        //controlBlock이 추락해서 제거되었을 때
        if(moveDistance == int.MaxValue)
        {
            for (int i = 0; i < controlBlock.Length; i++)
            {
                GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
            }
            GameObject.Find("Main Camera").GetComponent<DisplayController>().DestroyControlBlockObject();
            controlBlock = null;
            Invoke("ChangeControlBlock", blockChangeTime);
            GetComponent<BlockArrayManager>().ShowContent();
            return;
        }
        else if(moveDistance == -1)
        {
            GameObject.Find("Main Camera").GetComponent<EventManager>().GameOver();
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
            GetComponent<BlockArrayManager>().CheckRow(controlBlock[i].posY);
        }
        
        //블럭 오브젝트를 이동시킴
        GameObject.Find("Main Camera").GetComponent<DisplayController>().MoveBlock(0, moveDistance);

        GameObject.Find("Main Camera").GetComponent<EventManager>().ResetFallColltime();
        controlBlock = null;
        Invoke("ChangeControlBlock", blockChangeTime);
        //Invoke("ChangeControlBlock()", EventManager.fallPeriod); //블럭 낙하주기만큼 기다린 후에 다음 블럭을 생성한다.
        GetComponent<BlockArrayManager>().ShowContent();
    }

    //블럭을 새로 생성하여 조종 가능하도록 바꿈
    public void ChangeControlBlock()
    {
        Debug.Log("블럭 교체");

        controlBlockType = Random.Range(0, 7);
        controlBlock = new Module[4];
        controlBlock[0].posX = blockStartPosX;
        controlBlock[0].posY = blockStartPosY;

        for (int i = 0; i < controlBlock.Length; i++)
        {
            if (GetComponent<BlockArrayManager>().GetModuleContent(controlBlock[i].posX, controlBlock[i].posY) == (int)BlockArrayManager.Content.Block)
            {
                GameObject.Find("Main Camera").GetComponent<EventManager>().GameOver();
            }
        }

        GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[0].posX, controlBlock[0].posY, (int)BlockArrayManager.Content.ControlBlock);
        for (int i = 1; i < controlBlock.Length; i++)
        {
            //print(BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0] + "," + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1]);
            controlBlock[i].posX = controlBlock[0].posX + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0];
            controlBlock[i].posY = controlBlock[0].posY + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1];
            GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        }

        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewBlock(controlBlock);
        GameObject.Find("Canvas").GetComponent<UIManager>().isAllowFall = true;

        GameObject.Find("Main Camera").GetComponent<DisplayController>().ResetPreview();
        GameObject.Find("Main Camera").GetComponent<DisplayController>().BlockPreview();
    }

    //블럭을 좌우로 이동시키는 메서드
    public void BlockHorzMove(int direction)
    {
        if(controlBlock != null)
        {
            for (int i = 0; i < controlBlock.Length; i++)
            {
                if (controlBlock[i].posX + direction < 0 || controlBlock[i].posX + direction >= BlockArrayManager.ColumnCount)
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
    }

    //블럭 등반 때 컨트롤블럭 모듈 내의 position을 변경해주는 메서드
    public void DownControlBlockPosition(int directionVert)
    {
        //for (int i = 0; i < controlBlock.Length; i++)
        //{
        //    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.Empty);
        //}
        //for (int i = 0; i < controlBlock.Length; i++)
        //{
        //    controlBlock[i].posY += directionVert;
        //    GetComponent<BlockArrayManager>().SetModuleContent(controlBlock[i].posX, controlBlock[i].posY, (int)BlockArrayManager.Content.ControlBlock);
        //}
        if(controlBlock != null)
        {
            for (int i = 0; i < controlBlock.Length; i++)
            {
                controlBlock[i].posY += 1;
            }
        }
    }
}
