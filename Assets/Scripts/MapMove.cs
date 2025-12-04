using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMove : MonoBehaviour
{
    public string transfermapname; //이동할 맵의 이름
    public int warppoint; //이동 위치
    private PlayerManager theplayer; //플레이어 객체 지정
    private EffectManager theeffect; //이펙트 객체 지정
    private FadeScreenSetting thefade; //스크린 페이드 객체 지정
    
    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        theeffect = FindObjectOfType<EffectManager>();
        thefade = FindObjectOfType<FadeScreenSetting>();
    }

    IEnumerator DoorOpen()
    {
        theplayer.towardpoint = warppoint;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(transfermapname);
    }

    //문이 아닐때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player(boy)")
        {
            theplayer.towardpoint = warppoint;
            SceneManager.LoadScene(transfermapname);
        }
    }
}
