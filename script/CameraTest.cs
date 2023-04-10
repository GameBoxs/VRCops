using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public float TurnSpeed = 4.0f; // 카메라 이동 스피드 변수로 float형으로 생성.
    private float xRotate = 0.0f; // x로테이트 변수
    // Start is called before the first frame update
    void Start() // 이 스크립트가 처음 실행할때.
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 중앙에 락 걸어 커서가 이동하는거를 막아줌.
        Cursor.visible = false; // 마우스 커서를 사라지게 함.
    }

    // Update is called once per frame
    void Update()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * TurnSpeed; // 마우스가 x축이 움직이는 인풋 받은 값에 스피드 변수를 곱해주어 y로테이트사이즈 변수에 넣어줌.
        float yRotate = transform.eulerAngles.y + yRotateSize; // y로테이트는 오일러각 y + 19번줄 변수 의 값을 넣어줌.

        float xRotateSize = -Input.GetAxis("Mouse Y") * TurnSpeed; // 22,23 코드는 19,20 코드 줄과 비슷하고 마우스 y축 입력시 x 로테이트에 넣어줌으로 설명 생략.
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0); // transform(위치정보 요소)의 오일러각은 vector3의 값을 넣어줌. 
    }
}
/*
졸업작품 VR 프로젝트 작업전 PC버전으로 유니티 환경 구축하고 유니티로 3D는 처음 개발이기에 마우스를 이용한 카메라 움직임 제어 스크립트를 테스트로 작성하여 유니티 적응을 시도한 스크립트
*/