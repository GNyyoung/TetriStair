using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class BlockRotation : MonoBehaviour {

    //기준점을 중심으로 나머지 모듈을 새로 배치한다.

    const int blockCount = 7;
    const int rotation = 4;
    const int moduleCount = 3;  //기준좌표를 갖는 모듈은 제외
    const int position = 2;
    public static int[,,,] blockMove = new int[blockCount, rotation, moduleCount, position];

    private void Start()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/Blocks.json");
        JsonData blockRotationData = JsonMapper.ToObject(jsonString);
        for(int i = 0; i < blockRotationData.Count; i++)
        {
            blockMove[
                (int)blockRotationData[i]["BlockNum"], 
                (int)blockRotationData[i]["Rotation"], 
                (int)blockRotationData[i]["ModuleNum"], 
                (int)blockRotationData[i]["Position"]] = (int)blockRotationData[i]["Value"];
        }
    }

    public BlockRotation()
    {
        //파일에서 값 가져와서 넣는걸로 하자...
    }
}
