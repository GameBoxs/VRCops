using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class VRSearchItemText : MonoBehaviour
{
    public GameObject SearchItem; // 스크립트 적용한 오브젝트 유니티에서 넣기
    public GameObject RightHand; // VR 캐릭터 오른손, 유니티에서 넣기
    GameObject SearchTextParent;
    GameObject SearchTextPrefeb;
    GameObject SearchText;
    bool IsSearchTextRun = false;
    public void ShowSearchItemText() // 조사 아이템 텍스트를 띄워주는 함수.
    {
        if (IsSearchTextRun == false) // 만약 bool타입이 false라면
        {
            MakeSearchItemText(); // 조사 텍스트를 만들어주는 함수 실행
            IsSearchTextRun = true; // bool타입 true로 변환
        }
    }
    public void ShowPhoneMessage() // 핸드폰 메시지 UI를 띄워주는 함수.
    {
        GameObject.Find("PhoneCanvas").transform.Find("MessageList").transform.gameObject.SetActive(true);
    }
    public void ShowLaptop() // 노트북 UI를 띄워주는 함수.
    {
        GameObject.Find("LaptopCanvas").transform.Find("Di").transform.gameObject.SetActive(true);
    }
    public void MakeOutline() // 오브젝트 아웃라인 만들어주는 함수
    {
        SearchItem.GetComponent<Outline>().enabled = true;
    }
    public void DestroySearchItemText() // 조사 텍스트 없애는 함수.
    {
        SearchItem.GetComponent<Outline>().enabled = false;
        if (IsSearchTextRun == true)
        {
            Destroy(SearchText); // 동적생성된 SearchText를 없앰.
            IsSearchTextRun = false;
        }
    }
    void MakeSearchItemText() // 조사 텍스트를 만들어 주는 함수.
    {
        SearchTextParent = SearchItem; // GameObject SearchTextParent는 SearchItem으로 지정.
        SearchTextPrefeb = Resources.Load("SerachText") as GameObject;
        SearchTextPrefeb.transform.Find("Text").GetComponent<Text>().text = SearchItem.GetComponent<Item>().Discription; // 자식중 Text에 콤포넌트 text를 찾아 text값을 SearchItem의 콤포넌트 Item에서 Discription으로 바꿈.
        SearchText = (GameObject)Instantiate(SearchTextPrefeb, Vector3.one, Quaternion.identity,RightHand.transform.Find("SearchCanvas").transform);
        // 동적생성 하는데 부모는 오른손, 위치는 조금 위에, 180도 회전시켜서 생성
        SearchText.transform.localScale = Vector3.one; // 로컬 스케일을 one즉 1:1로 바꿈
        SearchText.transform.localPosition = Vector3.zero;
        SearchText.transform.localEulerAngles = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start() // 스크립트 실행시 아래 문단 실행됨.
    {
        SearchItem.GetComponent<Outline>().OutlineColor = Color.yellow;
        SearchItem.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        SearchItem.GetComponent<Outline>().OutlineWidth = 10f;
    }
}
