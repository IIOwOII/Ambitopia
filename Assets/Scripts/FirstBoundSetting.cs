using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoundSetting : MonoBehaviour
{
    [Tooltip("바운드 내부의 StartPoint 값들과 일치시키기")]
    public int[] boundnumbers;
    private BoxCollider2D bound;
    private CameraManager thecamera;
    private PlayerManager theplayer;

    void Awake()
    {
        bound = GetComponent<BoxCollider2D>();
        //CameraManager 스크립트 가진 객체(main camera) 찾기
        thecamera = FindObjectOfType<CameraManager>();
        theplayer = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        foreach (int bn in boundnumbers)
        {
            if (theplayer.towardpoint == bn)
                thecamera.SetBound(bound); //SetBound 함수 호출
        }
    }
}
