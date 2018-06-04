using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDsc : MonoBehaviour {
    private Transform[] HUDChilds;
    private string Object_name;
    private string Object_Resources;
    // Use this for initialization
    void Start () {
        SetChilds();
    }
	
	// Update is called once per frame
	void Update () {
        if (GetHUDTextObjectByName("Object_name"))
        {
            Text tempObject_name = GetHUDTextObjectByName("Object_name");
            tempObject_name.text = Object_name;
        }
        if (GetHUDTextObjectByName("Object_Resources"))
        {
            Text tempObject_Resources = GetHUDTextObjectByName("Object_Resources");
            tempObject_Resources.text = Object_Resources;
        }
    }
    public void SetChilds()
    {
       int count = transform.childCount;
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
    public void UpdateHUD(string name, int resourceAmount)
    {
            Object_name = name;
            Object_Resources= resourceAmount.ToString();
    }
}
