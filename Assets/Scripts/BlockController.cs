using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    public Module[] controlBlock = new Module[4];

    public struct Module
    {
        public int indexX, indexY;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //조종중인 블럭을 회전시킨다.
    public void RotateBlock(bool direction)
    {

    }

    //블럭이 시간에 따라 자동으로 내려갈 때 사용하는 메서드
    public void FallBlock()
    {

    }

    //블럭을 빠른 낙하시킬 때 사용하는 메서드
    public void FastFallBlock()
    {
        int distance = int.MaxValue;
        for(int i = 0; i < controlBlock.Length; i++)
        {
            //controlBlock[i].indexY
            GetComponent<BlockArrayManager>().GetCollisionDistance();
        }
    }
}
