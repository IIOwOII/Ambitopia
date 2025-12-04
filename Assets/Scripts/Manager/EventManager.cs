using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    EffectManager theeffect;
    PlayerManager theplayer;
    public GameObject eventscenepanel; //이벤트 연출 판넬
    private Animator eventscenepanelanim;
    WaitUntil waitpaneloff;
    WaitUntil distanceisone; //플레이어가 한 칸 이동했는지 확인
    public string eventmanifest; //일회성 이벤트 진행여부 확인 (0은 false, 1은 true)

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        theeffect = FindObjectOfType<EffectManager>();
        theplayer = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        eventmanifest = "000"; //일회성이벤트 총 개수만큼 크기가 늘어남
        eventscenepanelanim = eventscenepanel.GetComponent<Animator>();
        waitpaneloff = new WaitUntil(() => eventscenepanelanim.GetCurrentAnimatorStateInfo(0).IsName("EventOff") && eventscenepanelanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        distanceisone = new WaitUntil(() => Vector2.Distance(this.transform.position, theplayer.transform.position) >= 1f);
    }

    public void EventShowOn()
    {
        eventscenepanel.SetActive(true);
    }

    public void EventShowOff()
    {
        eventscenepanelanim.SetBool("beoff", true);
        StartCoroutine(EventShowReset());
    }

    private IEnumerator EventShowReset()
    {
        yield return waitpaneloff;
        eventscenepanel.SetActive(false);
    }

    public void EventScene(int _eventnumber)
    {
        switch (_eventnumber)
        {
            case 0: //튜토리얼 끝내기
                TheTutorial thetutorial = FindObjectOfType<TheTutorial>();
                thetutorial.StartCoroutine(thetutorial.EndTutorial());
                break;
            case 1: //첫 튜토리얼
                GameObject target = GameObject.Find("mirrordoor");
                theeffect.StartCoroutine(theeffect.Fadein(target, 0.005f));
                break;
            default:
                break;
        }
    }

    //이벤트 커맨드
    public IEnumerator PlayerMove(string _command)
    {
        /* L:left, R:right, U:up, D:down
         * 예) "LLR" 왼쪽으로 2칸 이동 후 오른쪽으로 1칸 이동
         */
        theplayer.iseventmove = true;
        for (int i = 0; i < _command.Length; i++)
        {
            switch(_command[i])
            {
                case 'L':
                    theplayer.h = -0.5f;
                    theplayer.v = 0;
                    break;
                case 'R':
                    theplayer.h = 0.5f;
                    theplayer.v = 0;
                    break;
                case 'U':
                    theplayer.v = 0.5f;
                    theplayer.h = 0;
                    break;
                case 'D':
                    theplayer.v = -0.5f;
                    theplayer.h = 0;
                    break;
            }
            this.transform.position = theplayer.transform.position;
            yield return distanceisone;
        }
        theplayer.h = 0;
        theplayer.v = 0;
        theplayer.iseventmove = false;
    }
}
