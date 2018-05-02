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
      
       // Debug.Log(this.transform.name + isSelected);
    }
}
