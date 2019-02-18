using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//나중에 ui 제거

public class EventManager : MonoBehaviour {

    float deltaTime;
    const float maxFallTime = 0.7f;             //블럭 추락 주기의 최초 시간 1.0초
    const float minFallTime = 0.4f;
    float fallTime;
    float fallCooltime = 0;
    float sinkTime;
    float sinkCooltime = 0;
    const float maxSinkTime = 2.5f;                //용암 상승 주기의 최초 시간 3.5초
    const float minSinkTime = 1.1f;             //용암 상승 주기의 최소 시간 1.2초
    public int maxClimbHeight = 0;       //플레이어가 올라간 최대 높이
    const int naturalBlockCycle = 15;           //자연블럭이 몇층 올라갈 때마다 생기는가
    bool isCreateNaturalBlock = false;
    float sinkStopCooltime = 0;       //0이 아니면 0이하로 될때까지 용암 상승을 막음
    float sinkStopTime = 7.0f;

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
        CreateNaturalBlock();
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

    //용암 상승을 관리하는 메서드
    private void BoardSinkCycle()
    {
        if (sinkStopCooltime <= 0)
        {
            if (sinkCooltime > sinkTime)
            {
                GameObject.Find("Lava").GetComponent<Lava>().UpdateLavaHeight(1);
                sinkCooltime = 0;
            }
            else
                sinkCooltime += deltaTime;
        }
        else
            sinkStopCooltime -= deltaTime;
    }

    //플레이어가 층 올라갈 때마다 낙하시간 계산
    //최소 시간에 도달하면 계산 안하게 짜면 좋을듯
    private void CalcFallTime()
    {
        fallTime = maxFallTime - (maxFallTime - minFallTime) * maxClimbHeight / 200;
        if (fallTime < minFallTime)
            fallTime = minFallTime;
        GameObject.Find("FallSpeed").GetComponent<Text>().text = "추락속도: " + fallTime.ToString("N2"); 
    }

    //플레이어가 층 올라갈 때마다 가라앉는 시간 계산
    //최소 시간에 도달하면 계산 안하게 짜면 좋을듯
    //재밌을거 같은데 playerMaxHeight로 고치지 말고 둬보자. 안올라가고 버틸수록 용암 상승속도가 느려서 유리해짐
    private void CalcSinkTime()
    {
        float sinkDecrement = Mathf.Pow(Mathf.Log(maxClimbHeight + 1, 4), 1.0f) / 5.0f;      //317층부터 minSinkTime이 적용됨
        sinkTime = sinkDecrement > maxSinkTime - minSinkTime ? minSinkTime : maxSinkTime - sinkDecrement;
        GameObject.Find("Lava").GetComponent<Lava>().sinkTime = sinkTime;
        GameObject.Find("LavaSpeed").GetComponent<Text>().text = "용암속도: " + sinkTime.ToString("N2");
    }

    //등반높이를 1칸 올림
    public void UpdateMaxClimbHeight()
    {
        maxClimbHeight += 1;
        GameObject.Find("Lava").GetComponent<Lava>().SetMaxHeight(maxClimbHeight);
    }

    public int GetMaxClimbHeight()
    {
        return maxClimbHeight;
    }

    public void ResetFallColltime()
    {
        fallCooltime = 0;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameObject gameoverPanel = GameObject.Find("Canvas").transform.Find("GameOver").gameObject;
        gameoverPanel.transform.Find("ResultOutputText").GetComponent<Text>().text = maxClimbHeight.ToString();
        gameoverPanel.SetActive(true);
        //그 다음 명령어
    }

    //플레이어가 일정 높이를 올라갈 때마다 자연블럭을 생성함
    void CreateNaturalBlock()
    {
        
        if(maxClimbHeight % naturalBlockCycle != 0)
            isCreateNaturalBlock = true;
        else if (isCreateNaturalBlock == true)
        {
            print("자연블럭 생성");
            isCreateNaturalBlock = false;
            GameObject.Find("GameBoardPanel").GetComponent<NaturalBlockCreator>().CreateNaturalBlock();
        }
    }

    public void SetSinkStopTime()
    {
        sinkStopCooltime = sinkStopTime;
    }

    public void ResetAllEvent()
    {
        sinkTime = maxSinkTime;
        fallTime = maxFallTime;
        fallCooltime = 0;
        sinkCooltime = 0;
        sinkStopCooltime = 0;
        isCreateNaturalBlock = false;
        maxClimbHeight = 0;
    }
}
