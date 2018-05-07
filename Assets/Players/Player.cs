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
        hUDsc.UpdateHUD( name, resourceAmount);
    }
    
}
