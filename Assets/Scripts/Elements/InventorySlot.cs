using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Database thedatabase;
    Inventory theinventory;
    public GameObject slot;
    public Text itemdescription;
    private int totalcount;
    private int indexvalue;
    private bool indexchange;
    private GameObject selectedslot;
    private Color selectedslotcolor;
    private List<int> itemidinslot;

    void Awake()
    {
        thedatabase = FindObjectOfType<Database>();
        theinventory = FindObjectOfType<Inventory>();
    }

    void Start()
    {
        itemidinslot = new List<int>();
        indexvalue = 0;
        totalcount = 0;
        indexchange = false;

        //슬롯 생성
        foreach (int _id in theinventory.playeritemlist.Keys)
        {
            GameObject temslot = Instantiate(slot);
            temslot.name = "Slot" + indexvalue.ToString();
            temslot.transform.Find("Icon").GetComponent<Image>().sprite = thedatabase.itemdata[_id].itemicon;
            temslot.transform.Find("Name").GetComponent<Text>().text = thedatabase.itemdata[_id].itemname;
            temslot.transform.Find("Count").GetComponent<Text>().text = "(" + theinventory.playeritemlist[_id].ToString() + ")";
            itemidinslot.Add(_id);
            totalcount++;
        }

        selectedslotcolor.r = 0.5f;
        selectedslotcolor.g = 0.5f;
        selectedslotcolor.b = 0.5f;
        selectedslot = this.transform.Find("Slot0").gameObject;
        selectedslot.GetComponent<Image>().color = selectedslotcolor;
        itemdescription.text = thedatabase.itemdata[itemidinslot[indexvalue]].itemdescription;
    }

    void Update()
    {
        //조작
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (indexvalue < totalcount - 1)
            {
                //소리 추가하기
                indexvalue++;
                indexchange = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (indexvalue > 0)
            {
                //소리 추가하기
                indexvalue--;
                indexchange = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            theinventory.UseItem(itemidinslot[indexvalue]);
        }

        //슬롯 색
        if (indexchange)
        {
            selectedslotcolor.r = 1f;
            selectedslotcolor.g = 1f;
            selectedslotcolor.b = 1f;
            selectedslot.GetComponent<Image>().color = selectedslotcolor;
            selectedslotcolor.r = 0.5f;
            selectedslotcolor.g = 0.5f;
            selectedslotcolor.b = 0.5f;
            selectedslot = this.transform.Find("Slot" + indexvalue.ToString()).gameObject;
            selectedslot.GetComponent<Image>().color = selectedslotcolor;
            itemdescription.text = thedatabase.itemdata[itemidinslot[indexvalue]].itemdescription;
            indexchange = false;
        }
    }
}
