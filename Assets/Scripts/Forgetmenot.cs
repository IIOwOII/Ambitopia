using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

//플레이어 환생과 최초 시작에 관한 스크립트
public class Forgetmenot : MonoBehaviour
{
    [System.Serializable]
    public class Birth
    {
        public bool isfirst; //최초 실행 여부
    }

    PlayerManager theplayer;

    public Birth birth;

    public bool issavemode; //Loading Scene이라면 false, 그 외는 true

    void Awake()
    {
		DontDestroyOnLoad(this.gameObject);
        if (!issavemode)
            StartCoroutine(Loading());
    }

    public void IsfirstSet(bool _isfirst)
    {
        birth.isfirst = _isfirst;
        BirthSave();
    }

    public void BirthSave()
    {
        //세이브 과정
        BinaryFormatter bf = new BinaryFormatter(); //변환기
        FileStream file = File.Create(Application.persistentDataPath + "/Birth"); //세이브 파일
        bf.Serialize(file, birth); //변환
        file.Close();
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(1f);
        theplayer = FindObjectOfType<PlayerManager>();
        BinaryFormatter bf = new BinaryFormatter();
        FileInfo fileinfo = new FileInfo(Application.persistentDataPath + "/Birth");
        if (fileinfo.Exists) //Birth 파일이 있을 경우
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Birth", FileMode.Open);
            birth = (Birth)bf.Deserialize(file); //변환
            if (!birth.isfirst) //isfirst가 false [타이틀로 이동] true이면 환생되고 처음 실행
            {
                theplayer.ontitle = true;
                SceneManager.LoadScene("BW_boyhouseyard");
            }
            file.Close();
        }
        else //Birth 파일이 없을 경우 [온전한 최초 실행]
        {
            theplayer.towardpoint = 0;
            SceneManager.LoadScene("M_tutorial");
        }
		issavemode = true; // 한 번 로딩완료 했으니 true로 전환
    }
}
