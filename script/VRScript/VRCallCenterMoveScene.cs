using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VRCallCenterMoveScene : MonoBehaviour
{
    public void RestartCallCenter() //112 신고센터 씬 재 로딩
    {
        SceneManager.LoadScene("Call_Center");
    }
    public void ChangeMainScene() // 112 신고센터에서 메인 씬으로 이동
    {
        SceneManager.LoadScene("VR_MainScene");
    }
}
