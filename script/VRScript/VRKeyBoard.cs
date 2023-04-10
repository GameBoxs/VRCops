using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VRKeyBoard : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler // MonoBehaviour뒤에 것들은 유니티상 이벤트를 구현 해주는 것들이며 이것들을 상속 받음.
{
    public GameObject InputField; // 인풋 필드 게임 오브젝트
    public GameObject IDKeyBoard; // ID 인풋 눌렀을때 띄울 키보드
    public GameObject PWKeyBoard; // PW 인풋 눌렀을때 띄울 키보드
    public void OnPointerClick(PointerEventData eventData) // OnPointerClick이벤트
    {
        if(InputField.transform.gameObject.name == "IDInputField") // 클릭된 InputField의 이름이 IDInputField라면
        {
            IDKeyBoard.transform.gameObject.SetActive(true); // IDKeyBoard를 활성화
            PWKeyBoard.transform.gameObject.SetActive(false); // PWKeyBoard를 비활성화
            Debug.Log("ID인풋필드"); // 디버그 로그 띄움
        }
        else if(InputField.transform.gameObject.name == "PWIputField") // 클릭된 InputField의 이름이 PWIputField라면
        {
            IDKeyBoard.transform.gameObject.SetActive(false); // IDKeyBoard를 비활성화
            PWKeyBoard.transform.gameObject.SetActive(true); // PWKeyBoard를 활성화
            Debug.Log("PW인풋필드"); // 디버그 로그 띄움
        }
        else
        {
            Debug.Log("뭔가 다른게 입력됨."); // 디버그 로그 띄움
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
