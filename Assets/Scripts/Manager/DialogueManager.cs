using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //딕셔너리<[npc번호,npc진행도], 대화+초상화>
    public Dictionary<string, string[]> dialoguedata;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        dialoguedata = new Dictionary<string, string[]>();
        GenerateData();
    }

    //게임 전체 대화 내용 생성 및 저장
    void GenerateData()
    {
        /*
         * key = "NPC번호:분기:진행도:랜덤수"
         * Text|대사:초상화번호
         * Select|질문/선택1:index/선택2:index/(선택3:index)/(선택4:index) //여기서 index는 indexvalue에 추가할 수
         * Command|함수
         */

        /*
         * Command 종류
         * Branch:n // 해당 NPC 분기 +n
         * Stop // 대화 종료
         * Progress // 해당 NPC 진행도 +1
         * Randplus // 해당 NPC 랜덤수 +1
         * Randreset // 해당 NPC 랜덤수 =0
         * 
         */

        /* 예시)
         * key = "2:0:0:0"
         * 내용 = Select|저기에 씨앗이 있는데 물 좀 뿌래줄래?/그래:1/싫어:7
         *(case 그래) Text|좋아!:2
         *            Text|그럼 여기 물뿌리개:1
         *            Command|item+1
         *            Text|물뿌리개를 얻었다:0
         *            Command|Progress
         *            Command|Stop
         *(case 싫어) Text|그래? 싫음 말고:3
         *            Command|랜덤수+1
         */

        //0번 NPC: 독백
        dialoguedata.Add("0:0:0:0", new string[] { "Text|(불을 키니 날 선 소리는 사라지고 익숙한 풍경이 펼쳐졌다.)", "Text|(분명히 내 집인 것 같다.)" });
        dialoguedata.Add("0:0:0:1", new string[] { "Text|(어렴풋한 기억으로 이 곳이 서재임을 알아냈다.)", "Text|(나의 여행을 기억하고 싶을 때마다 줄곧 일기를 쓰곤 했다.)" });
        dialoguedata.Add("0:0:0:10", new string[] { "Select|이제 밖으로 나갈까?/나가자:1/조금만 더 둘러보자:2", "Command|Event:0" });

        //1번 NPC: 푸리
        dialoguedata.Add("1:0:0:0", new string[] { "Text|(이 잔디밭에서 알 수 없는 위화감이 느껴진다.)", "Text|(하지만 왜인지 낯설지 않다.)" });

        //2번 NPC: 피아
    }
}
