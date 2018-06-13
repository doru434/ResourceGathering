using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string PlayerName;
    private int ResourceCount;
    private HUDsc hUDsc;
    private GameObject[] resourcesArray;
    public List<Resource> resourcesList;
	// Use this for initialization
	void Start () {
        ResourceCount = 0;
        Transform child = transform.GetChild(0);
        resourcesList = new List<Resource>();
        hUDsc = child.GetComponent<HUDsc>();
        hUDsc.SetStartingResources(ResourceCount);
        hUDsc.SetChilds();
        resourcesArray=GameObject.FindGameObjectsWithTag("Resource");
        InitializeAllResources();
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    public void UpdateHUD(GameObject selected)
    {
        hUDsc.UpdateHUD(selected);
    }
    public void DecreseResourceCount(int cost)
    {
        ResourceCount -= cost;
        hUDsc.UpdatePlayerResources(ResourceCount);
    }
    private void InitializeAllResources()
    {
        foreach(GameObject i in resourcesArray)
        {
            Resource temp = i.transform.GetComponent<Resource>();
            resourcesList.Add(temp);
        }
    }
    public void AddResource(int resource)
    {
        ResourceCount += resource;
        hUDsc.UpdatePlayerResources(ResourceCount);
    }
    public int GetResourceCount()
    {
        return ResourceCount;
    }
    public bool AbleToPay(int MuleCost)
    {
        if(ResourceCount-MuleCost>=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
