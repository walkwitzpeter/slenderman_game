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

    public bool beenInCabin = false;
    public bool canUseDoors = true;

    // Start is called before the first frame update
    void Start()
    {
        walkingSpeed = speed;
        enduranceMax = endurance;
        scoreText.gameObject.SetActive(false);
        score = 0;
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
                    Destroy(interactable.gameObject);
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
                    if(SceneManager.GetActiveScene().name.Equals("Playing Field"))
                    {
                        TransitionScenes("Cabin", "Playing Field", new Vector3(500, 4.1f, 500));
                    }
                    else if(SceneManager.GetActiveScene().name.Equals("Cabin"))
                    {
                        TransitionScenes("Playing Field", "Cabin", new Vector3(462, 4.1f, 465));
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
}