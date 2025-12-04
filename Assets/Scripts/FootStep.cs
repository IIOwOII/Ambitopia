using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FootStep : MonoBehaviour
{
    [Tooltip("0: 목재, 1:잔디, 2:모래")][Range(0,2)]
    public int regionvalue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerPosition"))
        {
            PlayerManager theplayer = FindObjectOfType<PlayerManager>();
            theplayer._regionvalue = regionvalue * 3;
        }
    }
}
