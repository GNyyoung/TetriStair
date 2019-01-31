using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    float deltaTime;
    const float maxFallTime = 2;
    const float minFallTime = 0.5f;
    float fallTime;
    float fallCooltime = 0;
    float sinkTime;
    float sinkCooltime = 0;
    const float maxSinkTime = 7;
    const float minSinkTime = 1;
    public static int maxClimbHeight = 0;
    int lavaHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime = Time.deltaTime;
        CalcFallTime();
        BlockFallCycle();
        CalcSinkTime();
        BoardSinkCycle();

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

    private void BoardSinkCycle()
    {
        if (sinkCooltime > sinkTime)
        {
            GameObject.Find("Lava").GetComponent<Lava>().UpdateLavaHeight();
            sinkCooltime = 0;
        }
        else
            sinkCooltime += deltaTime;
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
    private void CalcSinkTime()
    {
        float sinkDecrement = Mathf.Pow(Mathf.Log(maxClimbHeight + 1, 5), 2) / 2;      //264층부터 minSinkTime이 적용됨
        sinkTime = sinkDecrement > maxSinkTime - minSinkTime ? minSinkTime : maxSinkTime - sinkDecrement;
        
    }

    public void UpdateMaxClimbHeight()
    {
        maxClimbHeight += 1;
    }

    public void ResetFallColltime()
    {
        fallCooltime = 0;
    }

    public void GameOver()
    {
        print("게임오버");
        Time.timeScale = 0;
        //그 다음 명령어
    }
}
