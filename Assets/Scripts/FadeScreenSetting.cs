using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScreenSetting : MonoBehaviour
{
    private Image fadepanel;
    private Color fadecolor;
    private GameObject canvasobj;
    WaitForSeconds waittime;
    public bool ismovefade;

    void Awake()
    {
        canvasobj = gameObject.transform.parent.gameObject;
        DontDestroyOnLoad(canvasobj.gameObject);
        SceneManager.sceneLoaded += MapMoving;
    }

    void Start()
    {
        fadepanel = gameObject.GetComponent<Image>();
        fadecolor = fadepanel.color;
        waittime = new WaitForSeconds(0.01f);
        ismovefade = true;
    }

    private void MapMoving(Scene scene, LoadSceneMode mode)
    {
        if (ismovefade)
            StartCoroutine(ScreenFade());
        else
            ismovefade = true; //다시 초기화
    }

    public IEnumerator ScreenFade()
    {
        fadecolor.a = 1f;
        while (fadecolor.a > 0f)
        {
            fadecolor.a -= 0.02f;
            fadepanel.color = fadecolor;
            yield return waittime;
        }
    }

    public IEnumerator ScreenFadeInAtTime(float _sec)
    {
        WaitForSeconds waittimevalue = new WaitForSeconds(_sec);
        while (fadecolor.a < 1f)
        {
            fadecolor.a += 0.01f;
            fadepanel.color = fadecolor;
            yield return waittimevalue;
        }
    }
}
