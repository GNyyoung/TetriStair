using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    public Module[] controlBlock = new Module[4];
    int controlBlockType;
    int currentRotation = 0;
    int blockStartPosX = Mathf.CeilToInt(BlockArrayManager.RowCount / 2);
    int blockStartPosY = 3;

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
    public void RotateBlock(int direction)
    {
        Module[] fixedBlock = controlBlock;
        int standardPosX = fixedBlock[0].posX;
        int standardPosY = fixedBlock[0].posY;

        int outOfBorderX = 0;           //블럭이 경계 밖으로 삐져나간 정도

        if (currentRotation + direction > 3)
            currentRotation = 0;
        else if (currentRotation + direction < 0)
            currentRotation = 3;
        else
            currentRotation += direction;

        for (int i = 1; i < fixedBlock.Length; i++)
        {
            //기준좌표는 바뀌면 안되므로 빼고 회전시킨다.
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

        controlBlock = fixedBlock;
        //BlockArrayManager의 gameArray를 업데이트하는 메서드 추가할것
    }

    //블럭이 시간에 따라 자동으로 내려갈 때 사용하는 메서드
    public void FallBlock()
    {
        for (int i = 0; i < controlBlock.Length; i++)
        {
            int belowElementContent = GetComponent<BlockArrayManager>().GetElementContent(controlBlock[i].posX, controlBlock[i].posY + 1);
            if (belowElementContent != 0)
            {
                ChangeControlBlock();
                return;
            }//리턴 앞에 controlBlock을 바꾸는 메서드가 실행되어야함
        }
        //controlBlock을 1만큼 이동시키는 메서드 실행
    }

    //블럭을 빠른 낙하시킬 때 사용하는 메서드
    //FallBlock이랑 합쳐도 될듯? 인수 하나 첨가해서 isFast 뭐 이런거
    public void FastFallBlock()
    {
        int moveDistance = int.MaxValue;
        for(int i = 0; i < controlBlock.Length; i++)
        {
            int distance = GetComponent<BlockArrayManager>().GetCollisionDistance(controlBlock[i].posX, controlBlock[i].posY);
            if (distance < moveDistance) moveDistance = distance;
        }

        //controlBlock을 moveDistance만큼 이동시키는 메서드 실행
        //controlBlock을 바꾸는 메서드 실행
    }

    public void ChangeControlBlock()
    {
        controlBlockType = Random.Range(0, 7);
        controlBlock = new Module[4];
        controlBlock[0].posX = blockStartPosX;
        controlBlock[0].posY = blockStartPosY;

        for(int i = 0; i < controlBlock.Length; i++)
        {
            controlBlock[i].posX = controlBlock[0].posX + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 0];
            controlBlock[i].posY = controlBlock[0].posY + BlockRotation.blockMove[controlBlockType, currentRotation, i - 1, 1];
        }

        //BlockArrayManager의 controlBlock을 갱신하는 메서드 추가할것
    }
}
