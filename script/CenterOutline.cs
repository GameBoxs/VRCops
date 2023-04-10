using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterOutline : MonoBehaviour
{
    public Button callme; // 112 신고센터에 전화번호 버튼 오브젝트를 넣을 버튼.
    // Start is called before the first frame update
    void Start() // 스크립트를 실행할때 madeoutline 함수를 실행함.
    {
        madeoutline();
    }
    public void madeoutline() // 아웃라인을 만들어주는 함수.
    {
        callme.GetComponent<Outline>().OutlineColor = Color.green;
        callme.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        callme.GetComponent<Outline>().OutlineWidth = 30f;
        callme.GetComponent<Outline>().enabled = true;
    }
    public void DestroyLine() // 아웃라인 만든것을 없애주는 함수.
    {
        callme.GetComponent<Outline>().enabled = false;
    }
}
