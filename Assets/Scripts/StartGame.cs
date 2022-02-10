using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void startGame()
    {
        SceneManager.LoadScene("Playing Field");
    }
}