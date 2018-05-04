using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor {
	// Use this for initialization
    private float moveSpeed = 3;
    private float rotateSpeed = 5;
    private Vector3 desiredPosition;
    private bool move;
    private bool rotate;
    protected override void Start () {
        base.Start();
        isSelected = false;
        move = false;
        rotate = false;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        UpdatePosition();
       // Debug.Log(this.transform.name + this.isSelected);
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
    public override void MoveObject(Vector3 destination)
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
