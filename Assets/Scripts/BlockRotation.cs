using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class BlockRotation : MonoBehaviour {

    //기준점을 중심으로 나머지 모듈을 새로 배치한다.

    const int blockCount = 7;
    public const int rotation = 4;
    const int moduleCount = 3;  //기준좌표를 갖는 모듈은 제외
    const int position = 2;
    public static int[,,,] blockMove = new int[blockCount, rotation, moduleCount, position];

    private void Start()
    {
        
    }

    public void SetBlockRotation()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/Blocks.json");
        print(Application.dataPath + "/Resources/Blocks.json");
        JsonData blockRotationData = JsonMapper.ToObject(jsonString);
        for (int i = 0; i < blockRotationData["Blocks"].Count; i++)
        {
            blockMove[
                (int)blockRotationData["Blocks"][i]["BlockNum"], 
                (int)blockRotationData["Blocks"][i]["Rotation"],
                (int)blockRotationData["Blocks"][i]["ModuleNum"],
                (int)blockRotationData["Blocks"][i]["Position"]] = (int)blockRotationData["Blocks"][i]["Value"];
        }
    }
}
