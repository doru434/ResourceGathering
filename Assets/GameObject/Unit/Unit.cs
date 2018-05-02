using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor {
	// Use this for initialization
	protected override void Start () {
        base.Start();
        isSelected = false;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();     
      
      // Debug.Log(this.transform.name + this.transform.position);
    }
    public override void MoveObect(Vector3 destination)
    {
        base.MoveObect(destination);
        Vector3 movement = new Vector3(0, 0, 0);
        movement.x = destination.x;
        movement.y = this.transform.position.y;
        movement.z = destination.z;
        
     
        this.transform.position = movement;
    }
}
