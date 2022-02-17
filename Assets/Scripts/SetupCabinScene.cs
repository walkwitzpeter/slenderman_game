using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SetupCabinScene : MonoBehaviour
{
    public GameObject newPlayer;
    private GameObject player;
    public PlayerController playerController;
    private bool insideCabin;

    public TextMeshProUGUI openingDialogue;

    void Start()
    {
        //turns off the Extra First Person Player if there is one
        newPlayer.SetActive(false);
        if((player = GameObject.Find("First Person Player")) == null)
        {
            newPlayer.SetActive(true);
        }
        else
        {
            Destroy(newPlayer);
        }

        playerController.canUseDoors = true;
        player = GameObject.Find("First Person Player");
        insideCabin = false;

        //runs beginning game dialogue
        if(!playerController.beenInCabin)
        {
             StartCoroutine(OpeningDialogue());
        }
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

    IEnumerator OpeningDialogue()
    {

        playerController.canUseDoors = false;

        string dialoguePath = "Assets/Text/OpeningDialogue.txt";
        string line;
        StreamReader readDialogue = new StreamReader(dialoguePath, true);

        openingDialogue.gameObject.SetActive(true);

        openingDialogue.color = Color.cyan;

        while((line = readDialogue.ReadLine()) != "")
        {
            openingDialogue.text = line;
            yield return new WaitForSeconds(4f);
        }

        openingDialogue.color = Color.blue;

        while((line = readDialogue.ReadLine()) != "")
        {
            openingDialogue.text = line;
            yield return new WaitForSeconds(4f);
        }

        openingDialogue.color = Color.white;

        while((line = readDialogue.ReadLine()) != null)
        {
            openingDialogue.text = line;
            yield return new WaitForSeconds(4f);
        }

        openingDialogue.gameObject.SetActive(false);
        //playerController.beenInCabin = true;
        playerController.canUseDoors = true;
        StopCoroutine(OpeningDialogue());
    }
}
