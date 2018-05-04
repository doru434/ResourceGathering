using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Transform[] ObjectName;
	// Use this for initialization
	void Start () {
        GetChilds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void GetChilds(Actor actor)
    {
       int count = transform.GetChildCount();
       ObjectName = new Transform[count];
       for (int i=0;i<count;i++)
        {
            ObjectName[i] = transform.GetChild(i);
            if( ObjectName[i].GetComponent<Text>())
            {
                Text temp = ObjectName[i].GetComponent<Text>();
                if(temp.name == "Object_name")
                {
                    //temp.text = 
                }
            }
        }
       
    }
}
