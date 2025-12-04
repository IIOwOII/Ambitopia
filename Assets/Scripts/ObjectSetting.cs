using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSetting : MonoBehaviour
{
    public int objectnumber; //오브젝트 번호
    private PlayerManager theplayer; //플레이어 객체화
    private DialogueSystem thedialoguesys; //대화 시스템 객체화
    private FadeScreenSetting thefade; //페이드 매니저 객체화

    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        thedialoguesys = FindObjectOfType<DialogueSystem>();
        thefade = FindObjectOfType<FadeScreenSetting>();
    }

    public void ObserveObject(int _objectnumber)
    {
        //현관문 최초 상호작용
        if (_objectnumber == 0)
        {
            thedialoguesys.NPCValueSet("0:0:0:10");
            thedialoguesys.Action(null);
        }

        //BO스위치
        if (_objectnumber == 1)
        {
            thefade.ismovefade = false; //페이드 연출 해제
            theplayer.towardpoint = 3;
            BGMManager.instance.BGMStop();
            SoundManager.instance.SoundPlay(1);
            SceneManager.LoadScene("BW_boyhouse");
        }
    }
}
