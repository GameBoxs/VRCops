using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Dynamic;
using System.Linq;
using UnityEngine.SceneManagement;
public class VRSearchFinish : MonoBehaviour
{
    public GameObject EvidenceBox; // GameObject 생성
    string Suspect = ""; // String형 생성

    public void SelectSuspect(string S) // 범인 지목시 String형 생성한 Suspect에 매개변수 s의 값을 저장하는 함수.
    {
        Suspect = S;
    }


    public void SubmitCrime() // 서버로 전송하는 함수.
    {
        List<Item> evidencelist = new List<Item>(); // Item형 리스트 evidencelist를 생성
        evidencelist = EvidenceBox.GetComponent<VRInventory>().items; // evidencelist에 EvidenceBox게임 오브젝트에 VRInventory스크립트에 저장된 items리스트의 값을 저장시킴.
        evidencelist.RemoveAll(item => item == null); // 람다식으로 item중 null값을 없애줌.
        string[] itemList = evidencelist.Select(each => each.itemName).ToArray<string>(); // string 배열로 itemList를 만들고 evidencelist에서 itemName들을 선택하여 값을 하나씩 저장함.
        var copstone = new CopstoneConnector(VRLogin.ID, VRLogin.PW); // static으로 저장된 id,pw로 로그인을 시도
        dynamic playLog; // dynamic 형으로 playlog를 생성
        int category_idx; // 서버에서 분류할 인텍스임.
        
        category_idx = 2; // 인덱스는 2
        playLog = new ExpandoObject();
        playLog.suspect = Suspect; // playLog.suspect는 Suspect값을
        playLog.evidence = itemList; // playLog.evidence는 itemList값을
        copstone.insertPlayLog(category_idx, playLog); // 값을 보냄.
    }

    public void MoveToMain() // 메인 화면으로 이동해주는 함수.
    {
        SceneManager.LoadScene("VR_MainScene");
    }
}
