using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupCabinScene : MonoBehaviour
{

    private GameObject player;
    private bool insideCabin;

    void Start()
    {
        player = GameObject.Find("First Person Player");
        insideCabin = false;
    }

    void Update()
    {
        if(player.transform.position.x > 515 || player.transform.position.x < 480 || player.transform.position.z > 510 || player.transform.position.z < 480)
        insideCabin = false;

        if(player != null && !insideCabin)
        {
            player.transform.position = new Vector3(500, 4.1f, 500);
            insideCabin = true;
        }
    }
}
