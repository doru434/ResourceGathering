using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    enum GatheringSlots { FirstFree=0, SecondFree, FirstSame, SecondSame, Occupied}
    enum ResourceLeft { Pass = 0, Less, ToMany }
    public int ResourceAmount;
    private int[] gatherers = new int[2];
    // Use this for initialization
    protected override void Start() {
        base.Start();
        gatherers[0] = 0;
        gatherers[1] = 0;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (Depleted())
        {
            DestroySource();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Unit>())
        {
            Unit eager = other.GetComponent<Unit>();
            int tempID = eager.gameObject.GetInstanceID();
            int slots = CanGathering(tempID);
            if (eager.GetWantToGather() && ResourceAmount != 0 && this.transform.gameObject.GetInstanceID() == eager.GetGatheringSourceID())
            {
                switch (slots)
                {
                    case 0:
                        {
                            gatherers[0] = tempID;
                            Gather(eager);
                            break;
                        }
                    case 1:
                        {
                            gatherers[1] = tempID;
                            Gather(eager);
                            break;
                        }
                    case 2:
                        {
                            Gather(eager);
                            break;
                        }
                    case 3:
                        {
                            Gather(eager);
                            break;
                        }
                    case 4:
                        {
                            eager.FindNextSource();
                            break;
                        }
                }
            }
            if (eager.IsFull())
            {
                if(gatherers[0]==eager.gameObject.GetInstanceID())
                {
                    gatherers[0] = 0;
                }
                if (gatherers[1] == eager.gameObject.GetInstanceID())
                {
                    gatherers[1] = 0;
                }
                eager.SetGathering(false);
                eager.RememberResourcePosition(this.transform.position);
                eager.ReturnResources();
            }
            if (ResourceAmount == 0 && !eager.IsFull())
            {
                eager.SetGathering(false);
                eager.FindNextSource();
            }
        }

    }
    private void Gather(Unit eager)
    {
        eager.SetGathering(true);
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
    private int CanGathering(int id)
    {
        if (gatherers[0]==id)
        {
            return (int)GatheringSlots.FirstSame;
        }
        if (gatherers[1] == id)
        {
            return (int)GatheringSlots.SecondSame;
        }
        if(gatherers[0]==0)
        {
            return (int)GatheringSlots.FirstFree;
        }
        if(gatherers[1]==0)
        {
            return (int)GatheringSlots.SecondFree;
        }
        return (int)GatheringSlots.Occupied;
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
