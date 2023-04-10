using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Dynamic;
using System;
using UnityEngine.Video;

public class DialogueManage : MonoBehaviour
{
    [Header("Text")] // 유니티 인스펙터 창에 나타날 헤더
    public Text playterText; // 사용자 텍스트가 출력될 Text형의 변수.
    public Text NpcText; // NPC 텍스트가 출력될 Text형의 변수.
    public Text address; // 주소가 적힐 Text형 변수
    [Header("image")]
    public Image oimg; // 사용자 이미지
    public Sprite nspr; // NPC1 이미지
    public Sprite ospr; // NPC2 이미지
    [Header("Button")]
    public Button b1; //선택지버튼 1
    public Button b2; //선택지버튼 2
    public Button b3; //선택지버튼 3

    int state = 0;//클릭시 값 증가 여부 판단(1이면 클릭 시 대화 진행, 0 이면 스톱)
    int dialogueNum = 0;//대화 몇 번 째인지 판단
    int talkerNum = 0;// 0이면 유저, 1이면 NPC
    int btn_num = 0;//버튼 숫자
    int score = 0;//점수
    int m_score = 0;//차감 점수
    int flag = 0; //0이면 업데이트 안함, 1이면 업데이트함.(버튼)
    int double_click = 0;//재클릭방지
    int temp_num = 0;//버튼 임시 숫자
    string current_addres;//주소 표시 텍스트
    string fail_Reason;//차감 사유
    int step = 0; // 단계
    int checker = 0; // 체크를 할 flag임.
    private string dialouges = ""; //string 값의 dialouges

    bool FinishTyping = false; //타이핑이 끝났는지 확인할 bool 타입.

    public void ButtonShow() // 선택지 버튼을 보여주는 함수로 버튼1~3을 활성화 시킴.
    {
        b1.gameObject.SetActive(true);
        b2.gameObject.SetActive(true);
        b3.gameObject.SetActive(true);
    }

    public void ButtonHide() // 선택지 버튼을 안보이게 하는 함수로 버튼1~3을 비활성화 시킴.
    {
        b1.gameObject.SetActive(false);
        b2.gameObject.SetActive(false);
        b3.gameObject.SetActive(false);
    }

    public void StartDialogue() //대화 시작 하는 함수.
    {
        dialogueNum = 0; // 밑에 주석처리한 잘못된 방식에 상용된 변수로, 게임의 스테이지라 생각하면됨 (ex. 1-1-1, 1-1-2 등등 과 비슷한 개념)
        CallStart(); // CallStart 함수를 실행.
    }

    public void chooseBtn(int n) // 선택한 버튼을 눌렀을때 btn_num변수값을 매개변수로 받은 값으로 변경해주는 함수.
    {
        btn_num = n;
    }

    public void videonChange(int idx) //비디오 변경 함수로 매개변수로 받은 int형으로 아래 조건에 맞는 영상으로 재생시킴.
    {
        if (idx == 0)
        {
            GameObject.Find("centermain").transform.Find("firstvid").transform.gameObject.SetActive(true);
            GameObject.Find("videos").transform.Find("kmuvid").transform.gameObject.SetActive(false);
            GameObject.Find("videos").transform.Find("locationTrackingVid").transform.gameObject.SetActive(false);
        }
        else if (idx == 1)
        {
            GameObject.Find("centermain").transform.Find("firstvid").transform.gameObject.SetActive(false);
            GameObject.Find("videos").transform.Find("kmuvid").transform.gameObject.SetActive(false);
            GameObject.Find("videos").transform.Find("locationTrackingVid").transform.gameObject.SetActive(true);
            GameObject.Find("VideoManager").GetComponent<CenterVideo>().PlayVid1();
        }
        else if (idx == 2)
        {
            GameObject.Find("centermain").transform.Find("firstvid").transform.gameObject.SetActive(false);
            GameObject.Find("videos").transform.Find("kmuvid").transform.gameObject.SetActive(true);
            GameObject.Find("videos").transform.Find("locationTrackingVid").transform.gameObject.SetActive(false);
            GameObject.Find("VideoManager").GetComponent<CenterVideo>().playVid2();
        }
    }

