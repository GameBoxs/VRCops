using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRShootRangeMoveScene : MonoBehaviour
{
    public void RestartShootRange()
    {
        SceneManager.LoadScene("ShootingRange");
    }
    public void ChangeMainScene()
    {
        SceneManager.LoadScene("VR_MainScene");
    }
}
