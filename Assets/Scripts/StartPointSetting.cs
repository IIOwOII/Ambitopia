using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointSetting : MonoBehaviour
{
    //이동 위치: startpoint랑 MapMove의 warppoint가 같으면 player를 이곳으로 이동
    public int startpoint;
    //startpoint와 관계없이 단순히 맵만 이동시키는지 여부 (위치이동 제외 여부)
    public bool PosFix = false;

    private PlayerManager theplayer;

    void Start()
    {
        theplayer = FindObjectOfType<PlayerManager>();

        if (!PosFix)
        {
            if (startpoint == theplayer.towardpoint)
                theplayer.transform.position = this.transform.position;
        }
    }
}
