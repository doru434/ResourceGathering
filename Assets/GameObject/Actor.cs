using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    protected int hp;
    public bool isSelected;
    protected int ResourceAmount;
    // Use this for initialization
    protected virtual void Start () {
		
	}

    // Update is called once per frame
    protected virtual void Update () {
		
	}
    protected virtual void MoveObject(Vector3 Destination)
    {

    }
    protected virtual void RotateObject()
    {

    }
    public void ChangeStateOfLight()
    {
        if(transform.GetComponentInChildren<Light>())
        {
            Light SelectionLight = transform.GetComponentInChildren<Light>();
            if (isSelected == true)
            {
                SelectionLight.enabled = true;
            }
            if (isSelected == false)
            {
                SelectionLight.enabled = false;
            }
        }
    }
    public int Resource{ get { return ResourceAmount; } }
}
