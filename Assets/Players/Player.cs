using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string PlayerName;
    private HUDsc hUDsc;
    private GameObject[] resourcesArray;
    public List<Resource> resourcesList;
	// Use this for initialization
	void Start () {
        Transform child = transform.GetChild(0);
        resourcesList = new List<Resource>();
        hUDsc = child.GetComponent<HUDsc>();
        hUDsc.SetChilds();
        resourcesArray=GameObject.FindGameObjectsWithTag("Resource");
        InitializeAllResources();
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    public void UpdateHUD(string name, int resourceAmount)
    {
        hUDsc.UpdateHUD( name, resourceAmount);
    }
    private void InitializeAllResources()
    {
        foreach(GameObject i in resourcesArray)
        {
            Resource temp = i.transform.GetComponent<Resource>();
            resourcesList.Add(temp);
        }
    }

    
}
