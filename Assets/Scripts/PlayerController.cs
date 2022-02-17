using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    public CharacterController controller;
    public Camera cam;
    public float endurance;

    private float walkingSpeed;
    private bool isRunning;
    private float enduranceMax;

    public TextMeshProUGUI scoreText;
    private int score;

    private string prevSceneName;

    public bool beenInCabin;
    public bool canUseDoors;

    private ArrayList pickupIds;
    public Transform pickupList;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello");
        if(!beenInCabin && SceneManager.GetActiveScene().name.Equals("Playing Field")) StartCoroutine(DelayUpdateOfPickupList());

        walkingSpeed = speed;
        enduranceMax = endurance;
        scoreText.gameObject.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;

        if(this.transform.position.y != 4.1f)
        {
            this.transform.position = new Vector3(posX, 4.1f, posZ);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
        {
            // On a right mouse button click, check for interaction.
            // Make a ray that will look for interactale objects
            Ray detect = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If detect finds an interactable object, interacts with object
            if (Physics.Raycast(detect, out hit, 5))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null
                && hit.transform.gameObject.layer == 6)
                {
                    //Remove the note from the master list of notes, destroy the note
                    string pickupId = hit.transform.gameObject.name;
                    int idNumber = Int32.Parse(pickupId);

                    pickupIds.Remove(idNumber);
                    
                    UpdatePickupValues(interactable.gameObject);
                    interactable.gameObject.SetActive(false);

                    score++;
                    if(score == 4){
                        SceneManager.LoadScene("GameOverScreen");
                    }
                    else{
                        StartCoroutine(UpdateScore());
                    }
                }
                else if(interactable != null 
                        && hit.transform.gameObject.layer == 7
                        && canUseDoors)
                {
                    //use the door
                    if(SceneManager.GetActiveScene().name.Equals("Playing Field"))
                    {
                        TransitionScenes("Cabin", "Playing Field", new Vector3(500, 4.1f, 500));
                    }
                    else if(SceneManager.GetActiveScene().name.Equals("Cabin"))
                    {
                        TransitionScenes("Playing Field", "Cabin", new Vector3(500, 4.1f, 500));
                    }
                } 
            }
        }

        // Lowers endurance while the player is running
        if (isRunning)
        {
            endurance -= 1;
        }

        // Running mechanics
        if (!isRunning)
        {
            // Regain endurance while not running
            if (endurance < enduranceMax)
            {
                endurance += .75f;
            }

            // Starts running
            if (Input.GetKeyDown("space") && endurance > 100)
            {
                isRunning = true;
                speed *= 2;
            }
        }

        // Stops running under certain conditions
        if (Input.GetKeyUp("space") || (isRunning && endurance <= 0))
        {
            isRunning = false;
            speed = walkingSpeed;
        }
    }

    IEnumerator UpdateScore()
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score " + score + "/?";
        yield return new WaitForSeconds(3f);
        scoreText.gameObject.SetActive(false);
        StopCoroutine(UpdateScore());
    }

        private void TransitionScenes(string nextScene, string prevScene, Vector3 playerPosition)
    {
        //prevScene = prevScene;
        SceneManager.LoadScene(nextScene);
        this.transform.position = playerPosition;
    }

    private void SetupPickupList()
    {
        Debug.Log("hi");
        score = 0;
        beenInCabin = true;
        pickupIds = new ArrayList();
        for(int i = 0; i < pickupList.childCount; i++)
        {
            Debug.Log(pickupList.GetChild(i).gameObject.name);
            pickupIds.Add(pickupList.GetChild(i).gameObject.name);
        }
    }

    IEnumerator DelayUpdateOfPickupList()
    {
        yield return new WaitForSeconds(1f);
        pickupList = GameObject.Find("PickupList").transform;

        if(!beenInCabin)
        {
            SetupPickupList();
        }

        RemoveAlreadyCollectedItems();
        StopCoroutine(DelayUpdateOfPickupList());
    }

    private void RemoveAlreadyCollectedItems()
    {
        int j = 0;
        for(int i = 0; i < pickupList.childCount; i++)
        {
            if(!pickupIds.Contains(i.ToString()))
            {
                GameObject pickup = pickupList.GetChild(i).gameObject;
                pickup.SetActive(false);
                j++;
            }
            Debug.Log(j);
        }
    }

    private void UpdatePickupValues(GameObject pickedUpPickup)
    {
        pickedUpPickup.name = "666";
        for(int i = 0; i < pickupList.childCount; i++)
        { 
            string temp = (string) pickupIds[i];
            pickupList.GetChild(i).name = i.ToString();
        }
    }
}