    public void addrChange(string adr) // 매개변수를 받은 텍스트로 주소텍스트를 변경해주는 함수.
    {
        address.text = adr;
    }

    public void onState() // 콜버튼을 누르면 대화를 시작하는 함수.(밑에 잘못된 방법이였던 방식에 사용된 함수.)
    {
        state = 1;
        ButtonHide();
        GameObject.Find("DialogueCanvas").transform.Find("Dialouge_1").transform.gameObject.SetActive(true);
        StartDialogue();
    }

    public void closeState() // 112 신고센터 대화 UI진행도를 멈추고 초기화 하는 함수.
    {
        state = 0;
        dialogueNum = 0;
        btn_num = 0;
        IsRun = false;
        IsStop = false;
        clear();
        GameObject.Find("DialogueCanvas").transform.Find("Dialouge_1").transform.gameObject.SetActive(false);
        videonChange(0);
    }
    bool IsFinish = false;
    public void CloseWindow() // UI중 빨간색 x를 눌렀을때 사용될 함수, 완벽하게 종료되면 서버로 데이터 전송, 아니면 그냥 closeState함수를 실행후 UI를 비활성화.
    {
        if(IsFinish == true)
            input_data();
        closeState();
        GameObject.Find("CallCanvas").transform.Find("centermain").transform.gameObject.SetActive(false);
    }

    public void input_data() // 데이터를 서버로 전송해줌.
    {
        var copstone = new CopstoneConnector(VRLogin.ID, VRLogin.PW); //static 으로 저장한 VRLogin스크립트의 ID,PW로 서버에 연결
        dynamic playLog;
        int category_idx;

        // 신고센터
        category_idx = 3;
        playLog = new ExpandoObject();
        playLog.deductScore = m_score; // 차감 점수를 전송
        playLog.totalScore = score; // 총 점수를 전송
        playLog.deductCause = fail_Reason; // 차감 이유
        copstone.insertPlayLog(category_idx, playLog);

        IsFinish = false;
    }

