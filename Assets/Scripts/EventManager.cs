using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    float deltaTime;
    const float maxFallTime = 1;
    const float minFallTime = 0.5f;
    float fallTime;
    float fallCooltime = 0;
    const float maxSinkTime = 7;
    const float minSinkTime = 1;
    int maxClimbHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime = Time.deltaTime;
        CalcFallTime();
        BlockFallCycle();
	}

    //블럭을 언제 1칸 떨어지게 할지 계산
    private void BlockFallCycle()
    {
        if(fallCooltime > fallTime)
        {
            GameObject.Find("GameBoardPanel").GetComponent<BlockController>().FallBlock();
            fallCooltime = 0;
        }
        else
            fallCooltime += deltaTime;
    }

    //플레이어가 층 올라갈 때마다 낙하시간 계산
    //최소 시간에 도달하면 계산 안하게 짜면 좋을듯
    private void CalcFallTime()
    {
        fallTime = maxFallTime - (maxFallTime - minFallTime) * maxClimbHeight / 200;
        if (fallTime < minFallTime)
            fallTime = minFallTime;
    }

    //플레이어가 층 올라갈 때마다 가라앉는 시간 계산
    //최소 시간에 도달하면 계산 안하게 짜면 좋을듯
    //재밌을거 같은데 playerMaxHeight로 고치지 말고 둬보자
    private float CalcSinkTime(int climbHeight)
    {
        float sinkDecrement = Mathf.Pow(Mathf.Log(climbHeight, 5), 2) / 2;      //264층부터 minSinkTime이 적용됨
        float sinkTime = sinkDecrement > maxSinkTime - minSinkTime ? minSinkTime : maxSinkTime - sinkDecrement;

        return sinkTime;
    }

    public void UpdateMaxClimbHeight()
    {
        maxClimbHeight += 1;
    }

    public void ResetFallColltime()
    {
        fallCooltime = 0;
    }
}
