using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffExtraPlayer : MonoBehaviour
{

    public GameObject player;
    private GameObject other;

    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
        if((other = GameObject.Find("First Person Player")) == null)
        {
            player.SetActive(true);
        }
        else
        {
            Destroy(player);
        }
    }
}
