using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayController : MonoBehaviour {

    public GameObject background;
    const int ResolutionHeight = 1920;
    public Camera mainCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BackgroundMove()
    {
        Mathf.FloorToInt(mainCamera.transform.position.y - ResolutionHeight);       //이걸로 어떤 인덱스를 이동시켜야 하는지 판단. 나머지를 구한다.
        //카메라 높이에 따라서 맨밑 배경이 위로 이동하게 한다.
    }
}
