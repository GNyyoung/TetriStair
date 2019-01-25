using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour {

    private int posX, posY; //기준좌표는 캐릭터 하체부분. 캐릭터는 element 두개를 차지한다.
    private int climbHeight = 0;
    private int maxClimbHeight = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //blockArrayManager에 있는 캐릭터 좌표를 이동 가능한 경우에 이동시킴.
    //캐릭터는 같은 위치에서 계속 있어야하므로 캐릭터가 위아래로 이동하는게 아니라 블럭들이 아래 위로 이동하게 해야함.
    //캐릭터가 그냥 왔다갔다 할때랑 최대 높이 갱신할 때랑 다르게 굴려야함.
    //최고 높이 갱신하는거면 가장 밑에 있는 블럭들 지우고 그냥 왔다갔다 하는거면 캐릭터 위치를 변경시켜서 출력해야함.
    //왜냐면 캐릭터가 밑에 갔다가 위로 올라가면 위에 있던 블럭들이 지워질 수 있음.
    public void Move(int directionHorz)
    {
        int directionVert = 0;

        if (GetComponent<BlockArrayManager>().GetElementContent(posX + directionHorz, posY - 1) == 0)
        {
            if (GetComponent<BlockArrayManager>().GetElementContent(posX + directionHorz, posY) == 1)
            {
                //1칸 높은 곳으로 올라감
                directionVert = 1;
                climbHeight += 1;
            }
            else if (GetComponent<BlockArrayManager>().GetElementContent(posX + directionHorz, posY + 1) == 1)
            {
                //같은 높이에서 이동
            }
            else if (GetComponent<BlockArrayManager>().GetElementContent(posX + directionHorz, posY + 2) == 1)
            {
                //1칸 낮은 곳으로 내려감
                directionVert = -1;
                climbHeight -= 1;
            }
            else
                return;
        }
        else
            return;
        
        if (climbHeight > maxClimbHeight)
        {
            //캐릭터가 최고높이를 갱신할 때
            GetComponent<BlockArrayManager>().UpdateBoardAtClimb(posX, posY, directionHorz);
            maxClimbHeight = climbHeight;
            transform.position = new Vector3(transform.position.x + directionHorz * BlockArrayManager.ElementsDistance, transform.position.y, transform.position.z);
            posX += directionHorz;
        }
        else
        {
            //아래위로 왔다갔다 할때
            GetComponent<BlockArrayManager>().CharacterMove(posX, posY, directionHorz, directionVert);
            transform.position = new Vector3(transform.position.x + directionHorz * BlockArrayManager.ElementsDistance, transform.position.y + directionVert * BlockArrayManager.ElementsDistance, transform.position.z);
            posX += directionHorz;
            posY += directionVert;
        }
        //캐릭터 오브젝트가 이동하는 애니메이션 필요
    }
    
    
}
