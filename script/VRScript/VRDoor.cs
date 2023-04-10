using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class VRDoor : MonoBehaviour
{
    public GameObject Door; // GameObject Door 생성
    public GameObject RightHand; // 오른손 오브젝트
    private bool IsDoorOpen = false; // bool 타입으로 선언
    GameObject SearchTextParent; // GameObject 생성 (부모로 설정할 오브젝트)
    GameObject SearchTextPrefeb; // GameObject 생성 (프리팩 오브젝트)
    GameObject SearchText; // GameObject 생성 (동적 생성한 오브젝트)

    public void OpenDoor() // 트리거 클릭시 발동시킬 함수
    {
        if(IsDoorOpen == false) // 만약 bool타입이 false면
        {
            Door.transform.gameObject.GetComponent<DoorScript>().OpenDoor(); // Door오브젝트에 DoorScript에서 OpenDoor 함수 실행
            IsDoorOpen = true; // bool타입을 true로 전환
        }
        else // 만약 bool타입이 ture라면
        {
            Door.transform.gameObject.GetComponent<DoorScript>().CloseDoor(); // Door오브젝트에 DoorScript에서 CloseDoor 함수 실행
            IsDoorOpen = false; // bool타입을 false로 전환
        }
    }

    public void OpenDoorText() // 문쪽으로 손을 향했을때 실행할 함수
    {
        SearchTextParent = Door; // GameObject SearchTextParent는 SearchItem으로 지정.
        SearchTextPrefeb = Resources.Load("SerachText") as GameObject; // 리소스 폴더에서 SerachText 프리팹 로딩하여 SearchTextPrefeb으로 설정함.
        SearchTextPrefeb.transform.Find("Text").GetComponent<Text>().text = "트리거를 땡겨 문을 열고 닫으세요"; // 자식중 Text에 콤포넌트 text를 찾아 text값을 SearchItem의 콤포넌트 Item에서 Discription으로 바꿈.
        SearchText = (GameObject)Instantiate(SearchTextPrefeb, Vector3.one, Quaternion.identity, RightHand.transform.Find("SearchCanvas").transform);// 동적생성 하는데 부모는 오른손, 위치는 조금 위에, 180도 회전시켜서 생성
        SearchText.transform.localScale = Vector3.one; // 로컬 스케일을 one즉 1:1로 바꿈
        SearchText.transform.localPosition = Vector3.zero; // 로컬 포지션은 Vector3 zero로 설정
        SearchText.transform.localEulerAngles = Vector3.zero; // EulerAngles 또한 Vector3 zero로 설정
    }
    public void DestroyDoorText() // 문쪽으로 향했던 손을 다른쪽으로 치우면 실행할 함수
    {
        Destroy(SearchText); // 동적생성된 SearchText를 없앰.
    }
}
