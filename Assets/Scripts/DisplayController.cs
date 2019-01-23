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


    // Use this for initialization
    void Start () {
        backgroundPanelNum = GameObject.Find("Canvas").transform.Find("Background").childCount;
        backgroundTransform = GameObject.Find("Canvas").transform.Find("Background") as RectTransform;
    }
	
	// Update is called once per frame
	void Update () {
        BackgroundMove();
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
        backgroundTransform.localPosition = -mainCamera.transform.position;
    }
}
