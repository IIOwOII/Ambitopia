using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int season; //계절

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
