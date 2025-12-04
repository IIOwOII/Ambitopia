using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGscroll : MonoBehaviour
{
    private MeshRenderer render;
    private float offsetval;
    public float speed;

    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    
    void Update()
    {
        offsetval += Time.deltaTime * speed;
        render.material.mainTextureOffset = new Vector2(0, offsetval);
    }
}
