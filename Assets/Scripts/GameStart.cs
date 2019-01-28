using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BlockController.Module[] startingModules = new BlockController.Module[BlockArrayManager.ColumnCount];
        for (int col = 0; col < BlockArrayManager.ColumnCount; col++)
        {
            GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().SetModuleContent(col, 13, (int)BlockArrayManager.Content.Block);
            startingModules[col].posX = col;
            startingModules[col].posY = 14;
        }
        GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().SetModuleContent(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), 12, (int)BlockArrayManager.Content.Character);
        GameObject.Find("GameBoardPanel").GetComponent<BlockArrayManager>().SetModuleContent(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), 11, (int)BlockArrayManager.Content.Character);

        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewBlock(startingModules);
        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateCharacter(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), 13);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAction>().InitializeCharacterPosition(Mathf.CeilToInt(BlockArrayManager.ColumnCount / 2), 12);

        GameObject.Find("GameBoardPanel").GetComponent<BlockController>().ChangeControlBlock();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
