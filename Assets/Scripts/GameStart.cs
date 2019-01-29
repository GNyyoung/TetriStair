using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

    const int floorPosY = 14;

	// Use this for initialization
	void Start () {
        GameObject gameBoardPanel = GameObject.Find("GameBoardPanel");

        //기본 및 회전 상태에서 블럭의 위치 정보 세팅
        gameBoardPanel.GetComponent<BlockRotation>().SetBlockRotation();

        //게임 시작시 밑바닥 블럭 세팅
        BlockController.Module[] startingModules = new BlockController.Module[BlockArrayManager.ColumnCount];
        for (int col = 0; col < BlockArrayManager.ColumnCount; col++)
        {
            gameBoardPanel.GetComponent<BlockArrayManager>().SetModuleContent(col, floorPosY, (int)BlockArrayManager.Content.Block);
            startingModules[col].posX = col;
            startingModules[col].posY = 14;
        }

        //캐릭터 위치 세팅
        gameBoardPanel.GetComponent<BlockArrayManager>().SetModuleContent(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 1, (int)BlockArrayManager.Content.Character);
        gameBoardPanel.GetComponent<BlockArrayManager>().SetModuleContent(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 2, (int)BlockArrayManager.Content.Character);

        //블럭 오브젝트 생성
        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewBlock(startingModules);
        //캐릭터 오브젝트 생성
        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateCharacter(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAction>().InitializeCharacterPosition(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), floorPosY - 1);

        //테트리스 블럭 생성
        gameBoardPanel.GetComponent<BlockController>().ChangeControlBlock();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
