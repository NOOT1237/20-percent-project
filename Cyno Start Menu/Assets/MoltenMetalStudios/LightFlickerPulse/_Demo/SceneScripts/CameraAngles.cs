using UnityEngine;
using System.Collections;

public class CameraAngles : MonoBehaviour {

	public Camera[] angles;
	public int onCam;


	void Update() {
		if(Input.GetKeyDown("v")){
			angles[onCam].enabled = false;
			onCam++;
			if(onCam != angles.Length){
				angles[onCam].enabled = true;
			}else{
				onCam = 0;
				angles[0].enabled = true;
			}
		}
	}
}

