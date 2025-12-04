using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSprite : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 rayvec;
    SpriteRenderer spr;
    Color color;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        rayvec = Vector3.up;
        color = spr.color;
    }

    void FixedUpdate()
    {
        //위에 player있는지 확인
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, rayvec, 1f, LayerMask.GetMask("Player"));
        if (rayhit.collider != null)
        {
            //반투명화
            if (color.a > 0.5f)
            {
                color.a -= 0.02f;
                spr.color = color;
            }
        }
        else
        {
            //불투명화
            if (color.a < 1f)
            {
                color.a += 0.02f;
                spr.color = color;
            }
        }
            
    }
}
