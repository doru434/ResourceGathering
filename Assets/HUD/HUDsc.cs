using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDsc : MonoBehaviour {
    private Transform[] HUDChilds;
	// Use this for initialization
	void Start () {
        SetChilds();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetChilds()
    {
       int count = transform.GetChildCount();
       HUDChilds = new Transform[count];
       for (int i=0;i<count;i++)
        {
            HUDChilds[i] = transform.GetChild(i);          
        }
    
    }
    public Transform[] getChilds()
    {
        return HUDChilds;
    }
    public Text GetHUDTextObjectByName(string name)
    {
       
        foreach (Transform i in HUDChilds)
        {
           
            if (i.GetComponent<Text>())
            {
               
                if (i.GetComponent<Text>().name == name)
                {
                    
                    return i.GetComponent<Text>();
                }
            }
        }
        
        return null;
    }
}
