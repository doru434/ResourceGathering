using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
    private int ResourceAmount;
	// Use this for initialization
	void Start () {
        ResourceAmount = 1000;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeResource(int number)
    {
        ResourceAmount -= ResourceAmount;
    }
}
