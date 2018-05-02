using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerName;
    private HUD hud;

	// Use this for initialization
	void Start () {
       hud =  transform.GetComponentInChildren<HUD>();
    }
	
	// Update is called once per frame
	void Update () {

	}
}
