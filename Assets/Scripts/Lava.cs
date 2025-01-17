﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//나중에 ui 제거

public class Lava : MonoBehaviour {

    int lavaHeight;
    public float sinkTime;
    public int maxHeight;
    float startPosY;
    float gapLavaToPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameStart.isGame == true)
        {
            gapLavaToPlayer = (this.gameObject.GetComponent<RectTransform>().localPosition.y + 380) / 100;
            GameObject.Find("LavaPosition").GetComponent<Text>().text = gapLavaToPlayer.ToString();
            CheckFallLava();
            startPosY = this.GetComponent<RectTransform>().localPosition.y;
        }
    }

    //플레이어가 1칸 올라갈 때 용암 높이 내리는 용도로 사용
    public void UpdateLavaHeight(int directionVert)
    {
        GetComponent<RectTransform>().localPosition += Vector3.up * BlockArrayManager.ModuleDistance * directionVert;
    }
    //용암을 실시간으로 올라오게 하는 데에 사용
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
    }

    //테스트용 메서드. 나중에 삭제
    public void SetMaxHeight(int height)
    {
        maxHeight = height;
    }

    public float GetGapLavaToPlayer()
    {
        return gapLavaToPlayer;
    }
}
