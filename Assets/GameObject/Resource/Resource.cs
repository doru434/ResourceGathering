using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    private int ResourceAmount;
    // Use this for initialization
    protected override void Start () {
        ResourceAmount = 1000;
    }

    // Update is called once per frame
    protected override void Update () {
		
	}
    private void ChangeResource(int number)
    {
        ResourceAmount -= ResourceAmount;
    }
    public int GetResource()
    {
        return ResourceAmount;
    }
    private void isUnit()
    {
        Collider collider = transform.GetComponent<Collider>();
       
    }
}
