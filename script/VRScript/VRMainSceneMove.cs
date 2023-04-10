using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMainSceneMove : MonoBehaviour
{
    public void MoveToShooting() // 장비체험 파트로 이동 해주는 함수
    {
        SceneManager.LoadScene("ShootingRange"); // shootingRange씬을 로딩함
    }
    public void MoveToCrime() // 강력범죄현장수사체험 파트로 이동 해주는 함수
    {
        SceneManager.LoadScene("Crime_1"); // Crime_1 씬을 로딩함.
    }
    public void CallCenter() // 112신고센터 체험 파트로 이동 해주는 함수
    {
        SceneManager.LoadScene("Call_Center"); // Call_Center 씬을 로딩함.
    }
}
