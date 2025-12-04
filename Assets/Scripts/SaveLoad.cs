using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        //Manager 변수
        public string eventmanifest; //일회성 이벤트 진행 여부
        public int season; //계절

        //NPC
        public int[] npcbranch; //NPC 분기점
        public int[] npcprogress; //NPC 진행도
        public int[] npcrand; //NPC 랜덤수

        //플레이어 스텟
        //public int playerrebirth; //환생 수
        public int playerlevel; //레벨
        public int playermaxlife; //최대 생명력
        public int playermaxmp; //최대 마나
        public int playeratk; //기본 공격력
        public int playerdef; //기본 방어력
        public int playersnt; //기본 정신력
        public int playeragi; //기본 민첩성
        public int playerdex; //기본 기교성
        public int playerreg; //기본 재생력
        public int playermaxexp; //최대 경험치
        public int playerexp; //현재 경험치

        //장비상태
        //인벤토리
        public List<int> playeritemlist_id; //소지 아이템 id값
        public List<int> playeritemlist_count; //소지 아이템 개수
    }   

    public Data data;

    EventManager theevent;
    TimeManager thetime;

    DialogueSystem thedialoguesys;
    Inventory theinventory;

    //세이브
    public void CallSave()
    {
        theevent = FindObjectOfType<EventManager>();
        thetime = FindObjectOfType<TimeManager>();
        thedialoguesys = FindObjectOfType<DialogueSystem>();
        theinventory = FindObjectOfType<Inventory>();

        #region 세이브할 변수 (미완)
        data.eventmanifest = theevent.eventmanifest;
        data.season = thetime.season;

        data.npcbranch = thedialoguesys.npcbranch;
        data.npcprogress = thedialoguesys.npcprogress;
        data.npcrand = thedialoguesys.npcrand;

        data.playeritemlist_id = new List<int>(theinventory.playeritemlist.Keys);
        data.playeritemlist_count = new List<int>(theinventory.playeritemlist.Values);
        #endregion

        //세이브 과정
        BinaryFormatter bf = new BinaryFormatter(); //변환기
        FileStream file = File.Create(Application.dataPath + "/Diary"); //세이브 파일
        bf.Serialize(file, data); //변환
        file.Close();
    }

    //로드
    public void CallLoad()
    {
        theevent = FindObjectOfType<EventManager>();
        thetime = FindObjectOfType<TimeManager>();
        thedialoguesys = FindObjectOfType<DialogueSystem>();

        BinaryFormatter bf = new BinaryFormatter();
        FileInfo fileinfo = new FileInfo(Application.dataPath + "/Diary");

        if (fileinfo.Exists) //세이브 파일이 있을 경우
        {
            FileStream file = File.Open(Application.dataPath + "/Diary", FileMode.Open);
            data = (Data)bf.Deserialize(file); //변환

            #region 로드할 변수 (미완)
            theevent.eventmanifest = data.eventmanifest;
            thetime.season = data.season;

            thedialoguesys.npcbranch = data.npcbranch;
            thedialoguesys.npcprogress = data.npcprogress;
            thedialoguesys.npcrand = data.npcrand;

            theinventory.playeritemlist = data.playeritemlist_id.Zip(data.playeritemlist_count, (i, c) => new { i, c }).ToDictionary(x => x.i, x => x.c);
            #endregion

            file.Close();
        }
        //세이브 파일만 의도적으로 삭제된 경우 이스터에그 추가하기(미완)
    }
}
