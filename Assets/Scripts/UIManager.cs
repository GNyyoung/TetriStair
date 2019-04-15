using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameObject character;
    public bool isAllowFall = true;     //블럭이 내려간 후 새로 생성될 때까지 낙하 버튼이 눌리지 않게 함.
    int controllerType;

    public bool isFall = false;         //1칸 낙하할지 끝까지 낙하할지 판단하게 하는 변수
    float cooltime = 0;
    float durationTime = 0;

	// Use this for initialization
	void Start () {
        if (GameObject.Find("JoystickBackground") != null)
            GameObject.Find("JoystickBackground").SetActive(false);
        if (GameObject.Find("CMoveButton") != null)
            GameObject.Find("CMoveButton").SetActive(false);
        if (GameObject.Find("CMoveTouch") != null)
            GameObject.Find("CMoveTouch").SetActive(false);

        controllerType = GameObject.Find("DontDestroyOnLoad").GetComponent<BalanceControl>().GetControllerType();
        switch (controllerType)
        {
            case 0:
                //조이스틱
                GameObject.Find("Canvas").transform.Find("JoystickBackground").gameObject.SetActive(true);
                break;
            case 1:
                //이동버튼
                GameObject.Find("Canvas").transform.Find("CMove_Button").gameObject.SetActive(true);
                break;
            case 2:
                //화면터치
                GameObject.Find("Canvas").transform.Find("CMove_Touch").gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("잘못된 컨트롤러 타입");
                return;

        }
	}
	
	// Update is called once per frame
	void Update () {
        KeyBoardControl();

        if(isFall == true)
        {
            if (durationTime >= 0.2f && cooltime >= 0.15f)
            {
                GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FallBlock();
                cooltime = 0;
            }
            cooltime += Time.deltaTime;
            durationTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    //캐릭터 좌우 이동
    public void OnClickCharacterMove(int direction)
    {
        character.GetComponent<CharacterAction>().CharacterMove(direction);
    }

    //블럭 회전
    public void OnClickRotate()
    {
        GameObject.Find("GameBoardPanel").GetComponent<BlockController>().RotateBlock();
        if (GameObject.Find("GameBoardPanel").GetComponent<BlockController>().controlBlock != null)
            GameObject.Find("Main Camera").GetComponent<DisplayController>().BlockPreview();
    }

    //블럭 좌우 이동
    public void OnClickBlockMove(int direction)
    {
        GameObject.Find("GameBoardPanel").GetComponent<BlockController>().BlockHorzMove(direction);
        if(GameObject.Find("GameBoardPanel").GetComponent<BlockController>().controlBlock != null)
            GameObject.Find("Main Camera").GetComponent<DisplayController>().BlockPreview();
    }

    //블럭 빠른추락
    /*public void OnClickBlockFall()
    {
        if(durationTime < 0.5f)
        {
            if (isAllowFall == true)
            {
                isAllowFall = false;
                GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FastFallBlock();
            }
        }
        durationTime = 0;
    }*/

    //낙하 버튼을 오래 누르고 있으면 1칸씩 빠르게 내려감.
    public void OnButtonDownBlockFall()
    {
        isFall = true;
    }

    public void OnButtonUpBlockFall()
    {
        isFall = false;
        if (durationTime < 0.2f)
        {
            if (isAllowFall == true)
            {
                isAllowFall = false;
                GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FastFallBlock();
            }
        }
        durationTime = 0;
    }

    public void Test()
    {
        print("드래그?");
    }
    
    public void SetCharacter(GameObject character)
    {
        this.character = character;
    }

    public void KeyBoardControl()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            OnClickRotate();
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            OnButtonDownBlockFall();
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            OnButtonUpBlockFall();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            OnClickBlockMove(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            OnClickBlockMove(1);
        else if (Input.GetKeyDown(KeyCode.A))
            OnClickCharacterMove(-1);
        else if (Input.GetKeyDown(KeyCode.D))
            OnClickCharacterMove(1);
    }

    public void OnClickRestart()
    {
        //GameObject gameBoard = GameObject.Find("GameBoard");
        //for(int i = gameBoard.transform.childCount - 1; i >= 0; i--)
        //{
        //    gameBoard.transform.GetChild(i);
        //}
        //GameObject.Find("GameOver").SetActive(false);
        
        SceneManager.LoadScene("Game");
    }

    public void OnClickLoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickResume()
    {
        GameObject.Find("PausePanel").SetActive(false);
        Time.timeScale = 1;
    }

    public void OnclickPause()
    {
        GameObject.Find("Canvas").transform.Find("PausePanel").gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickDebugStop()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void OnClickStartPanel()
    {
        GameObject.Find("Main Camera").GetComponent<GameStart>().StartGame();
    }

    public void ActiveUI()
    {
        GameObject.Find("Canvas").transform.Find("GameBoardPanel").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("BlockController").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Lava").gameObject.SetActive(true);
    }
}
