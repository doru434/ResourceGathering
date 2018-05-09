using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Actor {
    private int ResourceAmount;
    private int MuleCost;
    public Transform newMule;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 0;
        MuleCost = 5;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        if(ResourceAmount >= MuleCost)
        {
            Instantiate(newMule, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z - 5), Quaternion.identity);
            ResourceAmount -= MuleCost;
        }

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
