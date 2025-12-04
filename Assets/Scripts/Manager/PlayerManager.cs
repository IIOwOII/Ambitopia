using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    DialogueSystem thedialoguesys; //대화 시스템 객체화
    ObjectSetting theobject; //조사된 오브젝트 스크립트 객체화
    public GameObject themenu; //메뉴 객체화
    public int towardpoint; //워프 번호 저장
    public float Speed;
    private int runval;
    public bool keygetpossible;
    public bool iseventmove;
    //발걸음 소리
    public float walkvalue;
    private int temp;
    public AudioClip[] footsteps;
    public int _regionvalue;
    public bool _iszone; //영역 안에 들어와있는지 확인 [발걸음 소리 유무]
    //방향 벡터
    Vector3 rayvec;
    //조사
    GameObject hitscaned;
    GameObject objscaned;
    GameObject doorscaned;
    GameObject npcscaned;
    Animator dooranimator;
    Vector2 posadj = new Vector2(0,-0.6f); //조사 중심점 조정
    //타이틀 표시 유무
    public bool ontitle;

    Rigidbody2D rigid;
    public float h;
    public float v;
    private Animator animator;
    private AudioSource audiosource;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        thedialoguesys = FindObjectOfType<DialogueSystem>();
    }

    void Start()
    {
        keygetpossible = true;
        iseventmove = false;
        walkvalue = 0;
    }

    void Update()
    {
        //움직임 통제
        if (keygetpossible && !iseventmove)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        else if (!keygetpossible && !iseventmove)
        {
            h = 0;
            v = 0;
        }

        //달리기
        if (!iseventmove)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                runval = 2;
            else
                runval = 1;
        }

        //걷기모션 조정
        if (animator.GetFloat("DirX") != h)
        {
            animator.SetBool("isChange", true);
            animator.SetFloat("DirX", h);
        }
        else if (animator.GetFloat("DirY") != v)
        {
            animator.SetBool("isChange", true);
            animator.SetFloat("DirY", v);
        }
        else
            animator.SetBool("isChange", false);

        if (h == 0 && v == 0)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);

        //방향벡터 조정
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_down"))
            rayvec = Vector3.down;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_left"))
            rayvec = Vector3.left;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_up"))
            rayvec = Vector3.up;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking_right"))
            rayvec = Vector3.right;

        if (keygetpossible && !iseventmove)
        {
            //조사 액션
            if (Input.GetKeyDown(KeyCode.Z) && objscaned != null)
            {
                theobject = objscaned.GetComponent<ObjectSetting>();
                theobject.ObserveObject(theobject.objectnumber);
            }
            //문 액션
            if (Input.GetKeyDown(KeyCode.Z) && doorscaned != null)
            {
                StartCoroutine(MovePause(1f));
                dooranimator = doorscaned.GetComponent<Animator>();
                dooranimator.SetBool("Open", true);
                doorscaned.GetComponent<MapMove>().StartCoroutine("DoorOpen");
            }
            //NPC 액션
            if (Input.GetKeyDown(KeyCode.Z) && npcscaned != null)
            {
                thedialoguesys.Action(npcscaned);
            }
            //메뉴 실행
            if (Input.GetKeyDown(KeyCode.X) && themenu.activeSelf == false)
            {
                keygetpossible = false;
                themenu.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        //움직임
        rigid.linearVelocity = new Vector2(h, v) * Speed * runval;

        //조사된 물체 저장
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position + posadj, rayvec, 1f, 1<<8|1<<10|1<<11);
        if (rayhit.collider != null)
        {
            hitscaned = rayhit.collider.gameObject;
            if (hitscaned.layer == 8)
                objscaned = hitscaned; //Object 추적
            if (hitscaned.layer == 10)
                doorscaned = hitscaned; //Door 추적
            if (hitscaned.layer == 11)
                npcscaned = hitscaned; //NPC 추적
        }
        else
        {
            hitscaned = null;
            objscaned = null;
            doorscaned = null;
            npcscaned = null;
        }

        //발걸음 소리
        walkvalue += Mathf.Clamp(Mathf.Abs(h) + Mathf.Abs(v), 0, 1) * runval;
        if (walkvalue > 30)
        {
            temp = (_iszone) ? Random.Range(1, 3) : 0; //영역 안에 없으면 temp는 0
            switch (temp)
            {
                case 1:
                    audiosource.clip = footsteps[1 + _regionvalue];
                    break;
                case 2:
                    audiosource.clip = footsteps[2 + _regionvalue];
                    break;
                case 3:
                    audiosource.clip = footsteps[3 + _regionvalue];
                    break;
                default:
                    audiosource.clip = footsteps[0];
                    break;
            }
            audiosource.Play();
            walkvalue = 0;
        }
    }

    //움직임 통제
    IEnumerator MovePause(float _waitsec)
    {
        keygetpossible = false;
        yield return new WaitForSeconds(_waitsec);
        keygetpossible = true;
    }
}
