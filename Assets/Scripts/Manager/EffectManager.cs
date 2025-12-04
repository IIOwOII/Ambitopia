using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject theloading;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //_speed의 빠르기로 _targetobj 페이드인 효과 부여
    public IEnumerator Fadein(GameObject _targetobj, float _speed = 0.02f)
    {
        SpriteRenderer targetsprite = _targetobj.GetComponent<SpriteRenderer>();
        Color color = targetsprite.color;
        WaitForSeconds waittime = new WaitForSeconds(0.01f);
        while (color.a > 0f)
        {
            color.a -= _speed;
            targetsprite.color = color;
            yield return waittime;
        }
    }

    //_speed의 빠르기로 _targetobj 페이드아웃 효과 부여
    public IEnumerator Fadeout(GameObject _targetobj, float _speed = 0.02f)
    {
        SpriteRenderer targetsprite = _targetobj.GetComponent<SpriteRenderer>();
        Color color = targetsprite.color;
        WaitForSeconds waittime = new WaitForSeconds(0.01f);
        while (color.a < 1f)
        {
            color.a += _speed;
            targetsprite.color = color;
            yield return waittime;
        }
    }

    public void LoadingSignDisplay(bool _active)
    {
        theloading.SetActive(_active);
    }
}
