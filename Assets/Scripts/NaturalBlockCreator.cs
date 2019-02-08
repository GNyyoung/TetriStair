using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalBlockCreator : MonoBehaviour {
    //최대 생성되는 블럭의 수는 25개.
    const int maxExpandCount = 3;
    const int maxCreateCount = 25;
    int createdBlockCount = 0;
    float maxProbility = 0.8f;
    float createProbility;
    // Use this for initialization
    void Start () {
        createProbility = Mathf.Sqrt(1 - Mathf.Pow(createdBlockCount / maxCreateCount, 2)) * maxProbility;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void CreateNaturalBlock()
    {
        int seedPosX = Random.Range(0, BlockArrayManager.ColumnCount);
        int seedPosY = maxExpandCount;
        GetComponent<BlockArrayManager>().SetModuleContent(seedPosX, seedPosY, (int)BlockArrayManager.Content.Block);
        GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(seedPosX, seedPosY);
        ExpendBlock(0, seedPosX, seedPosY);
    }

    //자연블럭 모듈을 확장시킴
    public void ExpendBlock(int createCount, int blockPosX, int blockPosY)
    {
        //들어가야 할거
        //생성할 블럭 위치가 잘못된 곳은 아닌지
        //확률에 따라서 생성할지 말지
        //3번 확인하고 나면 스탑
        //4방향을 모두 확인해야함
        if (createCount == maxExpandCount)
            return;
        else
        {
            //오른쪽 확인
            if (blockPosX + 1 >= BlockArrayManager.ColumnCount)
            {
                if (GetComponent<BlockArrayManager>().GetModuleContent(0, blockPosY) == (int)BlockArrayManager.Content.Empty &&
                    Random.value < createProbility)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(0, blockPosY, (int)BlockArrayManager.Content.Block);
                    GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(0, blockPosY);
                    createdBlockCount += 1;
                    ExpendBlock(createCount + 1, 0, blockPosY);
                }
            }
            else if (blockPosX + 1 < 0)
                return;
            else
            {
                if (GetComponent<BlockArrayManager>().GetModuleContent(blockPosX + 1, blockPosY) == (int)BlockArrayManager.Content.Empty &&
                    Random.value < createProbility)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(blockPosX + 1, blockPosY, (int)BlockArrayManager.Content.Block);
                    GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(blockPosX + 1, blockPosY);
                    createdBlockCount += 1;
                    ExpendBlock(createCount + 1, blockPosX + 1, blockPosY);
                }
            }

            //왼쪽 확인
            if (blockPosX - 1 < 0)
            {
                if (GetComponent<BlockArrayManager>().GetModuleContent(BlockArrayManager.ColumnCount - 1, blockPosY) == (int)BlockArrayManager.Content.Empty &&
                    Random.value < createProbility)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(BlockArrayManager.ColumnCount - 1, blockPosY, (int)BlockArrayManager.Content.Block);
                    GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(BlockArrayManager.ColumnCount - 1, blockPosY);
                    createdBlockCount += 1;
                    ExpendBlock(createCount + 1, BlockArrayManager.ColumnCount - 1, blockPosY);
                }
            }
            else if (blockPosX - 1 >= BlockArrayManager.ColumnCount)
                return;
            else
            {
                if (GetComponent<BlockArrayManager>().GetModuleContent(blockPosX - 1, blockPosY) == (int)BlockArrayManager.Content.Empty &&
                    Random.value < createProbility)
                {
                    GetComponent<BlockArrayManager>().SetModuleContent(blockPosX - 1, blockPosY, (int)BlockArrayManager.Content.Block);
                    GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(blockPosX - 1, blockPosY);
                    createdBlockCount += 1;
                    ExpendBlock(createCount + 1, blockPosX - 1, blockPosY);
                }
            }

            //위쪽 확인
            if(GetComponent<BlockArrayManager>().GetModuleContent(blockPosX, blockPosY - 1) == (int)BlockArrayManager.Content.Empty &&
                Random.value < createProbility / 2)
            {
                GetComponent<BlockArrayManager>().SetModuleContent(blockPosX, blockPosY - 1, (int)BlockArrayManager.Content.Block);
                GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(blockPosX, blockPosY - 1);
                createdBlockCount += 1;
                ExpendBlock(createCount + 1, blockPosX, blockPosY - 1);
            }


            //아래쪽 확인
            if (GetComponent<BlockArrayManager>().GetModuleContent(blockPosX, blockPosY + 1) == (int)BlockArrayManager.Content.Empty &&
                Random.value < createProbility / 2)
            {
                GetComponent<BlockArrayManager>().SetModuleContent(blockPosX, blockPosY + 1, (int)BlockArrayManager.Content.Block);
                GameObject.Find("Main Camera").GetComponent<DisplayController>().InstantiateNewModule(blockPosX, blockPosY + 1);

                ExpendBlock(createCount + 1, blockPosX, blockPosY + 1);
            }

        }
    }
}
