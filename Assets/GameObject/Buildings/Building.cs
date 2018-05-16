using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Actor {
    private int ResourceAmount;
    private int MuleCost;
    public Transform newMule;
    private Vector3 spawnPoint;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 0;
        MuleCost = 20;
        spawnPoint = new Vector3(transform.position.x-4, transform.position.y + 2.5f, transform.position.z - 6);
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();  
        if(ResourceAmount >= MuleCost)
        {
            if (Physics.CheckBox(spawnPoint, new Vector3(1.0f, 1.0f, 0.5f)))
            {
                spawnPoint.x += 2.0f;
            }
            else
            {
                Instantiate(newMule, spawnPoint, Quaternion.identity);
                ResourceAmount -= MuleCost;
                spawnPoint = new Vector3(transform.position.x-4, transform.position.y + 2.5f, transform.position.z - 6);
            }
            
           
        }

	}
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Unit>())
        {
            Unit unit = other.transform.GetComponent<Unit>();
            if(unit.GetGoingBackToBase())
            { 
                ResourceAmount+=unit.GetResource();
                unit.TransferResources();
            }
        }
    }
    public int GetResource()
    {
        return ResourceAmount;
    }
}
