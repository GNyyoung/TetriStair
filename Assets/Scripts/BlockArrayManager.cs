using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//디버그용으로 UI 넣어놨으니 나중에 제거하자.
public class BlockArrayManager : MonoBehaviour {

    public enum Content { Empty, Block, Character, ControlBlock}

    public const int ColumnCount = 10;
    public const int RowCount = 22;
    public const int unusedTopRowCount = 7;
    public const int unusedBotRowCount = 1;
    public const int ModuleDistance = 100;            //두 좌표 사이의 간격
    private int[,] gameArray = new int[ColumnCount, RowCount];   //맨 위 3칸은 안보이게 한다. 

    public GameObject pointObject;
    List<GameObject> pointList = new List<GameObject>();
    public GameObject gameBoardPosition;
    

	// Use this for initialization
	void Start () {
        gameBoardPosition.GetComponent<RectTransform>().localPosition = GameObject.Find("GameBoard").GetComponent<RectTransform>().localPosition;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //한 행이 블럭으로 찼는지 확인한다.
    //true가 나올 경우 보너스 발동하는 메서드에 연결
    public void CheckRow(int posY)
    {
        bool isComplete = true;         //한줄이 되면 true
        for(int k = 0; k < ColumnCount; k++)
        {
            if (gameArray[k, posY] != (int)Content.Block)
                isComplete = false;
        }
        if (isComplete == true)
        {
            GameObject.Find("Main Camera").GetComponent<EventManager>().SetSinkStopTime();
            print(5.0f + "초 간 용암 정지");
        }
    }

    public int[,] GetGameArray()
    {
        return gameArray;
    }
    

    //블럭이 내려가면서 다른 모듈에 닿기 위해 필요한 거리 계산
    public int GetCollisionDistance(BlockController.Module[] controlBlock, bool checkCollide)
    {
        int moveDistance = int.MaxValue;
        int distance = 0;
        bool isLoop = true;
        bool isCollideCharacter = false;
        int downCount = 1;

        for (int i = 0; i < controlBlock.Length; i++)
        {
            distance = 0;
            downCount = 1;
            isLoop = true;
            while(isLoop == true)
            {
                if(controlBlock[i].posY + downCount >= RowCount)
                {
                    distance = int.MaxValue;
                    isLoop = false;
                }
                else
                {
                    switch (gameArray[controlBlock[i].posX, controlBlock[i].posY + downCount])
                    {
                        case (int)BlockArrayManager.Content.Empty:
                            distance += 1;
                            break;
                        case (int)BlockArrayManager.Content.Block:
                            if (distance <= moveDistance)
                            {
                                isCollideCharacter = false;
                                moveDistance = distance;
                            }
                            isLoop = false;
                            break;
                        case (int)BlockArrayManager.Content.Character:
                            if (distance < moveDistance)
                            {
                                moveDistance = distance;
                                isCollideCharacter = true;
                            }
                            isLoop = false;
                            break;
                        case (int)BlockArrayManager.Content.ControlBlock:
                            distance += 1;
                            break;
                        default:
                            Debug.LogError("잘못된 값");
                            return -1;
                    }
                    downCount += 1;
                }
            }
        }

        if(isCollideCharacter == true && checkCollide == true)
        {
            print("게임오버");
            Time.timeScale = 0;
            //게임오버 메서드 출력
            return -1;
        }
        else
            return moveDistance;
    }

    public int GetModuleContent(int posX, int posY)
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
        for (int row = RowCount - 2; row >= 0; row--)
        {
            for (int col = ColumnCount - 1; col >= 0; col--)
            {
                gameArray[col, row + 1] = gameArray[col, row];
            }
        }
        SetModuleContent(posX + directionHorz, posY, (int)Content.Character);
        SetModuleContent(posX + directionHorz, posY - 1, (int)Content.Character);

        GetComponent<BlockController>().DownControlBlockPosition(1);

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
        SetModuleContent(posX + directionHorz, posY - 1 + directionVert, (int)Content.Character);
    }

    //gameArray 내에 저장된 block들을 보여주는 테스트용 메서드
    public void ShowContent()
    {
        //return;
        while(pointList.Count > 0)
        {
            Destroy(pointList[0]);
            pointList.RemoveAt(0);
        }

        for (int i = 0; i < RowCount; i++)
        {
            for (int k = 0; k < ColumnCount; k++)
            {
                if (gameArray[k, i] != 0)
                {
                    GameObject point = Instantiate(pointObject, GameObject.Find("GameBoardPanel").transform);
                    point.GetComponent<RectTransform>().localPosition = gameBoardPosition.GetComponent<RectTransform>().localPosition + new Vector3(BlockArrayManager.ModuleDistance * (k + 0.5f), -ModuleDistance * (i + 0.5f - unusedTopRowCount));
                    if (gameArray[k, i] == (int)Content.Block)
                        point.GetComponent<Image>().color = Color.red;
                    else if(gameArray[k, i] == (int)Content.ControlBlock)
                        point.GetComponent<Image>().color = Color.green;
                    else if (gameArray[k, i] == (int)Content.Character)
                    {
                        point.GetComponent<Image>().color = Color.blue;
                    }
                    pointList.Add(point);
                }
            }
        }
    }

    public void ResetGameArray()
    {
        for(int row = 0; row < RowCount; row++)
        {
            for(int col = 0; col < ColumnCount; col++)
            {
                gameArray[col, row] = (int)Content.Empty;
            }
        }
    }
}
