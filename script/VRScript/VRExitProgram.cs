using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRExitProgram : MonoBehaviour
{
    public void QuitGame() // 프로그램 종료하는 함수
    {
        #if UNITY_EDITOR // 만약 유니티 에디터에서 실행 했을때는 아래 10번줄 실행
        UnityEditor.EditorApplication.isPlaying=false; // 유니티 에디터 실행을 종료함.
        #else // 유니티 에디터가 아니라면
        Application.Quit();  // 프로그램 종료
        #endif
    }
}
