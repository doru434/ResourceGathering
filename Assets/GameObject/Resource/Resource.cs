using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    private int ResourceAmount;
    private Unit tempSlot;
    private Unit gatheringUnit;
    // Use this for initialization
    protected override void Start () {
        ResourceAmount = 1000;

    }

    // Update is called once per frame
    protected override void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
      
        if (other.GetComponent<Unit>())
        {
            tempSlot = other.GetComponent<Unit>();
        }
    }
    void OnTriggerExit(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {
       if( other.GetComponent<Unit>())
        {
            Unit temp = other.GetComponent<Unit>();
            if (gatheringUnit)
            {
                if (temp.name == gatheringUnit.name)
                {
                    int gatherAmount = gatheringUnit.getGatheringAmount();
                    if (temp.SpaceForResource())
                    {
                        if (ResourceAmount - gatherAmount >= 0)
                        {
                            temp.lastGather += Time.deltaTime;
                            if (temp.lastGather >= temp.getGatheringSpeed())
                            {
                                ResourceAmount -= gatherAmount;
                                gatheringUnit.setResourceCount();
                            }
                        }
                        //else find next gathering point
                    }
                    //else go back to base
                }
            }
        }

    }
    public int getResource()
    {
        return ResourceAmount;
    }
    private void ChangeResource(int number)
    {
        ResourceAmount -= ResourceAmount;
    }
    public int GetResource()
    {
        return ResourceAmount;
    }

    public void Gather(Unit unit)
    {
        if (tempSlot)
        {
            if (unit.name == tempSlot.name)
            {
                unit.StopMoving();
                gatheringUnit = tempSlot;
            }
        }
    }
}