    public void clear() // 데이터 초기화 해주는 함수.
    {
        talkerNum = 0;
        current_addres = "";
        temp_num = 0;
        addrChange(current_addres);
        videonChange(0);
        dialouges = "";
        playterText.text = "";
        NpcText.text = "";
        IsFinish = false;
    }
/* 잘못된 진행 방식에 사용된 코루틴 타이핑 효과임.
    IEnumerator _Typing()//코루틴 타이핑 효과
    {
        if (talkerNum == 0)
        {
            playterText.text = "";
            for (int i = 0; i <= dialouges.Length; i++)
            {
                playterText.text = dialouges.Substring(0, i);
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(5.0f);
            step++;
            if(dialogueNum == 1 && state == 1)
            {
                if(btn_num == 1 || btn_num == 2 || btn_num == 3)
                {
                    checker++;
                    step--;
                }
                    
            }
            if(dialogueNum == 2 && state == 1 && btn_num == 3)
            {
                checker++;
                step--;
            }
            if(IsFinish == true)
            {
                yield return new WaitForSeconds(1f);
                FinishTyping = true;
            }
            IsRun = false;
        }
        else if (talkerNum == 1 || talkerNum == 2)
        {
            NpcText.text = "";
            for (int i = 0; i <= dialouges.Length; i++)
            {
                NpcText.text = dialouges.Substring(0, i);
                yield return new WaitForSeconds(0.15f);
                
            }
            yield return new WaitForSeconds(5.0f);
            step++;
            
            if (dialogueNum == 1 && state == 1)
            {
                if (btn_num == 1 || btn_num == 2 || btn_num == 3)
                {
                    checker++;
                    step--;
                }
            }
            if (dialogueNum == 2 && state == 1 && btn_num == 3)
            {
                checker++;
                step--;
            }
            IsRun = false;
        }
        IsRun = false;
    }*/
    // Start is called before the first frame update
    void Start()
    {

    }
    bool IsStop = false;
    bool IsRun = false;
    // Update is called once per frame
    void Update()
    {
        //realstart();
    }
    void narrationplay(string audioname) //나레이션 오디오 플레이 해주는 함수. 매개변수로 받은 오디오 이름을 틀어줌.
    {
        AudioClip AC = Resources.Load(audioname) as AudioClip; // resource폴더에서 매개변수로 받은 이름의 오디오를 오디오클립 변수 AC의 값으로 지정.
        GameObject.Find("XR Rig Advanced").transform.Find("Narration").GetComponent<AudioSource>().clip = AC;
        GameObject.Find("XR Rig Advanced").transform.Find("Narration").GetComponent<AudioSource>().Play();
    }
    IEnumerator TypingEvent() // 타이핑효과를 내주는 IEnumerator임.
    {
        if (talkerNum == 0) // 시용자
        {
            playterText.text = "";
            for (int i = 0; i <= dialouges.Length; i++)
            {
                playterText.text = dialouges.Substring(0, i);
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(1.0f);
        } 
        else if (talkerNum == 1 || talkerNum == 2) //NPC1, NPC2
        {
            NpcText.text = "";
            for (int i = 0; i <= dialouges.Length; i++)
            {
                NpcText.text = dialouges.Substring(0, i);
                yield return new WaitForSeconds(0.15f);

            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    public void CallStart() // UI에서 대화 콘텐츠를 실행하는 Stage1 코루틴을 실행하는 함수.
    {
        StartCoroutine(Stage1());
    }
    IEnumerator Stage1() // 첫번째 대화
    {
        talkerNum = 0; // talkerNum 이 0이면 사용자, 1은 여자, 2는 남자.
        dialouges = "네! 112 신고 센터입니다.";
        yield return StartCoroutine(TypingEvent()); // 타이핑 효과가 끝날때 까지 스크립트 진행을 멈춤.
        talkerNum = 1;
        dialouges = "엄마? 오늘 저녁은 먹으러 못 갈 것 같아";
        narrationplay("audio1");
        yield return StartCoroutine(TypingEvent());
        videonChange(1);
        StartCoroutine(Stage2()); // stage2 코루틴을 실행하고 stage1은 종료됨.
    }
    IEnumerator Stage2()
    {
        talkerNum = 0;
        dialouges = "1. 선생님 장난 전화 하시면 안 됩니다." + "\n" + "2. 혹시, 지금 위급 상황이신가요?" + "\n" + "3. 전화 잘 못 거셨는데요?";
        yield return StartCoroutine(TypingEvent());
        yield return StartCoroutine(WaitSelectButton());
        if (btn_num == 1) // 선택한 버튼 1,2,3에 맞게 알맞게 코루틴이 실행됨. 이후 진행되는 코루틴에 대한 설명은 비슷하므로 생략함.
            StartCoroutine(Stage2_1());
        else if(btn_num == 2)
            StartCoroutine(Stage3());
        else
            StartCoroutine(Stage2_2());
    }
    IEnumerator Stage2_1() // 잘못된 선택 시
    {
        ButtonHide();
        talkerNum = 0;
        dialouges = "선생님 장난 전화 하시면 안 됩니다.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        dialouges = "아닌데요... 저 ...한 상황에 처해있어요!";
        narrationplay("audio2");
        yield return StartCoroutine(TypingEvent());
        talkerNum = 2;
        oimg.sprite = nspr;
        dialouges = "전화기 내놔! 누구랑 통화하는거야!";
        narrationplay("audio3");
        yield return StartCoroutine(TypingEvent());
        talkerNum = 0;
        dialouges = "선생님? 선생님?";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 2;
        dialouges = "죄송합니다. 여자친구가 흥분했나봐요.. 이제 조용할 겁니다...\n(상단의 X를 눌러 나가주세요)";
        narrationplay("audio4");
        yield return StartCoroutine(TypingEvent());
        score = 0; // 점수는 0으로
        m_score = -5; // 차감 점수는 -5
        fail_Reason = "상황을 자세히 살펴보지 않음."; // 차감 이유
        IsFinish = true; // IsFinish를 True로 바꿔주고 다음 코루틴은 없음.
    }
    IEnumerator Stage2_2()
    {
        ButtonHide();
        talkerNum = 0;
        dialouges = "전화 잘 못 거셨습니다.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        dialouges = "어...겨.ㅇ..아닌가요...?...죄송합니다..";
        narrationplay("audio7");
        yield return StartCoroutine(TypingEvent());
        dialouges = "........\n(상단의 X를 눌러 나가주세요)";
        yield return StartCoroutine(TypingEvent());
        score = 0;
        m_score = -5;
        fail_Reason = "상황을 자세히 살펴보지 않음.(불친절)";
        IsFinish = true;
    }
    IEnumerator Stage3()
    {
        ButtonHide();
        talkerNum = 0;
        dialouges = "혹시 지금 위급 상황이신가요?";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 2;
        oimg.sprite = nspr;
        dialouges = "오늘 못 들어간다고 전해!";
        narrationplay("audio5");
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        oimg.sprite = ospr;
        dialouges = "...음..네 맞아요! 엄마..";
        narrationplay("audio6");
        yield return StartCoroutine(TypingEvent());
        StartCoroutine(Stage4());
    }
    IEnumerator Stage4()
    {
        ButtonHide();
        btn_num = 0;
        talkerNum = 0;
        dialouges = "1. 지금 어디에 계신가요." + "\n" + "2. 감금당하셨나요?" + "\n" + "3. 전화 잘 못 거신 듯 합니다?";
        yield return StartCoroutine(TypingEvent());
        yield return StartCoroutine(WaitSelectButton());
        if (btn_num == 1)
            StartCoroutine(Stage5());
        else if (btn_num == 2)
            StartCoroutine(Stage6());
        else
            StartCoroutine(Stage4_1());
    }
    IEnumerator Stage4_1()
    {
        ButtonHide();
        talkerNum = 0;
        dialouges = "전화 잘 못 거셨습니다.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        dialouges = "어...겨..아닌가요...?...죄송합니다..";
        narrationplay("audio7");
        yield return StartCoroutine(TypingEvent());
        dialouges = "........\n(상단의 X를 눌러 나가주세요)";
        yield return StartCoroutine(TypingEvent());
        score = 0;
        m_score = -5;
        fail_Reason = "상황을 자세히 살펴보지 않고, 위급한 상황을 인지하지 못함.";
        IsFinish = true;
    }
    IEnumerator Stage5()
    {
        ButtonHide();
        talkerNum = 0;
        dialouges = "지금 어디에 계신가요? 현재 위치와 본인을 특정할 만한 것을 유추할 수 있게 둘러서 말해주세요.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        dialouges = "빨래 해놓은 흰색 심슨 후드티입고 나왔어, 공대 실습실도 다 비어있더라구, 지금은 1층 유니티카페야!";
        narrationplay("audio8");
        yield return StartCoroutine(TypingEvent());
        current_addres = "대구광역시 달서구 달서대로 675, 계명대학교 공과대학";
        addrChange(current_addres);
        videonChange(2);
        talkerNum = 2;
        oimg.sprite = nspr;
        dialouges = "무슨 소리하는거야! 빨리 끊어!";
        narrationplay("audio9");
        yield return StartCoroutine(TypingEvent());
        talkerNum = 0;
        dialouges = "계명대학교 공학관, 흰색 심슨 후드티 입은 여성분, 신고 접수되었습니다. 곧 경찰관이 출동합니다.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        oimg.sprite = ospr;
        dialouges = "네,  집에가서 봐요.. 엄마..\n(상단의 X를 눌러 나가주세요)";
        narrationplay("audio10");
        yield return StartCoroutine(TypingEvent());
        m_score = 0;
        fail_Reason = "없음";
        score = 5;
        IsFinish = true;
    }
    IEnumerator Stage6()
    {
        ButtonHide();
        talkerNum = 0;
        dialouges = "감금당하셨나요? 위치를 파악할 수 있게 범인이 눈치 못채도록 돌려서 말해주세요.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        dialouges = "어, 엄마.. 그 마카롱 잘하는 데서 사놨어.. 내 책상 3층 서랍에 있을거야.. 아마 한 5개쯤 들어있을껄??";
        narrationplay("audio11");
        yield return StartCoroutine(TypingEvent());
        current_addres = "대구광역시 달서구 서당로7길 45-5, 305호";
        addrChange(current_addres);
        videonChange(2);
        talkerNum = 2;
        oimg.sprite = nspr;
        dialouges = /*"<color =#ff0000>" + */"전화를 뭐 그리 오래해!"/* + "</color>"*/;
        narrationplay("audio12");
        yield return StartCoroutine(TypingEvent());
        talkerNum = 0;
        dialouges = "대구광역시 달서구 서당로7길 45-5, OO룸 305호, 신고 접수되었습니다. 가급적이면 범인을 자극하지 말아주십시오. 곧 경찰관이 출동합니다.";
        yield return StartCoroutine(TypingEvent());
        talkerNum = 1;
        oimg.sprite = ospr;
        dialouges = "네, 가볼게요.. 엄마..\n(상단의 X를 눌러 나가주세요)";
        narrationplay("audio13");
        yield return StartCoroutine(TypingEvent());
        m_score = 0;
        fail_Reason = "없음";
        score = 5;
        IsFinish = true;
    }
    IEnumerator WaitSelectButton() // 버튼 선택시 다음 코루틴으로 진행되지 않기 위해 while 무한 반복 해주는 IEnumerator
    {
        btn_num = 0;
        ButtonShow();
        while(true)
        {
            if (btn_num == 0) // 만약 btn_num이 0이라면 아직 아무것도 선택한 것이 아니기에 0.1초 기다리고 계속 무한 반복됨.
                yield return new WaitForSeconds(0.1f);
            else // btn_num이 0이 아니라는 것은 1~3중 하나를 선택했으므로 break를 사용하여 while 무한 반복을 탈출함.
                break;
        }
    }
	/* //--- 처음 112 신고센터 대화 다이얼로그 만든 함수인데 이는 잘못된 함수로 잊어버리지 않게 주석으로 처리함. 잘못된 방법임. (이 스크립트 고치느라 약 3일 걸림.)
	   //--- 이유는 매 단계마다 start코루틴을 썼는데 코루틴 실행하고 바로 다음 줄로 넘어가기 때문에 스크립트가 엉키는 문제가 발생함. (ex. 1단계 대화 이후 2단계로 가야하는데 3,4 단계로 넘어간다는 등...)
    public void realstart()
    {
        if (dialogueNum == 0 && state == 1)
        {
            if (step == 0 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 0;
                dialouges = "네! 112 신고 센터입니다.";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            if (step == 1 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 1;
                dialouges = "엄마? 오늘 저녁은 먹으러 못 갈 것 같아";
                narrationplay("audio1");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            if (step != 0 && step != 1 && IsRun == false)
            {
                videonChange(1);
                dialogueNum = 1;
                step = 0;
            }
        }
        if (dialogueNum == 1 && state == 1)
        {
            if (step == 0 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 0;
                dialouges = "1. 선생님 장난 전화 하시면 안 됩니다." + "\n" + "2. 혹시, 지금 위급 상황이신가요?" + "\n" + "3. 전화 잘 못 거셨는데요?";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 1)
            {
                if(IsStop == false)
                {
                    btn_num = 0;
                    timesetting();
                }
                if (btn_num == 1)//0점 피해자 사망
                {
                    if (checker == 0 && IsRun == false)
                    {
                        IsRun = true;
                        ButtonHide();
                        talkerNum = 0;
                        dialouges = "선생님 장난 전화 하시면 안 됩니다.";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 1 && IsRun == false)
                    {
                        IsRun = true;
                        talkerNum = 1;
                        dialouges = "아닌데요... 저 ...한 상황에 처해있어요!";
                        narrationplay("audio2");
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 2 && IsRun == false)
                    {
                        IsRun = true;
                        oimg.sprite = nspr;
                        talkerNum = 2;
                        dialouges = "전화기 내놔! 누구랑 통화하는거야!";
                        narrationplay("audio3");
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 3 && IsRun == false)
                    {
                        IsRun = true;
                        talkerNum = 0;
                        dialouges = "선생님? 선생님?";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 4 && IsRun == false)
                    {
                        IsFinish = true;
                        IsRun = true;
                        talkerNum = 2;
                        dialouges = "죄송합니다. 여자친구가 흥분했나봐요.. 이제 조용할 겁니다...\n(상단의 X를 눌러 나가주세요)";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                        narrationplay("audio4");
                        step = 0;
                        state = 0;
                        score = 0;
                        m_score = -5;
                        fail_Reason = "상황을 자세히 살펴보지 않음.";
                        checker = 0;
                        IsFinish = true;
                        FinishTyping = false;
                    }
                }
                else if (btn_num == 2)//정식 루트
                {
                    if (checker == 0 && IsRun == false)
                    {
                        IsRun = true;
                        ButtonHide();
                        talkerNum = 0;
                        dialouges = "혹시 지금 위급 상황이신가요?";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 1 && IsRun == false)
                    {
                        IsRun = true;
                        talkerNum = 2;
                        oimg.sprite = nspr;
                        dialouges = "오늘 못 들어간다고 전해!";
                        narrationplay("audio5");
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 2 && IsRun == false)
                    {
                        IsRun = true;
                        talkerNum = 1;
                        oimg.sprite = ospr;
                        dialouges = "...음..네 맞아요! 엄마..";
                        narrationplay("audio6");
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                        dialogueNum = 2;
                        checker = 0;
                        step = 0;
                    }
                }
                else if (btn_num == 3)//0점 피해자 사망
                {
                    if (checker == 0 && IsRun == false)
                    {
                        IsRun = true;
                        ButtonHide();
                        talkerNum = 0;
                        dialouges = "전화 잘 못 거셨습니다.";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 1 && IsRun == false)
                    {
                        IsRun = true;
                        talkerNum = 1;
                        dialouges = "어...겨.ㅇ..아닌가요...?...죄송합니다..";
                        narrationplay("audio7");
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 2 && IsRun == false)
                    {
                        IsRun = true;
                        dialouges = "........\n(상단의 X를 눌러 나가주세요)";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                        state = 0;
                        score = 0;
                        m_score = -5;
                        fail_Reason = "상황을 자세히 살펴보지 않음.(불친절)";
                        checker = 0;
                        IsFinish = true;
                    }
                }
            }
        }
        if (dialogueNum == 2 && state == 1)
        {
            if(btn_num != 0)
            {
                IsRun = false;
            }
            if (step == 0 && IsRun == false)
            {
                btn_num = 0;
                IsStop = false;
                IsRun = true;
                talkerNum = 0;
                dialouges = "1. 지금 어디에 계신가요." + "\n" + "2. 감금당하셨나요?" + "\n" + "3. 전화 잘 못 거신 듯 합니다?";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 1)
            {
                if (IsStop == false)
                {
                    btn_num = 0;
                    timesetting();
                }
                if (btn_num == 1)
                {
                    ButtonHide();
                    dialogueNum = 3;
                    step = 0;
                }
                else if (btn_num == 2)
                {
                    ButtonHide();
                    dialogueNum = 4;
                    step = 0;
                }
                else if (btn_num == 3)
                {
                    if (checker == 0 && IsRun == false)
                    {
                        IsRun = true;
                        ButtonHide();
                        talkerNum = 0;
                        dialouges = "전화 잘 못 거셨습니다.";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 1 && IsRun == false)
                    {
                        IsRun = true;
                        talkerNum = 1;
                        dialouges = "어...겨..아닌가요...?...죄송합니다..";
                        narrationplay("audio7");
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                    }
                    else if (checker == 2 && IsRun == false)
                    {
                        IsRun = true;
                        dialouges = "........\n(상단의 X를 눌러 나가주세요)";
                        StartCoroutine(_Typing());
                        StopCoroutine(_Typing());
                        state = 0;
                        score = 0;
                        m_score = -5;
                        fail_Reason = "상황을 자세히 살펴보지 않고, 위급한 상황을 인지하지 못함.";
                        checker = 0;
                        //closeState();
                        IsFinish = true;
                    }
                }
            }
        }
        if (dialogueNum == 3 && state == 1)//성공1
        {
            if (step == 0 && IsRun == false)
            {
                IsRun = true;
                ButtonHide();
                talkerNum = 0;
                dialouges = "지금 어디에 계신가요? 현재 위치와 본인을 특정할 만한 것을 유추할 수 있게 둘러서 말해주세요.";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 1 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 1;
                dialouges = "빨래 해놓은 흰색 심슨 후드티입고 나왔어, 공대 실습실도 다 비어있더라구, 지금은 1층 유니티카페야!";
                narrationplay("audio8");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 2 && IsRun == false)
            {
                IsRun = true;
                current_addres = "대구광역시 달서구 달서대로 675, 계명대학교 공과대학";
                addrChange(current_addres);
                step++;
            }
            else if (step == 3)
            {
                videonChange(2);
                step++;
            }
            else if (step == 4 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 2;
                oimg.sprite = nspr;
                dialouges = "무슨 소리하는거야! 빨리 끊어!";
                narrationplay("audio9");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 5 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 0;
                dialouges = "계명대학교 공학관, 흰색 심슨 후드티 입은 여성분, 신고 접수되었습니다. 곧 경찰관이 출동합니다.";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 6 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 1;
                oimg.sprite = ospr;
                dialouges = "네,  집에가서 봐요.. 엄마..\n(상단의 X를 눌러 나가주세요)";
                narrationplay("audio10");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
                state = 0;
                m_score = 0;
                fail_Reason = "없음";
                score = 5;
                step = 0;
                //closeState();
                IsFinish = true;
            }
        }
        if (dialogueNum == 4 && state == 1)//성공2
        {
            if (step == 0 && IsRun == false)
            {
                IsRun = true;
                ButtonHide();
                talkerNum = 0;
                dialouges = "감금당하셨나요? 위치를 파악할 수 있게 둘러서 말해주세요.";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 1 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 1;
                dialouges = "어, 엄마.. 그 마카롱 잘하는 데서 사놨어.. 내 책상 3층 서랍에 있을거야.. 아마 한 5개쯤 들어있을껄??";
                narrationplay("audio11");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 2)
            {
                current_addres = "대구광역시 달서구 서당로7길 45-5, 305호";
                addrChange(current_addres);
                step++;
            }
            else if (step == 3)
            {
                videonChange(2);
                step++;
            }
            else if (step == 4 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 2;
                oimg.sprite = nspr;
                dialouges = "전화를 뭐 그리 오래해!";
                narrationplay("audio12");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 5 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 0;
                dialouges = "대구광역시 달서구 서당로7길 45-5, OO룸 305호, 신고 접수되었습니다. 가급적이면 범인을 자극하지 말아주십시오. 곧 경찰관이 출동합니다.";
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
            }
            else if (step == 6 && IsRun == false)
            {
                IsRun = true;
                talkerNum = 1;
                oimg.sprite = ospr;
                dialouges = "네, 가볼게요.. 엄마..\n(상단의 X를 눌러 나가주세요)";
                narrationplay("audio13");
                StartCoroutine(_Typing());
                StopCoroutine(_Typing());
                m_score = 0;
                fail_Reason = "없음";
                score = 5;
                step = 0;
                //closeState();
                IsFinish = true;
            }
        }
    }*/
}
