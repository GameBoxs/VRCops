using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Center_ShowUI : MonoBehaviour //112 신고센터 UI를 띄워주는 스크립트.
{
    public GameObject SearchItem; // 유니티에서 오브젝트를 직접 넣을수 있도록 public 으로 선언한 게임 오브젝트.

    public void ShowCallui() // ui를 보여주는 함수.
    {
        GameObject.Find("CallCanvas").transform.Find("centermain").transform.gameObject.SetActive(true);
    }
    public void MakeOutline() // 생성한 SearchItem 오브젝트에 적용된 outline 스크립트의 변수값들을 설정해줌. > 실질적으로는 오브젝트에 아웃라인을 만들어주는 함수임.
    {
        SearchItem.GetComponent<Outline>().OutlineColor = Color.yellow;
        SearchItem.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        SearchItem.GetComponent<Outline>().OutlineWidth = 20f;
        SearchItem.GetComponent<Outline>().enabled = true;
    }

    public void DestroyLine() // 만들었던 아웃라인을 없애주는 함수.
    {
        SearchItem.GetComponent<Outline>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
