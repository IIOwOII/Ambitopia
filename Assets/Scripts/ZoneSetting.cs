using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSetting : MonoBehaviour
{
    PlayerManager theplayer;
    [Tooltip("발걸음 소리 on: true")]
    public bool iszone;

    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        theplayer._iszone = iszone;
    }
}
