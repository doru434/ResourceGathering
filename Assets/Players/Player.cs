using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string PlayerName;
    private HUDsc hUDsc;

	// Use this for initialization
	void Start () {
        Transform child = transform.GetChild(0);
        hUDsc = child.GetComponent<HUDsc>();
        hUDsc.SetChilds(); 
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    public void UpdateHUD(string name, int resourceAmount)
    {
        Text Object_name = hUDsc.GetHUDTextObjectByName("Object_name");
        Object_name.text = name;
        Text Object_Resources = hUDsc.GetHUDTextObjectByName("Object_Resources");
        Object_Resources.text = resourceAmount.ToString();
    }
    
}
