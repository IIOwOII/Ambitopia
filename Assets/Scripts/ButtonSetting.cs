using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour
{
    public void TitleButton()
    {
        MainTitle thetitle = FindObjectOfType<MainTitle>();
        GetComponent<Button>().interactable = false;
        StartCoroutine(ButtonFadeOut());
        thetitle.StartGame();
    }

    //버튼 페이드 아웃
    IEnumerator ButtonFadeOut()
    {
        Image buttonspr = GetComponent<Image>();
        Color color = buttonspr.color;
        WaitForSeconds waittime = new WaitForSeconds(0.01f);
        while (color.a > 0)
        {
            color.a -= 0.01f;
            buttonspr.color = color;
            yield return waittime;
        }
    }

    public void FadeIn()
    {
        StartCoroutine(ButtonFadeIn());
    }

    //버튼 페이드 인
    IEnumerator ButtonFadeIn()
    {
        Image buttonspr = GetComponent<Image>();
        Color color = buttonspr.color;
        WaitForSeconds waittime = new WaitForSeconds(0.01f);
        while (color.a < 1)
        {
            color.a += 0.01f;
            buttonspr.color = color;
            yield return waittime;
        }
    }
}
