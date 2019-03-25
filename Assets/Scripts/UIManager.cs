using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameObject character;
    public bool isAllowFall = true;
    int controllerType;

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
    public void OnClickBlockFall()
    {
        //블럭이 한번에 떨어지는 코드
        if(isAllowFall == true)
        {
            isAllowFall = false;
            GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FastFallBlock();
        }

        //블럭이 한칸씩 빠르게 떨어지는 코드
        //어떻게 짜냐? 그냥 바꾸지 말까?
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
            OnClickBlockFall();
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
}
