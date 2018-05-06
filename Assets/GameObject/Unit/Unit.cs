﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor {
	// Use this for initialization
    private float moveSpeed = 1;
    private float rotateSpeed = 5;

    public float lastGather;
    private float gatheringSpeed;
    private int gatheringAmount;
    private int resource;


    public int maxResource = 100;
    private Vector3 desiredPosition;
    private bool move;
    private bool rotate;



    protected override void Start () {
        base.Start();
        lastGather = 0.0f;
        gatheringSpeed = 2.0f;
        gatheringAmount = 2;
        resource = 0;
        isSelected = false;
        move = false;
        rotate = false;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        UpdatePosition();
       
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
    public void setResourceCount()
    {
        resource += gatheringAmount;
        lastGather = 0;
        Debug.Log(this.transform.name + this.resource);
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
    public void MoveManager(Vector3 destination, bool isResource, Resource resource)
    {
        if (isResource)
        {
            MoveObject(destination);
            resource.Gather(this);
   
        }
        else if (!isResource)
        {
            MoveObject(destination);
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
}
