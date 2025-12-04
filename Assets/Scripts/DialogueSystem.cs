using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text talktext; //대화창 텍스트
    public Text selecttext; //선택창 질문 텍스트
    public Text[] selectiontext; //선택지 텍스트
    public GameObject panel; //대화창
    public GameObject selectpanel; //선택창 질문
    public GameObject[] selectionpanel; //선택지
    private Animator panelanim; //대화창의 애니메이터
    private Animator selectpanelanim; //선택창의 애니메이터
    DialogueManager thedialogue; //대화 매니저 생성
    PlayerManager theplayer; //플레이어 객체화
    EventManager theevent; //이벤트 매니저 객체화
    public int[] npcbranch; //NPC 분기점
    public int[] npcprogress; //NPC 진행도
    public int[] npcrand; //NPC 랜덤수
    //대화 변수
    private int thekey_npcnum;
    private int thekey_npcbranch;
    private int thekey_npcprogress;
    private int thekey_npcrand;
    private string thekey;
    private int indexvalue;
    private bool istyping;
    WaitUntil waitzget;
    WaitUntil waitpaneloff;
    WaitUntil waitselectpaneloff_2;
    WaitForSeconds waittime;
    WaitForSeconds typingdelay;
    private IEnumerator typingcoroutine;
    private Image selectedpanel;
    private Color selectedpanelcolor;
    private string sentencememory; //Talk 함수 중간자 역할
    private string[] sentencearray;
    private int selecting; //선택지 고르는 중(선택지 수와 같은 값으로 맞추기)
    private bool selectchange;
    private int selectreturn; //선택한 값
    //타이핑 사운드
    AudioSource selecttypingsound;

    void Awake()
    {
        thedialogue = gameObject.GetComponent<DialogueManager>();
        theplayer = FindObjectOfType<PlayerManager>();
        theevent = FindObjectOfType<EventManager>();
        panelanim = panel.GetComponent<Animator>();
        selectpanelanim = selectpanel.GetComponent<Animator>();
    }

    void Start()
    {
        npcbranch = new int[3] { 0, 0, 0 };
        npcprogress = new int[3] { 0, 0, 0 }; //추후 수정 //npc 수+1 만큼 배열의 크기 늘어남 //세이브데이터에서 정보 불러오기 구현해야함
        npcrand = new int[3] { 0, 0, 0 };
        panel.SetActive(false);
        indexvalue = 0;
        istyping = false;
        selecting = 0;
        selectchange = false;
        selectedpanel = null;
        waitzget = new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        waitpaneloff = new WaitUntil(() => panelanim.GetCurrentAnimatorStateInfo(0).IsName("DialogueOff") && panelanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        waitselectpaneloff_2 = new WaitUntil(() => selectpanelanim.GetCurrentAnimatorStateInfo(0).IsName("2SelectOff") && selectpanelanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        waittime = new WaitForSeconds(0.5f);
        typingdelay = new WaitForSeconds(0.05f);
        sentencearray = new string[2];
        //타이핑 사운드 할당
        selecttypingsound = this.transform.Find("SelectTyping").GetComponent<AudioSource>();
    }

    void Update()
    {
        //선택지 고르는 중
        if (selecting != 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && selectreturn < (selecting - 1))
            {
                selectchange = true;
                selectreturn += 1;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && selectreturn > 0)
            {
                selectchange = true;
                selectreturn -= 1;
            }
            if (selectchange)
            {
                selecttypingsound.Play();
                if (selectedpanel != null)
                {
                    selectedpanelcolor.r = 1f;
                    selectedpanelcolor.g = 1f;
                    selectedpanelcolor.b = 1f;
                    selectedpanel.color = selectedpanelcolor;
                }
                StartCoroutine(SelectionChange());
                selectchange = false;
            }
        }
    }

    public void Action(GameObject _npcscaned) //NPC 조사
    {
        if (_npcscaned != null)
            thekey_npcnum = _npcscaned.GetComponent<NPCInformation>().npcnumber;
        else
            thekey_npcnum = 0;
        thekey_npcbranch = npcbranch[thekey_npcnum];
        thekey_npcprogress = npcprogress[thekey_npcnum];
        thekey_npcrand = npcrand[thekey_npcnum];
        thekey = thekey_npcnum.ToString() + ":" + thekey_npcbranch.ToString() + ":" + thekey_npcprogress.ToString() + ":" + thekey_npcrand.ToString();
        switch (thedialogue.dialoguedata[thekey][0].Split('|')[0])
        {
            case "Text": //첫 대사
                panel.SetActive(true);
                break;
            case "Select": //첫 선택지
                selectpanel.SetActive(true);
                break;
        }
        Talk(thekey); //Key를 Talk 함수로 전달
        theplayer.keygetpossible = false; //플레이어 키입력 통제
    }

    private void Talk(string _thekey)
    {
        if (indexvalue == thedialogue.dialoguedata[_thekey].Length) //다음 대화 내용 없을 경우
        {
            indexvalue = 0; //index를 다시 초기화
            if (panel.activeSelf == true) //마지막 단계가 대사일때
                StartCoroutine(ExitDialogue());
            else if (selectpanel.activeSelf == true) //마지막 단계가 선택지일때
                StartCoroutine(SelectionEnd());
            theplayer.keygetpossible = true; //플레이어 키입력 통제 해제
        }
        else
        {
            sentencearray = thedialogue.dialoguedata[_thekey][indexvalue].Split('|');
            switch (sentencearray[0])
            {
                case "Text": //대사
                    talktext.text = ""; //대화창 초기화
                    if (selectpanel.activeSelf == true) //이전 단계가 선택지일때
                    {
                        selectpanel.SetActive(false);
                        panel.SetActive(true);
                    }
                    sentencememory = sentencearray[1]; //중간자에 문장 할당
                    typingcoroutine = Typing(sentencememory);
                    StartCoroutine(typingcoroutine); //대화 내용 표시(Typing)
                    indexvalue++; //index 증가
                    StartCoroutine(NextSentence()); //다음 대화 대기
                    break;

                case "Select": //선택지
                    selecttext.text = ""; //선택창 질문 초기화
                    if (panel.activeSelf == true) //이전 단계가 대사일때
                    {
                        panel.SetActive(false);
                        selectpanel.SetActive(true);
                    }
                    StartCoroutine(SelectionStart(2));
                    break;

                case "Command": //명령어
                    switch (sentencearray[1])
                    {
                        case "Stop": //대화 종료
                            indexvalue = thedialogue.dialoguedata[_thekey].Length - 1;
                            break;
                        case "Progress": //NPC 진행도 +1
                            npcprogress[thekey_npcnum]++;
                            break;
                        case "Randplus": //NPC 랜덤수 +1
                            npcrand[thekey_npcnum]++;
                            break;
                        case "Randreset": //NPC 랜덤수 =0
                            npcrand[thekey_npcnum] = 0;
                            break;
                        default: //그 외
                            switch(sentencearray[1].Split(':')[0])
                            {
                                case "Branch": //NPC 분기 +n
                                    npcbranch[thekey_npcnum] += int.Parse(sentencearray[1].Split(':')[1]);
                                    break;
                                case "Event": //이벤트 매니저 실행
                                    theevent.EventScene(int.Parse(sentencearray[1].Split(':')[1]));
                                    break;
                            }
                            break;
                    }
                    indexvalue++;
                    Talk(thekey);
                    break;
            }
        }
    }

    //NPC 관련 변수 지정
    public void NPCValueSet(string _valueset)
    {
        //NPCValueSet("0:0:0:0") //0번 NPC의 branch, progress, rand를 모두 0으로 바꿈
        int _npcnum = int.Parse(_valueset.Split(':')[0]);
        npcbranch[_npcnum] = int.Parse(_valueset.Split(':')[1]);
        npcprogress[_npcnum] = int.Parse(_valueset.Split(':')[2]);
        npcrand[_npcnum] = int.Parse(_valueset.Split(':')[3]);
    }

    IEnumerator Typing(string _sentence)
    {
        istyping = true;
        for (int i = 0; i < _sentence.Length; i++)
        {
            talktext.text += _sentence[i];
            yield return typingdelay;
        }
        istyping = false;
    }

    IEnumerator NextSentence()
    {
        yield return waittime; //Z키 입력 전 0.5초 대기
        yield return waitzget; //Z키 입력 대기
        if (istyping) //아직 Typing 실행 중일때
        {
            StopCoroutine(typingcoroutine); //Typing 즉시 정지
            talktext.text = sentencememory;
            yield return waittime;
            yield return waitzget; //Z키 입력 재대기
        }
        Talk(thekey);
    }

    IEnumerator ExitDialogue()
    {
        panelanim.SetBool("beoff", true); //대화창 종료 애니메이션
        yield return waitpaneloff;
        panel.SetActive(false);
    }

    IEnumerator SelectionStart(int _selectionnumber)
    {
        selectpanelanim.SetInteger("selectionnumber", 0);
        for (int i = 0; i < sentencearray[1].Split('/')[0].Length; i++)
        {
            selecttext.text += sentencearray[1].Split('/')[0][i]; //질문 출력
            yield return typingdelay;
        }
        selectpanelanim.SetInteger("selectionnumber", _selectionnumber);
        for (int j = 0; j < _selectionnumber; j++)
        {
            selectiontext[j].text = sentencearray[1].Split('/')[j + 1]; //선택지 출력
        }
        selectreturn = 0;
        selectchange = true;
        selecting = _selectionnumber;
        yield return waitzget;
        selecttypingsound.Play();
        selecting = 0;
        indexvalue += int.Parse(sentencearray[1].Split('/')[selectreturn + 1].Split(':')[1]);
        Talk(thekey);
    }

    IEnumerator SelectionEnd()
    {
        selectpanelanim.SetBool("beoff", true); //선택창 종료 애니메이션
        yield return waitselectpaneloff_2;
        selectpanel.SetActive(false);
    }

    IEnumerator SelectionChange()
    {
        selectedpanel = selectionpanel[selectreturn].GetComponent<Image>();
        selectedpanelcolor = selectedpanel.color;
        selectedpanelcolor.r = 0.5f;
        selectedpanelcolor.g = 0.5f;
        selectedpanelcolor.b = 0.5f;
        selectedpanel.color = selectedpanelcolor;
        yield return null;
    }
}
