﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrayManager : MonoBehaviour {

    public enum Content { Empty, Block, Character}

    public const int ColumnCount = 10;
    public const int RowCount = 17;
    public const int unusedTopRowCount = 3;
    public const int ModuleDistance = 100;            //두 좌표 사이의 간격
    private int[,] gameArray = new int[ColumnCount, RowCount];   //맨 위 3칸은 안보이게 한다. 

    

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //한 행이 블럭으로 찼는지 확인한다.
    //true가 나올 경우 보너스 발동하는 메서드에 연결
    public bool CheckRow(int[] rowIndex)
    {
        bool isComplete = true;         //한줄이 되면 true
        for(int i = 0; i < rowIndex.Length; i++)
        {
            for(int k = 0; k < gameArray.GetLength(1); k++)
            {
                if (gameArray[i, k] != 1)
                    isComplete = false;
            }
            if (isComplete == true)
                return true;
        }
        return false;
    }

    public int[,] GetGameArray()
    {
        return gameArray;
    }

    //y축으로 다른 모듈에 닿기 위해 필요한 거리 계산
    public int GetCollisionDistance(int indexX, int indexY)
    {
        int distance = 0;

        while(indexY < gameArray.GetLength(1))
        {
            if (gameArray[indexX, indexY] == 0)
            {
                distance += 1;
            }
            else if (gameArray[indexX, indexY] == 2)
            {
                //게임오버 메서드 실행
            }
            indexY += 1;
        }

        return distance;
    }

    public int GetElementContent(int posX, int posY)
    {
        return gameArray[posX, posY];
    }

    public void SetModuleContent(int posX, int posY, int content)
    {
        gameArray[posX, posY] = content;
    }

    //캐릭터가 최고높이 갱신할 때 보드를 업데이트함.
    //모든 모듈의 위치가 1칸 내려감(array의 y이 1 커짐)
    public void UpdateBoardAtClimb(int posX, int posY, int directionHorz)
    {
        SetModuleContent(posX, posY, (int)Content.Empty);
        SetModuleContent(posX, posY - 1, (int)Content.Empty);
        for (int row = RowCount - 2; row >= unusedTopRowCount; row--)
        {
            for (int col = ColumnCount; col >= 0; col--)
            {
                gameArray[col, row + 1] = gameArray[col, row];
            }
        }
        SetModuleContent(posX + directionHorz, posY, (int)Content.Character);
        SetModuleContent(posY + directionHorz, posY, (int)Content.Character);

        //모듈들 위치 갱신
        GameObject.Find("Main Camera").GetComponent<DisplayController>().DownAllModule();
    }

    //캐릭터가 최고높이로 안올라가고 아래위, 또는 옆으로 왔다갔다 할때.
    //캐릭터 위치만 변함.
    public void CharacterMove(int posX, int posY, int directionHorz, int directionVert)
    {
        SetModuleContent(posX, posY, (int)Content.Empty);
        SetModuleContent(posX, posY - 1, (int)Content.Empty);
        
        SetModuleContent(posX + directionHorz, posY + directionVert, (int)Content.Character);
        SetModuleContent(posX + directionHorz, posY +  - 1 + directionVert, (int)Content.Character);
    }
    
}
