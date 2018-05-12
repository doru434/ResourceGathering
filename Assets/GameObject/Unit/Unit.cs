using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor {
    // Use this for initialization
    private GameObject mainBase;
    private Player myPlayer;

    private float moveSpeed = 4;
    private float rotateSpeed = 5;

    private float gatheringSpeed;
    private int gatheringAmount;
    private int resource;

    public float lastGather;
    public int maxResource;

    private Vector3 desiredPosition;
    private bool move;
    private bool rotate;
    private bool wantToGather;
    private bool goingBackToBase;

    private Vector3 basePosition;
    private Vector3 resourcePosition;

    protected override void Start () {
        base.Start();
        
        mainBase = GameObject.FindGameObjectWithTag("Base");
        basePosition = mainBase.transform.position;

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        myPlayer = Player.transform.GetComponent<Player>();

        lastGather = 0.0f;
        gatheringSpeed = 2.0f;
        gatheringAmount = 2;
        resource = 0;
        maxResource = 6;
        wantToGather = false;
        goingBackToBase = false;
        isSelected = false;
        move = false;
        rotate = false;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        UpdatePosition();
        if(resource == maxResource)
        {
            wantToGather = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (goingBackToBase == true || wantToGather == true)
        {
            if (collision.gameObject.tag == "Gatherer")
            {
                Physics.IgnoreCollision(collision.collider, this.transform.GetComponent<BoxCollider>());
            }
        }
    }

    private void UpdatePosition()
    {
        if(move == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        }
        if(rotate == true)
        {
            RotateObject();
        }

        if(desiredPosition == this.transform.position)
        {
            move = false;
            rotate = false;
        }

    }
    public float GetGatheringSpeed()
    {
        return gatheringSpeed;
    }
    public int GetGatheringAmount()
    {
        return gatheringAmount;
    }
    public int GetResource()
    {
        return resource;
    }
    public bool GetWantToGather()
    {
        return wantToGather;
    }
    public bool GetGoingBackToBase()
    {
        return goingBackToBase;
    }
    // Updates resource count of a unit when gathering
    public void SetResourceCount(bool lastPart, int SourceResource)
    {
        if (lastPart == false)
        {
            resource += gatheringAmount;
            lastGather = 0;
        }
        if(lastPart == true)
        {
            resource += SourceResource;
            lastGather = 0;
        }
    }
    // Gives resource to base
    public void TransferResources()
    {
        resource = 0;
        goingBackToBase = false;
        wantToGather = true;
        MoveObject(resourcePosition);
    }
    // Checking if there is a space for resources 
    public bool SpaceForResource()
    {
        if (resource + gatheringAmount <= maxResource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // checking if there is a space for more resources
    public bool IsFull()
    {
        if (resource == maxResource)
            return true;
        else
            return false;
    }
    public void MoveManager(Vector3 destination, bool isResource, Resource resource)
    {
        if (isResource)
        {          
            MoveObject(destination);
            wantToGather = true;
   
        }
        else if (!isResource)
        {
            MoveObject(destination);
            wantToGather = false;
        }

    }
    public void StopMoving()
    {
        desiredPosition=transform.position;
    }
    protected override void MoveObject(Vector3 destination)
    {
        base.MoveObject(destination);
        desiredPosition.x = destination.x;
        desiredPosition.y = this.transform.position.y;
        desiredPosition.z = destination.z;

        if(this.transform.position != desiredPosition)
        {
            move = true;
            rotate = true;
        }
        
    }
    protected override void RotateObject()
    {
        base.RotateObject();

        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, this.transform.position-desiredPosition, Time.deltaTime * rotateSpeed, 0.0f);

        newDir.y = 0;
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    // Sending this unit to base
    public void ReturnResources()
    {
        wantToGather = false;
        goingBackToBase = true;
        MoveObject(basePosition);
    }
    public void RememberResourcePosition(Vector3 resourceLocation)
    {
        resourcePosition = resourceLocation;
    }
    public void FindNextSource()
    {      
        foreach (Resource i in myPlayer.resourcesList)
        {
            if (i.GetResource() != 0)
            {
                MoveManager(i.transform.position, true, i);
                break;
            }
        }
    }
}
