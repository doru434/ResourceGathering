using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDsc : MonoBehaviour {
    private Transform[] HUDChilds;
    private Button muleButton;
    private Building mainBase;
    private Player player;
    private int MuleCost;
    private string Object_name;
    private string Object_Resources;
    private string Player_Resources;
    private bool isSelected = false;
    private bool isBuilding = false;
    // Use this for initialization
    void Start () {
        isSelected = false;
        SetChilds();
        muleButton = GetHUDButtonObjectByName("GathererButton");
        mainBase= FindObjectOfType<Building>();

        muleButton.onClick.AddListener(TaskOnClick);
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
            if (isBuilding == true)
            {
                tempObject_Resources.text = "";
            }
        }
        if (GetHUDTextObjectByName("Text_PlayerResources"))
        {
            Text tempObject_Resources = GetHUDTextObjectByName("Text_PlayerResources");
            tempObject_Resources.text = Player_Resources;
        }
        if (GetHUDImageObjectByName("SelectedBar"))
        {
            Image tempObject_Resources = GetHUDImageObjectByName("SelectedBar");
            tempObject_Resources.gameObject.SetActive(isSelected);
            if (isBuilding == true)
            {
                tempObject_Resources.GetComponentInChildren<Text>().text = "";
            }
        }
        if (GetHUDButtonObjectByName("GathererButton"))
        {
            Button tempObject_Resources = GetHUDButtonObjectByName("GathererButton");
            tempObject_Resources.gameObject.SetActive(isBuilding);
        }
    }
    void TaskOnClick()
    {
       bool succes = mainBase.CreateMule();
        if(succes==false)
        {
            Debug.Log("Spawning failed");
        }
    }
    public void SetStartingResources(int ResourceCount)
    {
        Player_Resources = ResourceCount.ToString();
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
    private Text GetHUDTextObjectByName(string name)
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
    private Image GetHUDImageObjectByName(string name)
    {

        foreach (Transform i in HUDChilds)
        {

            if (i.GetComponent<Image>())
            {

                if (i.GetComponent<Image>().name == name)
                {

                    return i.GetComponent<Image>();
                }
            }
        }

        return null;
    }
    private Button GetHUDButtonObjectByName(string name)
    {

        foreach (Transform i in HUDChilds)
        {

            if (i.GetComponent<Button>())
            {

                if (i.GetComponent<Button>().name == name)
                {

                    return i.GetComponent<Button>();
                }
            }
        }

        return null;
    }
    public void UpdateHUD(GameObject selectedObject)
    {
        if (selectedObject.name == "Ground")
        {
            Object_name = "";
            isSelected = false;
            isBuilding = false;
            Object_Resources = "";
        }
        if (selectedObject.name != "Ground")
        {
            Object_name = selectedObject.name;
            isSelected = true;
            if (selectedObject.GetComponent<Actor>())
            {
                Object_Resources = selectedObject.GetComponent<Actor>().Resource.ToString();
            }
            if (selectedObject.GetComponent<Building>())
            {
                isBuilding = true;
                
            }
            else
            {
                isBuilding = false;
            }
        }
    }
    public void UpdatePlayerResources(int Resources)
    {
        Player_Resources = Resources.ToString();
    }
}
