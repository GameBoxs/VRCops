using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CenterVideo : MonoBehaviour // 112 신고센터 비디오를 재생하는 스크립트
{
    [Header("Videos")]
    public GameObject video1; // video1은 비디오를 출력시킬 오브젝트를 유니티에서 직접 넣어줌.
    public VideoPlayer dvd; // 비디오 리소스를 유니티 내에서 끌어다 넣어줌.
    public GameObject video2; // 9번줄과 설명 같음.
    public VideoPlayer blueray; // 10번줄과 설명 같음.

    public void PlayVid1() // 비디오 1번 실행해주는 함수.
    {
        video1.SetActive(true); // 오브젝트 video1을 활성화
        dvd.Play(); // 비디오 dvd를 실행
    }

    public void resetVid() // 비디오를 리셋 시켜주는 함수.
    {
        dvd.time = 0f; // 영상 time을 0으로
        dvd.playbackSpeed = 1f; // 스피드는 1
        blueray.time = 0f;
        blueray.playbackSpeed = 1f;
    }

    public void playVid2() // 비디오2 실행해주는 함수.
    {
        video2.SetActive(true);
        blueray.Play();
    }
    public void stopvid() // 비디오 정지 해주는 함수.
    {
        dvd.Pause();
        blueray.Pause();
    }
}
