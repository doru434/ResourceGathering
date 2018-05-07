using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Actor {
    private int ResourceAmount;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 0;

    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Unit>())
        {
            Unit unit = other.transform.GetComponent<Unit>();
            if(unit.getGoingBackToBase())
            { 
                ResourceAmount+=unit.getResource();
                unit.TransferResources();
            }
        }
    }
    public int getResource()
    {
        return ResourceAmount;
    }
}
