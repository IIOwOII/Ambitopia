using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//게임 실행 시, 튜토리얼 진행 담당 및 튜토리얼 실행 여부에 따라 메인 화면 호출
public class TheFirst : MonoBehaviour
{
    public SpriteRenderer blackscreen;
    public Text tutotext;
    public Image[] keypadinfo;
    public Text[] keypadtext;
    PlayerManager theplayer;
    AudioSource theaudio;
    SpriteRenderer thesprite;
    Color color1;
    Color color2;
    Color color3;
    WaitForSeconds interval;
    WaitForSeconds longinterval;
    private float soulcharge;
    private bool start;

    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        theaudio = GetComponent<AudioSource>();
        thesprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        interval = new WaitForSeconds(0.01f);
        longinterval = new WaitForSeconds(1f);
        start = false;
        theplayer.keygetpossible = false;
        soulcharge = 0;
        color1 = thesprite.color;
        color2 = blackscreen.color;
        color3 = tutotext.color;
        StartCoroutine(Waiting());
    }

    void Update()
    {
        if (start)
        {
            if (soulcharge < 1)
            {
                theaudio.pitch = soulcharge;
                theaudio.volume = soulcharge;
                color1.a = 1 - soulcharge;
                color2.a = 1 - soulcharge;
                thesprite.color = color1;
                tutotext.color = color1;
                blackscreen.color = color2;
            }
            else
            {
                StartCoroutine(Introduce());
                start = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (start)
        {
            if (Input.GetMouseButton(0))
                soulcharge += 0.01f;
        }
    }

    IEnumerator Waiting()
    {
        while (color3.a < 1)
        {
            color3.a += 0.01f;
            tutotext.color = color3;
            yield return interval;
        }
        yield return longinterval;
        start = true;
    }

    IEnumerator Introduce()
    {
        color3.a = 0;
        color3.r = 0;
        color3.g = 0;
        color3.b = 0;
        tutotext.color = color3;
        yield return longinterval;
        SoundManager.instance.SoundPlay(0);
        tutotext.text = "그는 당신의 도움없이는 움직이지도, 결정하지도 못합니다.";
        yield return StartCoroutine(Fadeinout());
        SoundManager.instance.SoundPlay(0);
        tutotext.text = "운명은 당신만이 결정할 수 있습니다.";
        yield return StartCoroutine(Fadeinout());
        SoundManager.instance.SoundPlay(0);
        tutotext.text = "이제, 그의 어렴풋한 기억이 당신을 인도합니다.";
        yield return StartCoroutine(Fadeinout());
        SoundManager.instance.SoundPlay(0);
        tutotext.text = "당신이 꿈꾸던 세상을 실현시키세요.";
        yield return StartCoroutine(Fadeinout());
        theplayer.keygetpossible = true;
        yield return StartCoroutine(Fadeinout2());
        this.enabled = false;
    }

    IEnumerator Fadeinout()
    {
        while (color3.a < 1)
        {
            color3.a += 0.01f;
            tutotext.color = color3;
            yield return interval;
        }
        yield return longinterval;
        while (color3.a > 0)
        {
            color3.a -= 0.01f;
            tutotext.color = color3;
            yield return interval;
        }
        yield return longinterval;
    }

    IEnumerator Fadeinout2()
    {
        while (color1.a < 1 || color3.a < 1)
        {
            color1.a += 0.01f;
            color3.a += 0.01f;
            for (int i = 0; i < 6; i++)
                keypadinfo[i].color = color1;
            for (int j = 0; j < 3; j++)
                keypadtext[j].color = color3;
            yield return interval;
        }
        yield return longinterval;
        while (color1.a > 0 || color3.a > 0)
        {
            color1.a -= 0.01f;
            color3.a -= 0.01f;
            for (int i = 0; i < 6; i++)
                keypadinfo[i].color = color1;
            for (int i = 0; i < 3; i++)
                keypadtext[i].color = color3;
            yield return interval;
        }
        yield return longinterval;
    }
}
