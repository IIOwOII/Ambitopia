using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    public Dictionary<int, int> playeritemlist;

    void Start()
    {
        //아이템 id, 아이템 개수
        playeritemlist = new Dictionary<int, int>();
    }

    //인벤토리 아이템 추가
    public void AddItem(int _id, int _count = 1)
    {
        //인벤토리에 없는 아이템 습득 시 인벤토리에 새로 추가
        if (!playeritemlist.ContainsKey(_id))
            playeritemlist.Add(_id, 0);
        //얻는 개수만큼 count 늘리기
        playeritemlist[_id] += _count;
    }

    //인벤토리 아이템 제거 및 감소
    public void RemoveItem(int _id, int _count = 1)
    {
        if (playeritemlist[_id] >= _count)
        {
            //count만큼 아이템 감소
            playeritemlist[_id] -= _count;
            //남은 개수가 0개면 인벤토리에서 삭제
            if (playeritemlist[_id] == 0)
                playeritemlist.Remove(_id);
        }
        else //감소하는 개수만큼 아이템이 충분하지 않을 때
        {
            //추가하기(미완)
        }
    }

    //인벤토리 아이템 사용
    public void UseItem(int _id, int _count = 1)
    {
        //아이템 종류 따라 함수 달라짐
    }
}
