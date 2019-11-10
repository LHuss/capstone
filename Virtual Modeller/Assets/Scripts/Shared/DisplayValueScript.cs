using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayValueScript : MonoBehaviour {

	protected Text valueText;

	void Start () {
		valueText = GetComponent<Text>();
	}
	
	public void UpdateText (float value) {
		valueText.text = value.ToString();
	}
}
