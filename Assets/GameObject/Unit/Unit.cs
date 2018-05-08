using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor {
	// Use this for initialization
    private float moveSpeed = 2;
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
        basePosition = new Vector3(3.5f, 0.0f, 18.0f);

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
                Physics.IgnoreCollision(collision.collider, this.transform.GetComponent<Collider>());
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
    public float getGatheringSpeed()
    {
        return gatheringSpeed;
    }
    public int getGatheringAmount()
    {
        return gatheringAmount;
    }
    public int getResource()
    {
        return resource;
    }
    public bool getWantToGather()
    {
        return wantToGather;
    }
    public bool getGoingBackToBase()
    {
        return goingBackToBase;
    }
    public void setResourceCount()
    {
        resource += gatheringAmount;
        lastGather = 0;
    }
    public void TransferResources()
    {
        resource = 0;
        goingBackToBase = false;
        wantToGather = true;
        MoveObject(resourcePosition);
    }
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
    public bool isFull()
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
    public void returnResources()
    {
        wantToGather = false;
        goingBackToBase = true;
        MoveObject(basePosition);
    }
    public void rememberResourcePosition(Vector3 resourceLocation)
    {
        resourcePosition = resourceLocation;
    }

}
