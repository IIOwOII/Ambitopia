using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public int eventnumber; //이벤트 번호
    [Tooltip("이벤트 자동실행")]
    public bool autoevent; //자동실행 or 조건실행
    [Tooltip("일회성 이벤트")]
    public bool onetimeevent; //일회성 or 다회성
    EventManager eventmanager; //이벤트 매니저 객체화

    void Awake()
    {
        eventmanager = FindObjectOfType<EventManager>();
    }

    void Start()
    {
        if (autoevent)
        {
            eventmanager.EventScene(eventnumber);
            Destroy(this.gameObject);
        }
    }
}
