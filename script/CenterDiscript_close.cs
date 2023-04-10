using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterDiscript_close : MonoBehaviour
{
    public void Kill_ui() // 112 신고센터 UI를 없애주는 함수.
    {
        GameObject.Find("Discription").gameObject.SetActive(false); // 게임오브젝트중 Discription 오브젝트를 찾고 비활성화 시킴.
        GameObject.Find("locationIndicatorCanvas").transform.Find("position_show").transform.gameObject.SetActive(true);
		//locationIndicatorCanvas오브젝트의 자식중 position_show를 찾아 활성화 시킴.
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
