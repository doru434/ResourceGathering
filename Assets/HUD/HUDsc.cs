using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDsc : MonoBehaviour {
    private Transform[] HUDChilds;
    private string Object_name;
    private string Object_Resources;
    private bool isSelected = false;
    private bool isBuilding = false;
    // Use this for initialization
    void Start () {
        isSelected = false;
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
        if (GetHUDImageObjectByName("SelectedBar"))
        {
            Image tempObject_Resources = GetHUDImageObjectByName("SelectedBar");
            tempObject_Resources.gameObject.SetActive(isSelected);
        }
        if (GetHUDButtonObjectByName("GathererButton"))
        {
            Button tempObject_Resources = GetHUDButtonObjectByName("GathererButton");
            tempObject_Resources.gameObject.SetActive(isBuilding);
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
}
