using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 매니지 먼트, 넣어야 씬 전환 사용 가능

public class Search_MoveScene : MonoBehaviour
{
    public void MoveMainMenu()
    {
        SceneManager.LoadScene("CallCenterScene");
    }
    public void Restart()
    {
        SceneManager.LoadScene("Crime_1");
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
