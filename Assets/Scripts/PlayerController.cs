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

    // Start is called before the first frame update
    void Start()
    {
        //brings camera to correct positon
        this.transform.position = new Vector3(500, 6, 500);

        walkingSpeed = speed;
        enduranceMax = endurance;
        score = 0;
        scoreText.gameObject.SetActive(false);
    }

    void Update()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;

        if(this.transform.position.y > 6)
        {
            this.transform.position = new Vector3(posX, 6, posZ);
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
                        && hit.transform.gameObject.layer == 7)
                {
                    if(SceneManager.GetActiveScene().name.Equals("Playing Field"))
                    {
                        SceneManager.LoadScene("Cabin");
                    }
                    else if(SceneManager.GetActiveScene().name.Equals("Cabin"))
                    {
                        SceneManager.LoadScene("Playing Field");
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
}


