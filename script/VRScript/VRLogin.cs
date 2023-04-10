using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; // Json 형식으로 받기 때문에 using해줌
using Newtonsoft.Json.Converters;
using System;
using UnityEngine.UI;

public class VRLogin : MonoBehaviour
{
    public InputField IDText; // InputField형인 IDText생성.
    public InputField PWText; // InputField형인 PWText생성.
    public static String ID; // 다른 스크립트에서도 사용해야 하기 때문에 static으로 string형인 ID를 생성.
    public static String PW; // 다른 스크립트에서도 사용해야 하기 때문에 static으로 string형인 PW를 생성.
    static CopstoneConnector copstone; // 서버와 연동 해주는 CopstoneConnector형인 copstone을 생성
    bool Success = false; // bool타입 생성
    public void LogIn() // 로그인 함수
    {
        try // try catch 사용
        {
            copstone = new CopstoneConnector(IDText.text, PWText.text); // copstone에 IDText의 text 내용과, PWText의 text를 보냄.
            Success = true; //만약 문제가 있다면 catch로 넘어갈것임.
        }
        catch (Exception e) // 로그인 실패 시
        {
            IDText.text = "";
            PWText.text = "";
            Debug.Log("로그인 실패");
            Success = false;
        }
        if (Success == true) // 만약 로그인 성공 했다면
        {
            GameObject.Find("LogginCanvas").transform.Find("VR_Login").transform.gameObject.SetActive(false); // LogginCanvas게임 오브젝트 찾고 자식인 VR_Login을 비활성화 시킴
            ID = IDText.text; // static으로 선언한 ID에 IDText.text의 값을 넣음.
            PW = PWText.text; // static으로 선언한 PW에 PWText.text의 값을 넣음.
            Debug.Log("ID:" + IDText.text + " PW:" + PWText.text); // 디버그 로그 띄움
            GameObject.Find("MoveSceneCanvas").transform.Find("BG").transform.gameObject.SetActive(true); // 메인화면 모니터에 띄워지는 캔버스인 MoveSceneCanvas캔버스를 찾고 자식인 BG를 활성화 시킴
        }
    }
    public void CloseKeyBoard() // VR입력 키보드를 사라지게 하는 함수
    {
        GameObject.Find("KeyBoardCanvas").transform.Find("OnScreenKeyboardID").transform.gameObject.SetActive(false); // KeyBoardCanvas를 찾고 자식으로 OnScreenKeyboardID을 비활성화 시킴
        GameObject.Find("KeyBoardCanvas").transform.Find("OnScreenKeyboardPW").transform.gameObject.SetActive(false); // KeyBoardCanvas를 찾고 자식으로 OnScreenKeyboardPW을 비활성화 시킴
    }
    public void LogOut() // 로그아웃 해주는 함수
    {
        ID = "";
        PW = "";
        IDText.text = "";
        PWText.text = "";
    }
    // Start is called before the first frame update
    void Start() // 스크립트가 실행 될때 실행 되는 함수.(유니티 기본 함수)
    {
        if (ID != null && PW != null) // ID, PW가 null값이 아닐때
        {
			//게임 첫 실행시 나오는 로그인 UI는 띄우지 않고 모니터에 씬 이동 해주는 UI만 띄워주고, 음성 나래이션은 비활성화 시킴.
			//여러 컨텐츠에서 다시 메인화면으로 넘어올 수 있기 때문에 이것을 만듬.
            GameObject.Find("LogginCanvas").transform.Find("VR_Login").transform.gameObject.SetActive(false);
            GameObject.Find("MoveSceneCanvas").transform.Find("BG").transform.gameObject.SetActive(true);
            GameObject.Find("XR Rig Advanced").transform.Find("MainNarration").transform.gameObject.SetActive(false);
        }
        else // 아니라면. 즉, 게임을 첫 실행 했을때
        {
            StartCoroutine(narrationplay()); // narrationplay 코루틴을 실행 시킴.
        }
    }
    IEnumerator narrationplay()
    {
        yield return new WaitForSeconds(1f); //1초 기다린뒤 다음줄 실행
        GameObject.Find("XR Rig Advanced").transform.Find("MainNarration").transform.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
