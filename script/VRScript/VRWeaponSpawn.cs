using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Dynamic;
public class VRWeaponSpawn : MonoBehaviour
{
    public GameObject PistolParent; // PistolPosition 오브젝트 넣을것
    public GameObject PistolPrefeb; // PistolPrefeb 오브젝트 넣을것.
    GameObject Pistol; // pistol 오브젝트
    public GameObject PistolClipParent; // PistolClipPosition 오브젝트 넣을것
    public GameObject PistolClipPrefeb; //PistolClipPrefeb 오브젝트 넣을것.
    GameObject PistolClip; // pistoclip 오브젝트
    bool IsReal = false; // 기록사격이면 true , 연습이면 false;
    bool FinishGunTutorial = false; // 사격 튜토리얼 끝났는지 여부

    public GameObject ExplainFinal; // ExplaneFinal 오브젝트 넣기.
    public GameObject Explain1; // Explain1 오브젝트 넣기
    bool Explain = false; // 설명창이 켜져있는지 아닌지, 기본값은 false;

    public Text MonitorScore; // Score의 MyScore넣을것

    public static int NumberScore; // 스코어 점수

    public void SpawnAll() // 인스턴트로 프리팹으로 만든 권총과 탄창을 스폰 시키는 함수.
    {
		//pistol부모 위치 x,y,z 지점에 스폰 되며 로테이션은 쿼터니언 identity임(0), 탄창도 탄창 부모 x,y,z위치에 로테이션은 쿼터니언 identity임.
        Pistol = Instantiate(PistolPrefeb, new Vector3(PistolParent.transform.position.x, PistolParent.transform.position.y, PistolParent.transform.position.z), Quaternion.identity);
        PistolClip = Instantiate(PistolClipPrefeb, new Vector3(PistolClipParent.transform.position.x, PistolClipParent.transform.position.y, PistolClipParent.transform.position.z), Quaternion.identity);
    }
    public void Retry() // 재시도 하는 함수.
    {
        Pistol.transform.parent = null; // 권총에 지정된 부모를 null값으로 변경. (한손에 권총 쥐고 있으면 부모는 손이고 이상태로 재시도 하면 오류가 생길수 있기 때문)
        PistolClip.transform.parent = null; //탄창에 지정된 부모를 null값으로 변경.
        Destroy(Pistol); // 인스턴트로 생성된 권총을 destory해줌
        Destroy(PistolClip); // 인스턴트로 생성된 탄창을 destory해줌.
        SpawnAll(); // 권총, 탄창을 스폰해주는 함수 실행.
    }
    public void FinishShoot() // 사격 종료 할때 실행되는 함수.
    {
        if(IsReal == false) // IsReal은 기록사격인지 아닌지 구별해주는 bool임. false이므로 연습사격임.
        {
            Pistol.transform.parent = null;
            PistolClip.transform.parent = null;
            Destroy(Pistol);
            Destroy(PistolClip);
        }
        else // 기록사격
        {
            Pistol.transform.parent = null;
            PistolClip.transform.parent = null;
            Destroy(Pistol);
            Destroy(PistolClip);
            SubmitScore(); // 기록된 점수를 서버로 전송해주는 함수.
        }
    }
    public void SubmitScore() // 서버로 정보 전송하는 함수
    {   // 총 완전 끝남.
        var copstone = new CopstoneConnector(VRLogin.ID,VRLogin.PW);
        string GunName = "반자동 권총";
        dynamic playLog;
        int category_idx;

        // 장비체험
        category_idx = 1;
        playLog = new ExpandoObject();
        playLog.gunName = GunName; //총 이름
        playLog.gunScore = Convert.ToInt32(MonitorScore.text); // 점수
        copstone.insertPlayLog(category_idx, playLog); // category_idx에 playLog(여기엔 ID, 총 이름, 점수가 포함되어 있음.)를 보냄.
    }
    public void InitMonitor() // 사격 기록 모니터 점수 초기화 해주는 함수.
    {
        MonitorScore.text = "0";
        NumberScore = 0;
    }
    public void RealShoot() // 기록사격 선택시 bool 변수값 바꿔주는 함수.
    {
        IsReal = true;
    }
    public void PracticeShoot() // 연습사격 선택시 bool 변수값 바꿔주는 함수
    {
        IsReal = false;
    }
    public void Explaincheck() // 사격전 교육ui에서 이전에 교육을 들었는지 안들었는지 확인 해주는 함수.
    {
        if(Explain == false) // 설명창을 띄운적이 없다면
        {
            if(FinishGunTutorial == true) // 교육을 이전에 들은적이 있다면
            {
                ExplainFinal.SetActive(true); //연습, 기록, 종료 UI를 띄움(앞의 교육 UI는 생략함.)
                Explain = true; // 설명, 선택창이 켜져있기에 Explain을 true로 바꿈
            }
            else // 교육을 이전에 들은적이 없다면
            {
                Explain1.SetActive(true); // 처음부터 교육하는 UI를 띄움.
                Explain = true; // 설명, 선택창이 켜져있기에 Explain을 true로 바꿈
            }
        }
    }
    public void CloseExplain() // 교육, 설명 UI창을 닫을때.
    {
        Explain = false; // Explain 변수값을 false로
    }
    public void FinishiTuto() // 튜토리얼(교육)을 다 봤을때 변수값을 변경해주는 함수.
    {
        FinishGunTutorial = true;
    }
    private void Update()
    {
        MonitorScore.text = NumberScore.ToString(); // NumberScore을 string으로 바꾼 값을 모니터 사격 점수 해주는 text의 값으로 넣음.
    }
}
