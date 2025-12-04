using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Sprite itemicon; //이미지
    public string itemname; //이름
    public string itemdescription; //설명

    public Item(string _name, string _description)
    {
        itemname = _name;
        itemdescription = _description;
        itemicon = Resources.Load("ItemIcon/" + _name, typeof(Sprite)) as Sprite;
        //아이템 아이콘 이름 = 아이템 이름
    }
}
