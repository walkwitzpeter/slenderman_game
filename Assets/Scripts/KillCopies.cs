using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class KillCopies : MonoBehaviour
 {
     static KillCopies instance;
     private int count = 0;
 
     void Awake()
     {
         if(instance == null)
         {    
             count++;
             Debug.Log(count);
             instance = this; // In first scene, make us the instance
             DontDestroyOnLoad(gameObject);
         }
         else if(instance != this)
         {
            Debug.Log("hi");
             Destroy(this.gameObject); // If object copy wakes up, destroy itself
         }
     } 
 }
