using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class VRRegister : MonoBehaviour, IPointerClickHandler
{
    public InputField InputField;
    public GameObject RegKeyBoard;

    public void OnPointerClick(PointerEventData eventData) // 이벤트를 사용함
    {
        string TextName = ""; //string형 TextName을 만듬
        if (InputField.name == "IDInputField") // 만약 Inputfield.name 이 IDInputField라면
            TextName = "ID를 입력해 주세요."; // TextName 에 값 넣음.
        else if(InputField.name == "PWIputField") // 15,16줄과 비슷한 기능이기에 설명 생략.
            TextName = "PW를 입력해 주세요.";
        else if (InputField.name == "NickNameIputField")
            TextName = "닉네임을 입력해 주세요.";
        else
            TextName = "이메일을 입력해 주세요.";
        RegKeyBoard.transform.GetComponent<KeyboardScript>().TextField = InputField; // RegKeyBoard게임 오브젝트의 요소중 스크립트 KeyboardScript에서 TextField는 InputField의 값을 넣음.
        RegKeyBoard.transform.Find("Text").GetComponent<Text>().text = TextName; // RegKeyBoard게임 자식중 Text에서 Text요소중 text를 TextName값을 넣음.
        RegKeyBoard.SetActive(true); // RegKeyBoard를 활성화
    }
    public void Register() // 가입 하는 함수
    {
        try // 로그인과 마찬가지로 try catch 사용
        {
            // 아이디, 비밀번호, 닉네임, 이메일 순서대로 인풋 필드 텍스트를 string생성한 변수에 넣음.
            String ID = GameObject.Find("RegCanvas").transform.Find("VR_Register").transform.Find("IDInputField").GetComponent<InputField>().text;
            String PW = GameObject.Find("RegCanvas").transform.Find("VR_Register").transform.Find("PWIputField").GetComponent<InputField>().text;
            String Nick = GameObject.Find("RegCanvas").transform.Find("VR_Register").transform.Find("NickNameIputField").GetComponent<InputField>().text;
            String EM = GameObject.Find("RegCanvas").transform.Find("VR_Register").transform.Find("EMailIputField").GetComponent<InputField>().text;
            var signin = CopstoneConnector.signIn(ID, PW, Nick, EM);
            if (signin.error != 0)
            {
                throw new Exception(signin.message);
            }
            //여기오면 회원가입성공
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            //회원가입 실패
        }
    }
}
