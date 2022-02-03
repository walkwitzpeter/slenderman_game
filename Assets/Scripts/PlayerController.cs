using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    public CharacterController controller;
    public Camera cam;
    public float endurance;

    private float walkingSpeed;
    private bool isRunning;
    private float enduranceMax;

    // Start is called before the first frame update
    void Start()
    {
        //brings camera to correct positon
        this.transform.position = new Vector3(0, 1, 0);

        walkingSpeed = speed;
        enduranceMax = endurance;
    }

    void Update()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;

        if(this.transform.position.y > 1.25)
        {
            this.transform.position = new Vector3(posX, 1, posZ);
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
                if (interactable != null)
                {
                    Destroy(interactable.gameObject);
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
}


