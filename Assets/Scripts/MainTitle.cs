using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTitle : MonoBehaviour
{
    public Image blackscreen;
    public GameObject startbutton;
    public BoxCollider2D firstbound;
    public Animator dooranim;
    Color color;
    PlayerManager theplayer;
    CameraManager thecamera;
    SpriteRenderer theplayerspr;
    Color playercolor;
    WaitForSeconds waittime;
    WaitForSeconds interval;
    public AudioClip[] SeasonOpening;

    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        if (!theplayer.ontitle)
            this.gameObject.SetActive(false);
        thecamera = FindObjectOfType<CameraManager>();
    }

    void Start()
    {
        //플레이어 초기 세팅
        theplayerspr = theplayer.gameObject.GetComponent<SpriteRenderer>();
        playercolor = theplayerspr.color;
        playercolor.a = 0; //플레이어 투명화
        theplayerspr.color = playercolor;
        theplayer.keygetpossible = false; //플레이어 키입력 통제
        theplayer.gameObject.transform.position = new Vector3(2, -1, 0); //플레이어 위치 고정

        waittime = new WaitForSeconds(1f);
        interval = new WaitForSeconds(0.01f);
        color = blackscreen.color;

        //카메라 초기 세팅
        thecamera.ChangeCameraSize(7.8f);
        thecamera.target = null;
        Vector3 _vec = new Vector3(0, 6.2f, -10);
        thecamera.CameraPositionSet(_vec);

        StartCoroutine(TitleLoading());
    }

    public void StartGame()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        WaitUntil waitclose = new WaitUntil(() => dooranim.GetCurrentAnimatorStateInfo(0).IsName("BW_door2_DtoU_Close") && dooranim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);
        WaitUntil waitopen = new WaitUntil(() => dooranim.GetCurrentAnimatorStateInfo(0).IsName("BW_door2_DtoU_Open") && dooranim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        yield return waittime;

        //여는 소리 + 문 열기
        SoundManager.instance.SoundPlay(2);
        dooranim.SetBool("Open", true);
        yield return waitopen;

        //블랙스크린 On
        while (color.a < 1f)
        {
            color.a += 0.005f;
            blackscreen.color = color;
            yield return interval;
        }

        //카메라 정상화
        thecamera.DefaultCameraSize();
        thecamera.target = theplayer.gameObject;
        thecamera.SetBound(firstbound);

        //플레이어 알파값 정상화
        Color playercolor = theplayerspr.color;
        playercolor.a = 1;
        theplayerspr.color = playercolor;
        yield return waittime;

        //닫는 소리 + 문 닫기 + 블랙스크린 Off
        while (color.a > 0f)
        {
            color.a -= 0.01f;
            blackscreen.color = color;
            yield return interval;
        }
        dooranim.SetBool("Open", false);
        yield return waitclose;
        SoundManager.instance.SoundPlay(3);

        //플레이어 정상화 + 타이틀 비활성화
        theplayer.ontitle = false;
        theplayer._iszone = true;
        theplayer.keygetpossible = true;
        this.gameObject.SetActive(false);
    }

    IEnumerator TitleLoading()
    {
        //데이터 로드
        GetComponent<SaveLoad>().CallLoad();
        yield return waittime; //로드 시간

        //Season에 따른 BGM 재생
        TimeManager thetime = FindObjectOfType<TimeManager>();
        AudioSource theaudiosource = GetComponent<AudioSource>();
        theaudiosource.clip = SeasonOpening[thetime.season];
        theaudiosource.Play();

        startbutton.SetActive(true); //시작 버튼 활성화
        startbutton.GetComponent<ButtonSetting>().FadeIn();

        //블랙스크린 페이드아웃
        while (color.a > 0)
        {
            color.a -= 0.01f;
            blackscreen.color = color;
            yield return interval;
        }
    }
}
