using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCabinScene : MonoBehaviour
{

    public GameObject player;

    void Awake()
    {
        player.transform.position = new Vector3(500, 6, 500);
    }
}
