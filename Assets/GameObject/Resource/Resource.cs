using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    private int ResourceAmount;
   
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 34;       
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        if(Depleted())
        {
            DestroySource();
        }
        
	}
    void OnTriggerStay(Collider other)
    {
       if( other.GetComponent<Unit>())
        {
            Unit eager = other.GetComponent<Unit>();
            if (eager.GetWantToGather() && ResourceAmount!=0)
            {

                int gatherAmount = eager.GetGatheringAmount();

                if (ResourceAmount - gatherAmount >= 0)
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.GetGatheringSpeed())
                    {
                        ResourceAmount -= gatherAmount;
                        eager.SetResourceCount(false, ResourceAmount);
                    }
                }
                else if (ResourceAmount != 0 && ResourceAmount < gatherAmount)
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.GetGatheringSpeed())
                    {
                        eager.SetResourceCount(true, ResourceAmount);
                        ResourceAmount = 0;
                    }
                }
                   
            }
            if(eager.IsFull())
            {
                eager.RememberResourcePosition(this.transform.position);
                eager.ReturnResources();
            }
            if (ResourceAmount == 0 && !eager.IsFull())
            {
                eager.FindNextSource();
            }           
        }

    }
    public int GetResource()
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
        Destroy(gameObject);
    }
}
