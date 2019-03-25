using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : MonoBehaviour {

    bool isClicked = false;
    Vector3 relativeDist;
    Vector3 initialPosition;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //오브젝트가 사각형으로 인식돼서, 스프라이트 밖에서는 드래그가 안되게 하기 위함.
    public void TestPointerDown()
    {
        float distance = Mathf.Pow(Input.mousePosition.x - transform.position.x, 2) + Mathf.Pow(Input.mousePosition.y - transform.position.y, 2);       //원 중앙과 얼마나 떨어져서 터치했나
        if (distance < Mathf.Pow(GetComponent<RectTransform>().lossyScale.x * GetComponent<RectTransform>().sizeDelta.x / 2, 2))
        {
            isClicked = true;
            relativeDist = Input.mousePosition - transform.position;
        }
    }

    public void TestDrag()
    {
        if(isClicked == true)
        {
            Vector3 dragDistance = Input.mousePosition - initialPosition - relativeDist;        //드래그한 거리
            if(Mathf.Pow(dragDistance.x, 2) + Mathf.Pow(dragDistance.y, 2) < 625)
            {
                transform.position = Input.mousePosition - relativeDist;
            }
            else
            {
                float rate = Mathf.Sqrt(625 / (Mathf.Pow(dragDistance.x, 2) + Mathf.Pow(dragDistance.y, 2)));
                transform.position = initialPosition + dragDistance * rate;
            }
        }
    }

    public void TestDragEnd()
    {
        transform.position = initialPosition;
        isClicked = false;
    }
}
