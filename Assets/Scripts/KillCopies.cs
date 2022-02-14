using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class KillCopies : MonoBehaviour
 {
     static KillCopies instance;
 
     void Awake()
     {
         if(instance == null)
         {    
             instance = this; // In first scene, make us the singleton.
             DontDestroyOnLoad(gameObject);
         }
         else if(instance != this)
             Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
     } 
 }
 
/*  public class KillCopies : MonoBehaviour
 {
     static KillCopies instance;
 
     public KillCopies GetInstance()
     {
         if(instance == null)
         {
             // Something similar to this to load and create the object when needed.
             Object prefab = Resources.Load("Path/To/Prefab"); // Look up the docs ;)
             GameObject go = Instantiate(prefab);
             instance = GetComponent<KillCopies>();
             DontDestroyOnLoad(go);
         }
         return instance;
     } 
 } */
