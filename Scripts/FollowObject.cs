using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {
    public Transform target;
    //따라다닐 타겟 잡고
    public Vector3 offset;
    //적용할 오프셋
    public Camera mainCamera;
    //메인카메라 붙일꺼
    public Camera uiCamera;
    //2d ui위젯 랜더링할 수직방향 카메라;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null) //타겟 있을 때
        {
            Vector3 finalPos = mainCamera.WorldToScreenPoint(target.position);
            //UI Camera 월드 좌표로 변환
            finalPos = uiCamera.ScreenToWorldPoint(finalPos);
            //수직 카메라 x y 축만 쓸거 기억
            finalPos = new Vector3(finalPos.x, finalPos.y, 0);
            //최종위치 오프셋하고 같이 적용시키기
            transform.position = finalPos + offset;
        }
	}
    /*
     플레이어 좌표 -> 메인 카메라로부터의 스크린좌표로 변환 -> UI 카메라 월드좌표로 다시변환
     offset 오프셋 : 영점위치 맞추는 느낌?
     */
}
