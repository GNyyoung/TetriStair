using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour {

    Text fallText;
    Text sinkText;

    // Use this for initialization
    void Start () {
        fallText = GameObject.Find("FallText").GetComponent<Text>();
        sinkText = GameObject.Find("SinkText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickGameStart()
    {
        SceneManager.LoadScene("Game");
        GameObject.Find("DontDestroyOnLoad").GetComponent<BalanceControl>().SetFallTime(GameObject.Find("FallSlider").GetComponent<Slider>().value);
        GameObject.Find("DontDestroyOnLoad").GetComponent<BalanceControl>().SetSinkTime(GameObject.Find("SinkSlider").GetComponent<Slider>().value);
    }

    public void OnValueChangeFallSlider()
    {
        fallText.text = GameObject.Find("FallSlider").GetComponent<Slider>().value.ToString("N2");
    }

    public void OnValueChangeSinkSlider()
    {
        sinkText.text = GameObject.Find("SinkSlider").GetComponent<Slider>().value.ToString("N2");
    }
}
