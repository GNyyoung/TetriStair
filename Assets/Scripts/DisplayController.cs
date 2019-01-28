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
    public GameObject moduleObject;
    public GameObject characterObject;
    List<GameObject> controlBlock = new List<GameObject>();     //현재 조종중인 블럭 오브젝트
    Vector3 gameBoardPosition;


    // Use this for initialization
    void Start () {
        backgroundPanelNum = GameObject.Find("Canvas").transform.Find("Background").childCount;
        backgroundTransform = GameObject.Find("Canvas").transform.Find("Background") as RectTransform;
        gameBoardPosition = GameObject.Find("GameBoard").GetComponent<RectTransform>().localPosition;
    }
	
	// Update is called once per frame
	void Update () {
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
        backgroundTransform.localPosition -= Vector3.down * BlockArrayManager.ModuleDistance;
    }

    //캐릭터가 최대 높이 갱신해서 올라갈때 실행
    public void DownAllModule()
    {
        RectTransform gameBoardTransform = GameObject.Find("GameBoard").transform as RectTransform;
        gameBoardTransform.localPosition -= Vector3.down * BlockArrayManager.ModuleDistance;
    }

    //블럭 떨어지고 나서 새로운 블럭 생성할 때 실행
    public void InstantiateNewBlock(BlockController.Module[] module)
    {
        controlBlock = new List<GameObject>();
        RectTransform gameBoardRectTransform = GameObject.Find("GameBoard").GetComponent<RectTransform>();
        for (int i = 0; i < module.Length; i++)
        {
            controlBlock.Add(Instantiate(moduleObject,
                gameBoardRectTransform.localPosition +
                Vector3.right * BlockArrayManager.ModuleDistance * (module[i].posX + 0.5f) +
                Vector3.down * BlockArrayManager.ModuleDistance * (module[i].posY * 0.5f),
                Quaternion.identity,
                gameBoardRectTransform.transform));
            controlBlock[controlBlock.Count - 1].GetComponent<RectTransform>().localPosition = new Vector3(BlockArrayManager.ModuleDistance * (module[i].posX + 0.5f), -BlockArrayManager.ModuleDistance * (module[i].posY + 0.5f - BlockArrayManager.unusedTopRowCount));
        }
    }
    
    //캐릭터 오브젝트를 생성함
    public void InstantiateCharacter(int posX, int posY)
    {
        RectTransform gameBoardRectTransform = GameObject.Find("GameBoard").GetComponent<RectTransform>();
        GameObject character = Instantiate(characterObject);
        character.GetComponent<RectTransform>().SetParent(GameObject.Find("GameBoard").transform);
        character.GetComponent<RectTransform>().localPosition = new Vector3(BlockArrayManager.ModuleDistance * (posX + 0.5f), -BlockArrayManager.ModuleDistance * (posY - BlockArrayManager.unusedTopRowCount) - (BlockArrayManager.ModuleDistance * 2 - character.GetComponent<RectTransform>().sizeDelta.y) / 2);
        character.GetComponent<RectTransform>().localScale = Vector3.one;
        GameObject.Find("Canvas").GetComponent<UIManager>().SetCharacter(character);
    }

    //블럭 이미지의 위치를 바꿔줌
    public void MoveBlock(int horzDistance, int vertDistance)
    {
        for(int i = 0; i < controlBlock.Count; i++)
        {
            controlBlock[i].GetComponent<RectTransform>().localPosition += 
                Vector3.right * BlockArrayManager.ModuleDistance * horzDistance + 
                Vector3.down * BlockArrayManager.ModuleDistance * vertDistance;
        }
    }

    //캐릭터 움직임 관련
    public void CharacterMove(int horzDistance, int vertDistance)
    {
        characterObject.GetComponent<RectTransform>().position += 
            Vector3.right * BlockArrayManager.ModuleDistance * horzDistance + 
            Vector3.down * BlockArrayManager.ModuleDistance * vertDistance;
    }
}
