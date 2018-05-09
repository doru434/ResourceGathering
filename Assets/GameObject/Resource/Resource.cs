using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Actor
{
    private int ResourceAmount;
    private Player myPlayer;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ResourceAmount = 3;
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        myPlayer = Player.transform.GetComponent<Player>();
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
        Debug.Log(other.name);
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);
    }
    void OnTriggerStay(Collider other)
    {
       if( other.GetComponent<Unit>())
        {
            Unit eager = other.GetComponent<Unit>();
            if (eager.getWantToGather() && ResourceAmount!=0)
            {

                int gatherAmount = eager.getGatheringAmount();

                if (ResourceAmount - gatherAmount >= 0)
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.getGatheringSpeed())
                    {
                        ResourceAmount -= gatherAmount;
                        eager.setResourceCount(false, ResourceAmount);
                    }
                }
                else if (ResourceAmount != 0 && ResourceAmount < gatherAmount)
                {
                    eager.StopMoving();
                    eager.lastGather += Time.deltaTime;
                    if (eager.lastGather >= eager.getGatheringSpeed())
                    {
                        eager.setResourceCount(true, ResourceAmount);
                        ResourceAmount = 0;
                    }
                }
                   
            }
            if(eager.isFull())
            {
                eager.rememberResourcePosition(this.transform.position);
                eager.returnResources();
            }
            if (ResourceAmount == 0 && !eager.isFull())
            {
                foreach (Resource i in myPlayer.resourcesList)
                {
                    if (i.getResource() != 0)
                    {
                        eager.MoveManager(i.transform.position, true, i);
                        break;
                    }
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
