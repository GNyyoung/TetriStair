using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayController : MonoBehaviour {

    public GameObject background;
    const int ResolutionHeight = 1920;
    public Camera mainCamera;
    private int backgroundPanelNum;
    private int backgroundCount = 1;
    RectTransform backgroundTransform;
    public GameObject module;


    // Use this for initialization
    void Start () {
        backgroundPanelNum = GameObject.Find("Canvas").transform.Find("Background").childCount;
        backgroundTransform = GameObject.Find("Canvas").transform.Find("Background") as RectTransform;
    }
	
	// Update is called once per frame
	void Update () {
        BackgroundMove();
        BackgroundPanelMove();
	}

    //배경이 계속 이어지게 함
    public void BackgroundPanelMove()
    {
        if(backgroundTransform.localPosition.y < - ResolutionHeight * backgroundCount)
        {
            print("실행중");
            backgroundTransform.GetChild((backgroundCount - 1) % 3).position += Vector3.up * backgroundPanelNum * ResolutionHeight;
            backgroundCount += 1;
        }
    }

    //배경을 움직임
    public void BackgroundMove()
    {
        backgroundTransform.localPosition = -mainCamera.transform.position;
    }

    //게임보드에 모듈 이미지를 띄움.
    public void ShowModule()
    {
        int[,] gameArray = GetComponent<BlockArrayManager>().GetGameArray();
        RectTransform gameBoardTransform = GameObject.Find("GameBoard").transform as RectTransform;

        for (int row = BlockArrayManager.unusedTopRowCount; row < BlockArrayManager.RowCount; row++)
        {
            for(int col = 0; col < BlockArrayManager.ColumnCount; col++)
            {
                if(gameArray[col,row] == 1)
                {
                    GameObject gameModule = Instantiate(module, gameBoardTransform);    //이렇게 하면 안됨. 이러면 매번 새로 만들어야 되잖아
                    gameModule.transform.position = new Vector3(
                        gameBoardTransform.position.x + gameBoardTransform.rect.width * (col + 0.5f), 
                        gameBoardTransform.position.y - gameBoardTransform.rect.height * (row - BlockArrayManager.unusedTopRowCount + 0.5f), 
                        gameModule.transform.position.z);
                }
            }
        }
    }
}
