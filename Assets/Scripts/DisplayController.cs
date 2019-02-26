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
    List<GameObject> controlBlockObject = new List<GameObject>();     //현재 조종중인 블럭 오브젝트
    List<GameObject> previewBlockObject = new List<GameObject>();    //추락 시 미리보기 블럭 오브젝트
    RectTransform gameBoardRectTransform;
    Vector3 initialGameBoardRectPosition;
    public GameObject lava;
    bool isBackgroundMove = false;

    // Use this for initialization
    void Start () {
        backgroundPanelNum = GameObject.Find("Canvas").transform.Find("Background").childCount;
        backgroundTransform = GameObject.Find("Canvas").transform.Find("Background").GetComponent<RectTransform>();
        gameBoardRectTransform = GameObject.Find("GameBoard").GetComponent<RectTransform>();
        initialGameBoardRectPosition = gameBoardRectTransform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        BackgroundPanelMove();
	}

    //배경이 계속 이어지게 함
    public void BackgroundPanelMove()
    {
        if (backgroundTransform.localPosition.y < -ResolutionHeight * backgroundCount)
        {
            print(backgroundTransform.localPosition.y + ", " + -ResolutionHeight * backgroundCount);
            backgroundTransform.GetChild((backgroundCount - 1) % 3).localPosition += Vector3.up * backgroundPanelNum * ResolutionHeight;
            print(backgroundPanelNum * ResolutionHeight);
            backgroundCount += 1;
        }
    }

    //배경을 움직임
    public void BackgroundMove()
    {
        backgroundTransform.localPosition += Vector3.down * BlockArrayManager.ModuleDistance;
    }

    //캐릭터가 최대 높이 갱신해서 올라갈때 실행
    public void DownAllModule()
    {
        gameBoardRectTransform.localPosition += Vector3.down * BlockArrayManager.ModuleDistance;
        //배열 밖으로 나간 오브젝트들 제거하는 메서드 실행

        GameObject[] allModuleObjects = GameObject.FindGameObjectsWithTag("Module");
        for(int i = allModuleObjects.Length - 1; i >= 0; i--)
        {
            if(Mathf.Abs(allModuleObjects[i].GetComponent<RectTransform>().localPosition.y + gameBoardRectTransform.localPosition.y - initialGameBoardRectPosition.y) > 
                GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().sizeDelta.y + BlockArrayManager.ModuleDistance)
            {
                Destroy(allModuleObjects[i]);
            }
        }
        CheckBlindBlock();
    }

    //블럭 떨어지고 나서 새로운 블럭 생성할 때 실행
    public void InstantiateNewBlock(BlockController.Module[] module)
    {
        controlBlockObject = new List<GameObject>();
        for (int i = 0; i < module.Length; i++)
        {
            controlBlockObject.Add(Instantiate(moduleObject, gameBoardRectTransform));
            controlBlockObject[i].GetComponent<RectTransform>().localPosition = 
                new Vector3(
                    module[i].posX + 0.5f,
                    -(module[i].posY + 0.5f - BlockArrayManager.unusedTopRowCount)) * BlockArrayManager.ModuleDistance + Vector3.down * (gameBoardRectTransform.localPosition.y - initialGameBoardRectPosition.y);
        }

        GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().ShowContent();
    }

    public void InstantiateNewModule(int posX, int posY)
    {
        GameObject newModule = Instantiate(moduleObject, gameBoardRectTransform);
        newModule.GetComponent<RectTransform>().localPosition = 
            new Vector3(posX + 0.5f, -(posY + 0.5f - BlockArrayManager.unusedTopRowCount)) * BlockArrayManager.ModuleDistance +
            Vector3.down * (gameBoardRectTransform.localPosition.y - initialGameBoardRectPosition.y);

        GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().ShowContent();
    }

    //캐릭터 오브젝트를 생성함
    public void InstantiateCharacter(int posX, int posY)
    {
        GameObject character = Instantiate(characterObject);
        character.GetComponent<RectTransform>().SetParent(GameObject.Find("GameBoard").transform);
        character.GetComponent<RectTransform>().localPosition = 
            new Vector3(
                BlockArrayManager.ModuleDistance * (posX + 0.5f), 
                -BlockArrayManager.ModuleDistance * (posY - BlockArrayManager.unusedTopRowCount) - (BlockArrayManager.ModuleDistance * 2 - character.GetComponent<RectTransform>().sizeDelta.y) / 2);
        character.GetComponent<RectTransform>().localScale = Vector3.one;
        GameObject.Find("Canvas").GetComponent<UIManager>().SetCharacter(character);
    }

    //블럭 이미지의 위치를 바꿔줌
    public void MoveBlock(int horzDistance, int vertDistance)
    {
        for(int i = 0; i < controlBlockObject.Count; i++)
        {
            controlBlockObject[i].GetComponent<RectTransform>().localPosition += 
                Vector3.right * BlockArrayManager.ModuleDistance * horzDistance + 
                Vector3.down * BlockArrayManager.ModuleDistance * vertDistance;
        }
        CheckBlindBlock();
    }

    //캐릭터 움직임 관련
    public void CharacterMove(int horzDistance, int vertDistance)
    {
        //print(horzDistance + ", " + vertDistance);
        characterObject.GetComponent<RectTransform>().localPosition +=
            Vector3.right * BlockArrayManager.ModuleDistance * horzDistance +
            Vector3.down * BlockArrayManager.ModuleDistance * vertDistance;
    }

    public void UpdateRotation(BlockController.Module[] fixedModule)
    {
        if(controlBlockObject.Count == fixedModule.Length)
        {
            for(int i = 0; i < controlBlockObject.Count; i++)
            {
                controlBlockObject[i].GetComponent<RectTransform>().localPosition =
                    new Vector3(
                        fixedModule[i].posX + 0.5f,
                        -(fixedModule[i].posY + 0.5f - BlockArrayManager.unusedTopRowCount)) * BlockArrayManager.ModuleDistance + Vector3.down * (gameBoardRectTransform.localPosition.y - initialGameBoardRectPosition.y);
            }
        }
        GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().ShowContent();
    }

    public void SetCharacterObject(GameObject characterObject)
    {
        this.characterObject = characterObject;
    }

    public void DestroyControlBlockObject()
    {
        for(int i = 0; i < controlBlockObject.Count; i++)
        {
            Destroy(controlBlockObject[i]);
        }
    }

    public void UpdateLavaPosition(int lavaHeight)
    {
        lava.GetComponent<RectTransform>().localPosition += Vector3.up * (GameObject.Find("Main Camera").GetComponent<EventManager>().GetMaxClimbHeight() - lavaHeight) * BlockArrayManager.ModuleDistance;
    }

    //블럭이 떨어질 위치를 미리 보여줌
    public void BlockPreview()
    {
        //미리보기 블럭 생성해서 controlBlock이 떨어질 곳과 똑같은 위치에 배치
        // controlBlock이 실제로 떨어지면 미리보기는 제거
        if(previewBlockObject.Count == 0)
        {
            for (int i = 0; i < controlBlockObject.Count; i++)
            {
                previewBlockObject.Add(Instantiate(moduleObject, gameBoardRectTransform));
                previewBlockObject[i].GetComponent<Image>().color -= Color.black * 0.5f;
            }
        }
        int moveDistance = GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().GetCollisionDistance(GameObject.Find("GameBoardPanel").GetComponent<BlockController>().controlBlock, false);

        //controlBlock이 다른 블럭에 안걸쳐질때
        if (moveDistance == int.MaxValue ||
            moveDistance == -1)
        {
            ResetPreview();
            return;
        }

        for(int i = 0; i < previewBlockObject.Count; i++)
        {
            previewBlockObject[i].GetComponent<RectTransform>().localPosition = 
                controlBlockObject[i].GetComponent<RectTransform>().localPosition + 
                Vector3.down * BlockArrayManager.ModuleDistance * moveDistance;
        }
        CheckBlindBlock();
    }

    public void ResetPreview()
    {
        for(int i = previewBlockObject.Count - 1; i >= 0; i--)
        {
            Destroy(previewBlockObject[i]);
        }
        previewBlockObject = new List<GameObject>();
    }

    public void CheckBlindBlock()
    {
        GameObject[] allModuleObjects = GameObject.FindGameObjectsWithTag("Module");
        for(int i = 0; i < allModuleObjects.Length; i++)
        {
            if (Mathf.Abs(allModuleObjects[i].GetComponent<RectTransform>().localPosition.y + gameBoardRectTransform.localPosition.y - initialGameBoardRectPosition.y) >
                GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().sizeDelta.y)
            {
                allModuleObjects[i].GetComponent<Image>().color -= new Color(0, 0, 0, allModuleObjects[i].GetComponent<Image>().color.a);
            }
        }
    }
}
