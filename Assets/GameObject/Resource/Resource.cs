using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Resource : Actor
{
    enum GatheringSlots { FirstFree=0, SecondFree, FirstSame, SecondSame, Occupied}
    enum ResourceLeft { Pass = 0, Less, ToMany }
    //public int ResourceAmount;
    private int[] gatherers = new int[2];
    private bool destroyed = false;
    // Use this for initialization
    protected override void Start() {
        base.Start();
        ResourceAmount = 12;
        gatherers[0] = 0;
        gatherers[1] = 0;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
       // ChangeStateOfLight();
        if (Depleted())
        {
            if (gatherers[0] == 0 && gatherers[1] == 0 && destroyed == false)
            {
                destroyed = true;
                DestroySource();
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Unit>())
        {
            Unit eager = other.GetComponent<Unit>();
            int tempID = eager.gameObject.GetInstanceID();

            GatherOrNot(CanGathering(tempID), tempID , eager);
            
            GathererFull(eager);

            ResourceDepleted(eager);

        }

    }
    private void FreeGatheringSlot(Unit eager)
    {
        if (gatherers[0] == eager.gameObject.GetInstanceID())
        {
            gatherers[0] = 0;
        }
        if (gatherers[1] == eager.gameObject.GetInstanceID())
        {
            gatherers[1] = 0;
        }
    }
    /// <summary>
    /// If resource is depleted, then:
    /// - stops gathering
    /// - free gathering slot
    /// - find next resource source
    /// </summary>
    private void ResourceDepleted(Unit eager)
    {
        if (ResourceAmount == 0 && !eager.IsFull())
        {
            eager.SetGathering(false);
            FreeGatheringSlot(eager);
            eager.ChangeStateOfRigidBody(true);
            eager.FindNextSource(0);
        }
    }
    /// <summary>
    /// If gatherer is full, then:
    /// - stops gathering
    /// - free gathering slot
    /// - sending gatherer back to base
    /// </summary>
    private void GathererFull(Unit eager)
    {
        if (eager.IsFull() && this.transform.gameObject.GetInstanceID() == eager.GetGatheringSourceID())
        {
            eager.SetGathering(false);          
            FreeGatheringSlot(eager);
            eager.RememberResourcePosition(this.transform.position);
            eager.ChangeStateOfRigidBody(true);
            eager.ReturnResources();
        }
    }
    /// <summary>
    /// Depending on the slots status chose to gather or find next source.
    /// </summary>
    private void GatherOrNot(int status, int tempID, Unit eager)
    {
        if (eager.GetWantToGather() && ResourceAmount != 0 && this.transform.gameObject.GetInstanceID() == eager.GetGatheringSourceID())
        {
            switch (status)
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
                        eager.FindNextSource(this.gameObject.GetInstanceID());
                        break;
                    }
            }
        }
    }
    /// <summary>
    /// Manage gathering for specific unit.
    /// </summary>
    private void Gather(Unit eager)
    {

        eager.SetGathering(true);       
        eager.SetIsWaiting(false);

        eager.DrawDebugLine(transform.position);
        if(eager.GetStateOfNavMeshAgent())
        {
           eager.ChangeStateOfRigidBody(false);
        }

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
    /// <summary>
    /// Return status of gathering slots.
    /// </summary>
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
    /// <summary>
    /// Return if the resource is depleted.
    /// </summary>
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
    /// <summary>
    /// Destroying resource if depleted.
    /// </summary>
    private void DestroySource()
    {
        UserInput input = FindObjectOfType<UserInput>();
        input.DeselectPreviousActor();
        foreach (Transform child in transform)
        {
            if (child.tag == "TreeDestroy")
            {
                if (child.GetComponent<Light>())
                {
                    child.GetComponent<Light>().enabled = false;
                }
                GameObject.Destroy(child.gameObject);               
            }
            if(child.name== "Lower_trunk")
            {
                child.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<NavMeshObstacle>().enabled = false;
    }
}
