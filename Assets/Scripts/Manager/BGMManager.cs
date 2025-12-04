using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance; //BGM매니저 자기자신
    private AudioSource theaudio;
    public AudioClip[] BGMTrack;
    private WaitForSeconds fadeinterval;
    private bool isBGMchanging;
    private int playingBGMnumber;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
        theaudio = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        fadeinterval = new WaitForSeconds(0.01f);
        isBGMchanging = false;
        playingBGMnumber = 0; //현재 재생중인 BGM 트랙넘버, 0은 None, 기존 BGM의 arr버전이면 -부호를 붙임
    }

    //BGM 바꾸고 재생
    public void BGMChange(int _track, float _volume, float _pitch = 1)
    {
        if (isBGMchanging)
            StopAllCoroutines();
        StartCoroutine(SmoothBGMChange(_track, _volume, _pitch));
    }

    IEnumerator SmoothBGMChange(int _track, float _volume, float _pitch)
    {
        isBGMchanging = true;
        while (theaudio.volume > 0f)
        {
            theaudio.volume -= 0.01f;
            yield return fadeinterval;
        }
        theaudio.Stop();
        theaudio.clip = BGMTrack[_track];
        theaudio.pitch = _pitch;
        theaudio.Play();
        while (theaudio.volume < _volume)
        {
            theaudio.volume += 0.01f;
            yield return fadeinterval;
        }
        isBGMchanging = false;
    }

    //BGM 정지
    public void BGMStop()
    {
        theaudio.Stop();
    }

    //BGM 페이드 아웃
    public IEnumerator BGMFadeOut()
    {
        while (theaudio.volume > 0f)
        {
            theaudio.volume -= 0.01f;
            yield return fadeinterval;
        }
    }

    public IEnumerator BGMFadeIn(float _volume = 1f)
    {
        while (theaudio.volume < _volume)
        {
            theaudio.volume += 0.01f;
            yield return fadeinterval;
        }
    }

    //Scene이 로드될 때마다 실행되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BGMSetting(scene.buildIndex);
    }

    private void BGMSetting(int _scenenum)
    {
        if (_scenenum == 2) //BO 보이 집안
        {
            if (playingBGMnumber != -1)
            {
                BGMChange(1, 1f, 0.5f);
                playingBGMnumber = -1;
            }
        }
        if (_scenenum == 3 || _scenenum == 4) //보이의 집안
        {
            if (playingBGMnumber != -2)
            {
                BGMChange(2, 0.5f, 0.5f);
                playingBGMnumber = -2;
            }
        }
        if (_scenenum == 5 || _scenenum == 6 || _scenenum == 7 || _scenenum == 8 || _scenenum == 9) //보이 집 밖
        {
            if (playingBGMnumber != 2)
            {
                BGMChange(2, 1f);
                playingBGMnumber = 2;
            }
        }    
    }
}
