using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    private int ResourceAmount;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 7;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        if(Depleted())
        {
            DestroySource();
        }
        
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
                    eager.StopMoving();
                    if (ResourceAmount - gatherAmount >= 0)
                    {
                        eager.lastGather += Time.deltaTime;
                        if (eager.lastGather >= eager.getGatheringSpeed())
                        {
                            ResourceAmount -= gatherAmount;
                            eager.setResourceCount(false, ResourceAmount);
                        }
                    }
                    if(ResourceAmount != 0 && ResourceAmount < gatherAmount)
                    {
                        eager.lastGather += Time.deltaTime;
                        if (eager.lastGather >= eager.getGatheringSpeed())
                        {
                            eager.setResourceCount(true , ResourceAmount);
                            ResourceAmount = 0;
                        }
                    }
                    //else find next gathering point
                }
                if(eager.isFull())
                {
                    eager.rememberResourcePosition(this.transform.position);
                    eager.returnResources();
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
    private bool Depleted()
    {
        if(ResourceAmount==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void DestroySource()
    {
        //Destroy(this);
    }
}
