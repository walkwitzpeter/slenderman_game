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
    public TextMeshProUGUI controlsDialogue;

    private bool hasLookedAround;
    private bool hasMovedAround;
    public bool hasClickedRadio;

    private float yRotation;

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

        player = GameObject.Find("First Person Player");
        playerController = GameObject.Find("First Person Player").GetComponent<PlayerController>();
        playerController.canUseDoors = true;
        insideCabin = false;

        yRotation = playerController.transform.localRotation.eulerAngles.y;

        //runs beginning game dialogue
        if(!playerController.beenInCabin)
        {
            StartCoroutine(WalkthroughControls());
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

        if(!hasLookedAround)
        {
            if(yRotation != playerController.transform.localRotation.eulerAngles.y)
            {
                hasLookedAround = true;
            }
        }

        if(!hasMovedAround)
        {
            if(Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
            {
                hasMovedAround = true;
            }
        }
    }

    IEnumerator WalkthroughControls()
    {
        playerController.canUseDoors = false;

        string dialoguePath = "Assets/Text/ControlWalkthrough.txt";
        StreamReader readControls = new StreamReader(dialoguePath, true);

        controlsDialogue.gameObject.SetActive(true);

            controlsDialogue.text = readControls.ReadLine();
            yield return new WaitForSeconds(3f);
            while(!hasLookedAround)
            {
                yield return new WaitForSeconds(3f);
            }

            controlsDialogue.text = readControls.ReadLine();
            yield return new WaitForSeconds(3f);
            while(!hasMovedAround)
            {
                yield return new WaitForSeconds(3f);
            }
            
            controlsDialogue.text = readControls.ReadLine();
            yield return new WaitForSeconds(1f);
            while(!hasClickedRadio)
            {
                yield return new WaitForSeconds(.1f);
            }

        readControls.Close();
        controlsDialogue.gameObject.SetActive(false);
        yield return StartCoroutine(OpeningDialogue());
        StopCoroutine(WalkthroughControls());
    }

    IEnumerator OpeningDialogue()
    {
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

        readDialogue.Close();
        openingDialogue.gameObject.SetActive(false);
        playerController.canUseDoors = true;
        StopCoroutine(OpeningDialogue());
    }
}
