using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //카메라 자기자신
    private Camera thecamera;

    //카메라가 따라다닐 대상 변수
    public GameObject target;
    private Vector3 targetposition;

    //카메라가 움직일 영역 관련 변수
    public BoxCollider2D bound;
    private Vector3 minbound;
    private Vector3 maxbound;
    private Vector3 cenbound;
    private Vector3 boundextent;
    private float halfwidth;
    private float halfheight;
    private float clampedX;
    private float clampedY;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        thecamera = GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player");
        halfheight = thecamera.orthographicSize;
        halfwidth = halfheight * Screen.width / Screen.height;
    }

    
    void Update()
    {
        if(null != target)
        {
            //카메라 이동
            targetposition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            this.transform.position = targetposition;

            //카메라 영역
            if (halfwidth < boundextent.x)
                clampedX = Mathf.Clamp(this.transform.position.x, minbound.x + halfwidth, maxbound.x - halfwidth);
            else //카메라 너비가 맵 너비보다 넓을 때
                clampedX = cenbound.x;

            if (halfheight < boundextent.y)
                clampedY = Mathf.Clamp(this.transform.position.y, minbound.y + halfheight, maxbound.y - halfheight);
            else //카메라 높이가 맵 높이보다 높을 때
                clampedY = cenbound.y;

            //카메라 최종이동
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }

    public void SetBound(BoxCollider2D newbound)
    {
        bound = newbound;
        minbound = bound.bounds.min;
        maxbound = bound.bounds.max;
        cenbound = bound.bounds.center;
        boundextent = bound.bounds.extents;
    }

    public void ChangeCameraSize(float _camerasize)
    {
        GetComponent<Camera>().orthographicSize = _camerasize;
        halfheight = thecamera.orthographicSize;
        halfwidth = halfheight * Screen.width / Screen.height;
    }

    public void DefaultCameraSize()
    {
        GetComponent<Camera>().orthographicSize = 5f;
        halfheight = thecamera.orthographicSize;
        halfwidth = halfheight * Screen.width / Screen.height;
    }
    
    public void CameraPositionSet(Vector3 _vector)
    {
        this.transform.position = _vector;
    }
}
