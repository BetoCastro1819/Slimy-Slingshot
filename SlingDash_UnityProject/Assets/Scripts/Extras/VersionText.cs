using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour {

    Text versionText;

	// Use this for initialization
	void Start () {
        versionText = GetComponent<Text>();

        versionText.text = "V" + Application.version;
	}
}
