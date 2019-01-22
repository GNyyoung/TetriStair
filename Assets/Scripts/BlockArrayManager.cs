using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrayManager : MonoBehaviour {

    public enum Arr { Empty, Block, Character}

    public const int RowCount = 10;
    private int[,] gameArray = new int[RowCount, 17]; //맨 위 3칸은 안보이게 한다.

    

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
        bool isComplete = true;
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

    public int GetElementContent(int indexX, int indexY)
    {
        return gameArray[indexX, indexY];
    }
}
