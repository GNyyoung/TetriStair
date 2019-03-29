using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceControl : MonoBehaviour {

    float fallTime;
    float sinkTime;
    int controllerType;
    float resolutionWidthRate = 9;
    float resolutionHeightRate = 16;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        Screen.SetResolution(Screen.width, (int)(Screen.width * resolutionHeightRate / resolutionWidthRate), true);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void SetFallTime(float fallTime)
    {
        this.fallTime = fallTime;
    }
    public float GetFallTime()
    {
        return fallTime;
    }
    public void SetSinkTime(float sinkTime)
    {
        this.sinkTime = sinkTime;
    }
    public float GetSinkTime()
    {
        return sinkTime;
    }
    public void SetControllerType(int type)
    {
        controllerType = type;
    }
    public int GetControllerType()
    {
        return controllerType;
    }
    
}
