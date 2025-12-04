using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; //사운드매니저 자기자신
    [Tooltip("단발성 효과음")]
    public AudioClip[] track;
    [Tooltip("자주 쓰는 효과음")]
    public AudioClip[] UsualTrack;
    [Tooltip("중첩 가능한 유사 BGM")]
    public AudioClip[] MetaBGMTrack;
    private GameObject temporarysoundobject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //자주 쓰는 효과음 매개 오브젝트 유지
        for (int i = 0; i < UsualTrack.Length; i++)
        {
            GameObject usualsoundobject = new GameObject(UsualTrack[i].name);
            usualsoundobject.transform.SetParent(this.transform);
            AudioSource usualsoundsource = usualsoundobject.AddComponent<AudioSource>();
            usualsoundsource.clip = UsualTrack[i];
            usualsoundsource.loop = false;
            usualsoundsource.playOnAwake = false;
        }
    }

    public void SoundPlay(int _track)
    {
        GameObject soundobject = new GameObject("TrackNumber_" + _track); //매개 오브젝트 생성
        soundobject.tag = "SoundObject";
        soundobject.transform.SetParent(this.transform); //사운드매니저 오브젝트에 소속
        AudioSource soundsource = soundobject.AddComponent<AudioSource>();
        soundsource.clip = track[_track];
        soundsource.loop = false;
        soundsource.Play();
        Destroy(soundobject, track[_track].length); //효과음 종료 후, 매개 오브젝트 파괴
    }

    public void MetaBGMPlay(int _metaBGMtrack, float _volume)
    {
        GameObject soundobject = new GameObject("MetaBGMNumber_" + _metaBGMtrack); //매개 오브젝트 생성
        soundobject.tag = "SoundObject";
        soundobject.transform.SetParent(this.transform); //사운드매니저 오브젝트에 소속
        AudioSource soundsource = soundobject.AddComponent<AudioSource>();
        soundsource.clip = MetaBGMTrack[_metaBGMtrack];
        soundsource.loop = true;
        soundsource.volume = _volume;
        soundsource.Play();
    }

    public void MetaBGMStop(int _metaBGMtrack)
    {
        Destroy(this.transform.Find("MetaBGMNumber_" + _metaBGMtrack));
    }
}
