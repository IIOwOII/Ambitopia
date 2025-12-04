using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    PlayerManager theplayer;
    private Animator theanim;
    private bool keygetpossible;

    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        theanim = GetComponent<Animator>();
    }

    void Start()
    {
        keygetpossible = true;
    }

    void Update()
    {
        if (keygetpossible)
        {
            /* MenuCondition에 대하여,
             * 0000이 기본, 이진수처럼 취급하여 계산한다.
             * 첫째 자리: AllOff
             * 둘째 자리: EquipOn
             * 셋째 자리: InventoryOn
             * 넷째 자리: InventoryPanelOn
             * 참은 1, 거짓은 0으로 취급한다.
             */
            //X
            if (Input.GetKeyDown(KeyCode.X))
            {
                switch (theanim.GetInteger("MenuCondition"))
                {
                    case 0:
                        theanim.SetInteger("MenuCondition", 8);
                        break;
                    case 3:
                        theanim.SetInteger("MenuCondition", 2);
                        break;
                    case 2:
                    case 4:
                        theanim.SetInteger("MenuCondition", 0);
                        break;
                }
            }
            //→
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (theanim.GetInteger("MenuCondition") == 0)
                {
                    theanim.SetInteger("MenuCondition", 2);
                }
            }
            //←
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (theanim.GetInteger("MenuCondition") == 0)
                {
                    theanim.SetInteger("MenuCondition", 4);
                }
            }
            //↓
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (theanim.GetInteger("MenuCondition") == 2)
                {
                    theanim.SetInteger("MenuCondition", 3);
                }
            }
        }
    }

    //애니메이션 끝날 때 키입력 가능
    public void AnimationEnd()
    {
        keygetpossible = true;
    }

    //애니메이션 시작할 때 키입력 불가능
    public void AnimationStart()
    {
        keygetpossible = false;
    }

    //메뉴 종료
    public void MenuOff()
    {
        theplayer.keygetpossible = true;
        this.gameObject.SetActive(false);
    }
}