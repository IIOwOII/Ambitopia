using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundMove : MonoBehaviour
{
    public BoxCollider2D newbound; //콜라이더에 들어가면 적용할 바운드
    private CameraManager thecamera;
    private BoxCollider2D triggerbound; //trigger 역할을 하는 바운드

    void Awake()
    {
        triggerbound = GetComponent<BoxCollider2D>();
        //CameraManager 스크립트 가진 객체(main camera) 찾기
        thecamera = FindObjectOfType<CameraManager>();
    }

    void Update()
    {
        if (thecamera.bound == newbound)
            triggerbound.enabled = false;
        else
            triggerbound.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerPosition"))
            thecamera.SetBound(newbound);
    }
}
