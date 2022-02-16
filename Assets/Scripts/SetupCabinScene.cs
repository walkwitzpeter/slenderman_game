using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupCabinScene : MonoBehaviour
{

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("First Person Player");
        player.transform.position = new Vector3(500, 6, 500);
    }
}
