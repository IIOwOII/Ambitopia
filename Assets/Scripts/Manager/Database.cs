using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public Dictionary<int, Item> itemdata;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        itemdata = new Dictionary<int, Item>();
        GenerateItemData();
    }

    //모든 아이템 목록
    void GenerateItemData()
    {
        //(ID, (이름, 설명))
        itemdata.Add(100, new Item("실험", "실험데이터"));
    }
}
