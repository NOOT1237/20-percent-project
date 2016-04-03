using UnityEngine;
using System.Collections;

public class BenchLighting : MonoBehaviour {

	private bool x = true;
	public GameObject bench;
	public string keyPress;

	void Update () {
		if(Input.GetKeyDown(keyPress)){
			x = !x;
		}
		if(x && bench.activeInHierarchy == false){
			bench.SetActive(true);
		}else if(!x && bench.activeInHierarchy == true){
			bench.SetActive(false);
		}
	}
}
