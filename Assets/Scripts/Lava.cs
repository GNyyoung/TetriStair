using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//나중에 ui 제거

public class Lava : MonoBehaviour {

    int lavaHeight;
    public float sinkTime;
    public int maxHeight;
    float startPosY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("LavaPosition").GetComponent<Text>().text = ((this.gameObject.GetComponent<RectTransform>().localPosition.y + 380) / 100).ToString();
        CheckFallLava();
        startPosY = this.GetComponent<RectTransform>().localPosition.y;
    }

    public void UpdateLavaHeight(int directionVert)
    {
        GetComponent<RectTransform>().localPosition += Vector3.up * BlockArrayManager.ModuleDistance * directionVert;
    }
    public void UpdateLavaHeight(float sinkTime, float deltaTime)
    {
        GetComponent<RectTransform>().localPosition += Vector3.up * BlockArrayManager.ModuleDistance / sinkTime * deltaTime;
    }

    //캐릭터가 용암에 빠지는지 체크
    public void CheckFallLava()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<RectTransform>().localPosition.y - GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y / 2 + GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().sizeDelta.y / 2 + GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().localPosition.y - GameObject.FindGameObjectWithTag("Player").GetComponent<RectTransform>().sizeDelta.y / 2 <
            this.GetComponent<RectTransform>().localPosition.y + this.GetComponent<RectTransform>().sizeDelta.y / 2 - GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y / 2 + GameObject.Find("Main Camera").GetComponent<EventManager>().GetMaxClimbHeight() * 100)
        {
            GameObject.Find("Main Camera").GetComponent<EventManager>().GameOver();
        }
        GameObject.Find("PlayerH").GetComponent<Text>().text = (GameObject.FindGameObjectWithTag("Player").GetComponent<RectTransform>().localPosition.y - GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y / 2 + GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().sizeDelta.y / 2 + GameObject.Find("GameBoardPanel").GetComponent<RectTransform>().localPosition.y - GameObject.FindGameObjectWithTag("Player").GetComponent<RectTransform>().sizeDelta.y / 2).ToString("N2");
        GameObject.Find("LavaH").GetComponent<Text>().text = (this.GetComponent<RectTransform>().localPosition.y + this.GetComponent<RectTransform>().sizeDelta.y / 2 - GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta.y / 2 + GameObject.Find("Main Camera").GetComponent<EventManager>().GetMaxClimbHeight() * 100).ToString("N2");
    }

    //테스트용 메서드. 나중에 삭제
    public void SetMaxHeight(int height)
    {
        maxHeight = height;
    }
}
