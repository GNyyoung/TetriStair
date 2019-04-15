using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

    const int floorPosY = BlockArrayManager.RowCount - 3 - BlockArrayManager.unusedBotRowCount;
    GameObject gameBoardPanel;

    bool isStart = false;       //게임 시작 누른 후 작동
    public static bool isGame = false;        //화면 이동 끝난 후 작동
    float distance = 10;
    bool slow = false;
    Sprite[] startImage = new Sprite[4];
    GameObject[] backgroundArray;

    int backgroundCount = 1;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        

        backgroundArray = new GameObject[GameObject.Find("Background").transform.childCount];
        for(int i = 0; i < backgroundArray.Length; i++)
        {
            backgroundArray[i] = GameObject.Find("Background").transform.GetChild(i).gameObject;
        }

        if(isGame == true)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().ActiveUI();
            gameBoardPanel = GameObject.Find("GameBoardPanel");
            GameObject.Find("Main Camera").GetComponent<DisplayController>().SetGameBoardTransform();
            InitializeGame();
            isStart = false;
            GameObject.Find("GameStartPanel").SetActive(false);
            for(int i = 0; i < GameObject.Find("Background").transform.childCount; i++)
            {
                GameObject.Find("Background").transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/배경");
            }
        }
        else
        {
            for (int i = 0; i < startImage.Length; i++)
            {
                //startImage[i] = 
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isStart == true)
        {
            if (distance < 50 && slow == false)
            {
                distance += 0.5f;
            }
            else
            {
                slow = true;
                distance -= 0.5f;
                if (distance < 0)
                {
                    isGame = true;
                    GameObject.Find("Canvas").GetComponent<UIManager>().ActiveUI();
                    gameBoardPanel = GameObject.Find("GameBoardPanel");
                    GameObject.Find("Main Camera").GetComponent<DisplayController>().SetGameBoardTransform();
                    InitializeGame();
                    isStart = false;
                    GameObject.Find("GameStartPanel").SetActive(false);
                }
            }
            GameObject.Find("Background").transform.position += Vector3.up * Mathf.Pow(distance, 2) * Time.deltaTime;
        }

        //if (backgroundArray[2 - (backgroundCount % 3)].transform.position.y > DisplayController.ResolutionHeight * backgroundCount)
        if (GameObject.Find("Background").GetComponent<RectTransform>().localPosition.y > DisplayController.ResolutionHeight * backgroundCount)
        {
            backgroundArray[2 - ((backgroundCount - 1) % 3)].transform.localPosition -= Vector3.up * DisplayController.ResolutionHeight * backgroundArray.Length;
            backgroundCount += 1;
        }
    }

    public void InitializeGame()
    {
        //GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().ResetGameArray();
        //GetComponent<EventManager>().ResetAllEvent();

        //게임 시작시 밑바닥 블럭 세팅
        BlockController.Module[] startingModules = new BlockController.Module[BlockArrayManager.ColumnCount];
        for (int col = 0; col < BlockArrayManager.ColumnCount; col++)
        {
            gameBoardPanel.GetComponent<BlockArrayManager>().SetModuleContent(col, floorPosY, (int)BlockArrayManager.Content.Block);
            startingModules[col].posX = col;
            startingModules[col].posY = floorPosY;
        }   

        //캐릭터 위치 세팅
        gameBoardPanel.GetComponent<BlockArrayManager>().SetModuleContent(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 1, (int)BlockArrayManager.Content.Character);
        gameBoardPanel.GetComponent<BlockArrayManager>().SetModuleContent(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 2, (int)BlockArrayManager.Content.Character);

        //블럭 오브젝트 생성
        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewBlock(startingModules, 0);
        //캐릭터 오브젝트 생성
        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateCharacter(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAction>().InitializeCharacterPosition(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 1);

        //테트리스 블럭 생성
        gameBoardPanel.GetComponent<BlockController>().ChangeControlBlock();
    }

    public void StartGame()
    {
        isStart = true;
    }
}
