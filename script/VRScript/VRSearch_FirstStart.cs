using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
public class VRSearch_FirstStart : MonoBehaviour
{
    GameObject DiscriptionCanvas; // GameObject생성
    // Start is called before the first frame update
    void Start() // 스크립트가 실행될때 실행됨.
    {
        DiscriptionCanvas = GameObject.Find("DiscriptionCanvas"); // DiscriptionCanvas의 게임 오브젝트는 DiscriptionCanvas로 설정
        DiscriptionCanvas.transform.Find("Discription").gameObject.SetActive(true); // DiscriptionCanvas의 자식에 Discription을 활성화
        DiscriptionCanvas.transform.Find("Discription").GetComponent<CanvasGroup>().alpha = 0; // DiscriptionCanvas의 자식에 Discription의 요소중 CanvasGroup에서 alpha를 0으로 설정
        StartCoroutine("Sizeup"); //Sizeup 코루틴 실행(아래와 코루틴 실행 법을 바꿔봤음)
        StartCoroutine(narrationplay()); // narrationplay코루틴 실행.
    }
    IEnumerator narrationplay()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("XR Rig Advanced").transform.Find("Narration").transform.gameObject.SetActive(true);
    }
    IEnumerator Sizeup()
    {
        while (DiscriptionCanvas.transform.Find("Discription").GetComponent<CanvasGroup>().alpha < 1.0f) // DiscriptionCanvas의 자식에 Discription의 요소중 CanvasGroup에서 alpha가 1.0미만일때까지 반복 실행
        {
            DiscriptionCanvas.transform.Find("Discription").GetComponent<CanvasGroup>().alpha += 0.2f; // alpha값을 0.2씩 더함.
            yield return new WaitForSeconds(0.1f); // 0.1초 기다림
            Debug.Log(DiscriptionCanvas.transform.Find("Discription").GetComponent<CanvasGroup>().alpha); // 디버그 띄움.
        }
    }
}
