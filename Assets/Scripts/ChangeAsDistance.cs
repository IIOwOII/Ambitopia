using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAsDistance : MonoBehaviour
{
    GameObject thetarget;
    PlayerManager theplayer;
    private Color thecolor;
    private SpriteRenderer thesprite;
    private float distance;

    void Awake()
    {
        theplayer = FindObjectOfType<PlayerManager>();
        thetarget = theplayer.gameObject;
    }

    void Start()
    {
        thesprite = GetComponent<SpriteRenderer>();
        thecolor = thesprite.color;
    }

    void Update()
    {
        distance = (thetarget.transform.position - this.transform.position).sqrMagnitude;
        thecolor.a = 1 - distance / 10;
        thesprite.color = thecolor;
    }
}
