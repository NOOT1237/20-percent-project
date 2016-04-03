using UnityEngine;
using System.Collections;

public class LightFlickerPulse : MonoBehaviour {  
	
	public enum UseType {Color, Intensity, Range, Off}  
	public UseType useType;  
	
	public enum WaveFunction {Noise, Sin, Tri, Sqr, Saw, SawInv}  
	public WaveFunction waveFunction;  

	public bool noiseUpdatePF = true;  
	public bool noiseSmoothFade;  
	public float noiseSmoothSpeed = 4;   
	
	public float timescaleMin = 0.1f;  
	public float coreBase = 0.8f;   
	public float amplitude = 0.4f;   
	public float phase;   
	public float frequency = 0.5f;   
	
	public bool useOutsideLight;  
	public Light[] outsideLight;   
	
	private Color mainColor;   
	private float mainValueI;   
	private float mainValueR;   
	
	private Color[] mainOutColor = new Color[49];   
	private float[] mainOutValueI = new float[49];   
	private float[] mainOutValueR = new float[49];   
	
	private Light lightS;   
	
	private bool error1;   
	private bool error2;   
	private bool assigned;  
	
	private float x;  
	private float y;   
	private float z;  

	private float noiseD;    
	private bool noiseGo;     
	
	public bool displayScriptErrors = true;
	
	
	void Update(){
		if(waveFunction == WaveFunction.Noise && !noiseUpdatePF){
			if(noiseD > frequency){
				noiseD = 0;
				noiseGo = true;
			}else{
				noiseD += Time.deltaTime;
				noiseGo = false;
			}
		}

		if(lightS == null && !useOutsideLight){
			lightS = gameObject.GetComponent<Light>();
		}
		if(!assigned){
			if(GetComponent<Light>() != null){
				mainColor = GetComponent<Light>().color;
				mainValueI = GetComponent<Light>().intensity;
				mainValueR = GetComponent<Light>().range;
				if(error1 && displayScriptErrors){
					Debug.Log ("FIXED ERROR REPORT: The missing Light component on the gameObject " + "'" +gameObject.name + "'" + " has been successfully re-established!", gameObject);
					error1 = false;
				}
				assigned = true;
			}else if(!error1 && displayScriptErrors && !useOutsideLight){
				Debug.LogError ("ERROR MISSING COMPONENT: The gameObject " + "'" +gameObject.name + "'" + " does not have a Light component attached to it. If you wish to use a Light from another gameObject, please select the boolean variable 'Use Outside Light' for this feature.", gameObject);
				error1 = true;
			}
			if(outsideLight.Length > 0 && outsideLight.Length < 50 && useOutsideLight){
				for(int i = 0; i < outsideLight.Length; i++){
					if(outsideLight[i] == null && displayScriptErrors && !error2){
						error2 = true;
						Debug.LogError ("ERROR EMPTY VARIABLE: The gameObject " + "'" +gameObject.name + "'" + " does not have a gameObject with a Light component attached to the variable 'Outside Light' index number " + "[ " + i + " ], to fix this either, assign a gameObject with a Light component in this space or, lower the size amount to fit your assigned lights. If you do not wish to use this feature, un-tick the boolean variable 'Use Outside Light.'", gameObject);
						Debug.LogError ("VARIABLE ADDITIONAL INFORMATION: This feature does not allow for fixing after the error has occurred due to strict limitations of the 'for' loop, you must replay with the Light component attached to the variable 'Outside Light' index number " + "[ " + i + " ], correctly.", gameObject);
					}else if(!error2){
						mainOutColor[i] = outsideLight[i].color;
						mainOutValueI[i] = outsideLight[i].intensity;
						mainOutValueR[i] = outsideLight[i].range;
					}
				}
				if(!error2){
					assigned = true;
				}
			}else if(!(outsideLight.Length < 50) && displayScriptErrors){
				Debug.LogError ("ERROR EXCEEDING ARRAY LIMITATIONS: Due to this script having to pre-load other array variables to gather all the Light components information a decision was made on how many arrays as max should be pre-defined to remove any performance issues. To fix this error you could modify this script to allocate more available spots located in the defining variables section or remove some or many Light components from the array list on the gameObject " + "'" +gameObject.name + "'. The defined Light components past the 48th index number will not work under this script.", gameObject);
			}
		}
		if(((!error1 && !useOutsideLight) || (!error2 && useOutsideLight)) && assigned && useType != UseType.Off && Time.timeScale > timescaleMin){
			CalWaveFunc();
			if(useOutsideLight){
				for(int i = 0; i < outsideLight.Length; i++){
					if(useType == UseType.Color){
						outsideLight[i].color = mainOutColor[i] * (z);
					}else if(useType == UseType.Intensity){
						outsideLight[i].intensity = mainOutValueI[i] * (z);
					}else if(useType == UseType.Range){
						outsideLight[i].range = mainOutValueR[i] * (z);
					}
					if(mainOutValueI[i] != outsideLight[i].intensity && (useType != UseType.Intensity)){
						outsideLight[i].intensity = mainOutValueI[i];
					}
					if(mainOutValueR[i] != outsideLight[i].range && (useType != UseType.Range)){
						outsideLight[i].range = mainOutValueR[i];
					}
					if(mainOutColor[i] != outsideLight[i].color && (useType != UseType.Color)){
						outsideLight[i].color = mainOutColor[i];
					}
				}
			}else{
				if(useType == UseType.Color){
					lightS.color = mainColor * (z);
				}else if(useType == UseType.Intensity){
					lightS.intensity = mainValueI * (z);
				}else if(useType == UseType.Range){
					lightS.range = mainValueR * (z);
				}
			}
		}  
		if(!useOutsideLight && !error1){
			if(mainValueI != lightS.intensity && (useType != UseType.Intensity)){
				lightS.intensity = mainValueI;
			}
			if(mainValueR != lightS.range && (useType != UseType.Range)){
				lightS.range = mainValueR;
			}
			if(mainColor != lightS.color && (useType != UseType.Color)){
				lightS.color = mainColor;
			}
		}
	}
	
	
	void CalWaveFunc(){
		
		x = (Time.time + phase)*frequency;
		x = x - Mathf.Floor(x);
		
		if(waveFunction == WaveFunction.Sin){
			y = Mathf.Sin(x*2*Mathf.PI);
		}else if(waveFunction == WaveFunction.Tri){
			if(x<0.5f){
				y=4.0f*x-1.0f;
			}else{
				y=-4.0f*x-3.0f;
			}
		}else if(waveFunction == WaveFunction.Sqr){
			if(x<0.5f){
				y=1.0f;
			}else{
				y=-1.0f;
			}
		}else if(waveFunction == WaveFunction.Saw){
			y=x;
		}else if(waveFunction == WaveFunction.SawInv){
			y=1.0f-x;
		}else if(waveFunction == WaveFunction.Noise){
			if(noiseGo || noiseUpdatePF){
				y= 1-(Random.value*2);
			}
		}
		if(noiseSmoothFade && waveFunction == WaveFunction.Noise){
			z = Mathf.Lerp(z, (y*amplitude)+coreBase, Time.deltaTime*noiseSmoothSpeed);
		}else{
			z = (y*amplitude)+coreBase;
		}
	}
}



