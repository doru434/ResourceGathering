using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    enum ResourceLeft { Pass=0, Less, ToMany}
    private int ResourceAmount;
   
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 11;       
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

                if (ResourceAmount - gatherAmount >= 0 && eager.EnoughtSpace())
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.GetGatheringSpeed())
                    {
                        ResourceAmount -= gatherAmount;
                        eager.SetResourceCount((int)ResourceLeft.Pass, ResourceAmount);
                    }
                }
                else if (ResourceAmount != 0 && ResourceAmount < gatherAmount)
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.GetGatheringSpeed())
                    {
                        eager.SetResourceCount((int)ResourceLeft.Less, ResourceAmount);
                        ResourceAmount = 0;
                    }
                }
                else if (ResourceAmount - gatherAmount >= 0 && !eager.EnoughtSpace())
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.GetGatheringSpeed())
                    {
                        int spaceLeft = eager.ResourceSpace();
                        ResourceAmount -= spaceLeft;
                        eager.SetResourceCount((int)ResourceLeft.ToMany, ResourceAmount);
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
        UserInput input = FindObjectOfType<UserInput>();
        input.DeselectPreviousActor();
        Destroy(gameObject);
       
    }
}
