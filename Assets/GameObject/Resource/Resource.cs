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
    void OnTriggerEnter(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {
       if( other.GetComponent<Unit>())
        {
            Unit eager = other.GetComponent<Unit>();
            if (eager.getWantToGather())
            {

                    int gatherAmount = eager.getGatheringAmount();
                    if (eager.SpaceForResource())
                    {
                        if (ResourceAmount - gatherAmount >= 0)
                        {
                            eager.lastGather += Time.deltaTime;
                            if (eager.lastGather >= eager.getGatheringSpeed())
                            {
                                ResourceAmount -= gatherAmount;
                                eager.setResourceCount();
                            }
                        }
                        //else find next gathering point
                    }
                    //else go back to base
                
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


}
