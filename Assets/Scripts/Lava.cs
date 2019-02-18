using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//나중에 ui 제거

public class Lava : MonoBehaviour {

    int lavaHeight;
    public float sinkTime;
    public int maxHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("LavaPosition").GetComponent<Text>().text = ((this.gameObject.GetComponent<RectTransform>().localPosition.y + 380) / 100).ToString();
        CheckFallLava();
    }

    public void UpdateLavaHeight(int directionVert)
    {
        GetComponent<RectTransform>().localPosition += Vector3.up * BlockArrayManager.ModuleDistance * directionVert;
    }

    //캐릭터가 용암에 빠지는지 체크
    public void CheckFallLava()
    {
        if(GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().localPosition.y - GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().sizeDelta.y / 2 + 3 * BlockArrayManager.ModuleDistance
            < GetComponent<RectTransform>().localPosition.y + GetComponent<RectTransform>().sizeDelta.y / 2)
        {
            GameObject.Find("Main Camera").GetComponent<EventManager>().GameOver();
        }
    }

    //테스트용 메서드. 나중에 삭제
    public void SetMaxHeight(int height)
    {
        maxHeight = height;
    }
}
