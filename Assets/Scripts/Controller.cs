using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    int controllerType;
    public GameObject controller_joystick;
    public GameObject controller_joystick2;
    public GameObject controller_touch;
    public GameObject controller_buttonLeft;
    public GameObject controller_buttonRight;

    //조이스틱용 변수
    float moveDelay = 0.2f;
    float moveCooltime = 0;
    public GameObject joystickBackground;
    int joystickDirection = 0;



    // Use this for initialization
    void Start () {
        switch (controllerType)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                Debug.LogError("적절하지 않은 컨트롤러 타입");
                return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(controllerType == 0)
        {
            moveCooltime += Time.deltaTime;
            CharacterMove(joystickDirection);
        }
	}

    public void OnMouseDown()
    {

    }

    public void TouchDrag()
    {
        print(Input.mousePosition.x);
        print(joystickBackground.transform.position.x - GetComponent<RectTransform>().sizeDelta.x / 2);
        if (Input.mousePosition.x < joystickBackground.transform.position.x - GetComponent<RectTransform>().sizeDelta.x / 3)
        {
            if (joystickDirection != -1)
            {
                print("왼쪽 이동");
                transform.position += Vector3.left * GetComponent<RectTransform>().sizeDelta.x / 2;
                joystickDirection = -1;
            }
        }
        else if (Input.mousePosition.x > joystickBackground.transform.position.x + GetComponent<RectTransform>().sizeDelta.x / 2)
        {
            if (joystickDirection != 1)
            {
                print("오른쪽 이동");
                transform.position += Vector3.right * GetComponent<RectTransform>().sizeDelta.x / 2;
                joystickDirection = 1;
            }
        }
        else
        {
            transform.position = joystickBackground.transform.position;
            joystickDirection = 0;
        }

    }

    public void EndDrag()
    {
        transform.position = joystickBackground.transform.position;
        joystickDirection = 0;
    }

    public void OnMouseUp()
    {
        transform.position = joystickBackground.transform.position;
    }

    public void CharacterMove(int direction)
    {
        if (joystickDirection != 0 && moveCooltime > moveDelay)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAction>().CharacterMove(direction);
            moveCooltime = 0;
        }
    }
}
