using UnityEngine;
using System.Collections;

public class Flashlightlight : MonoBehaviour {
public bool on = false;
	// Use this for initialization
	void Start () {
	
	}
	
	
 
 void Update()
 {
     if(Input.GetKeyDown(KeyCode.E))
         on = !on;
     if(on)
         GetComponent<Light>().enabled = true;
     else if(!on)
         GetComponent<Light>().enabled = false;
 }
 }
	

