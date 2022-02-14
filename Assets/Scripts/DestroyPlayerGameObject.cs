using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayerGameObject : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("First Person Player");
        if(player !=null) Destroy(player);
    }
}
