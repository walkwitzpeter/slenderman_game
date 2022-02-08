using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stalk : MonoBehaviour { 
    public GameObject target;
    public float speed;
    public float cooldownTime;
    public float stalkTime;

    public float posX;
    public float posY;
    public float posZ;

    private bool isStalking;

    // Start is called before the first frame update
    void Start()
    {
        isStalking = false;
        transform.position = new Vector3(posX, posY , posZ);
    }

    // OnEnable is called when the object.SetActive(true)
    void OnEnable()
    {
        StartCoroutine(Cooldown());
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (isStalking)
        {
            transform.position = Vector3.MoveTowards(
                new Vector3(transform.position.x, posY, transform.position.z), 
                new Vector3(target.transform.position.x, posY, target.transform.position.z), 
                speed/100);
        }
    }

    IEnumerator Cooldown(){
        isStalking = false;
        yield return new WaitForSeconds(cooldownTime);
        StartCoroutine(StalkPlayer());
        StopCoroutine(Cooldown());
    }

    IEnumerator StalkPlayer(){
        isStalking = true;
        yield return new WaitForSeconds(stalkTime);
        StartCoroutine(Cooldown());
        StopCoroutine(StalkPlayer());
    }
}
