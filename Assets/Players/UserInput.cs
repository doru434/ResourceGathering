using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {
    private Player player = null;
    private GameObject Selected=null;
    private bool SomethingSelected = false;
    private static int CAMERA_SPEED = 20;
    private static int ZOOM_SPEED = 10;
    private static int MIN_ZOOM = 5;
    private static int MAX_ZOOM = 20;
    private static int ROTATE_SPEED = 100;


    // Use this for initialization
    void Start () {
        player = transform.root.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        MoveCamera();
        RotateCamera();
        GetMouseClick();
        if(Selected)
        {
            HudUpdate(Selected.name);
        }
    }
    public bool GetSomethingSelected() { return SomethingSelected; }
    public GameObject GetSelected() { return Selected; }
    // moves camera in x-z axis
    private void MoveCamera()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;

        //x--z movement
        if (Input.GetKey(KeyCode.A))
        {
            destination.x -= CAMERA_SPEED * Time.deltaTime; 
        }
       if (Input.GetKey(KeyCode.D))
        {
            destination.x += CAMERA_SPEED * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            destination.z += CAMERA_SPEED * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            destination.z -= CAMERA_SPEED * Time.deltaTime;
        }

        //zoom in, zoom out
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            destination.y -= ZOOM_SPEED * Input.GetAxis("Mouse ScrollWheel");
        }

        if(destination.y < MIN_ZOOM)
        {
            destination.y = MIN_ZOOM;
        }
        if(destination.y > MAX_ZOOM)
        {
            destination.y = MAX_ZOOM;
        }
        if(destination != origin)
        {
            Camera.main.transform.position = destination;
        }
    }
    // rotates camera in x-y axis
    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            destination.y += Input.GetAxis("Mouse X") * ROTATE_SPEED;
            destination.x -= Input.GetAxis("Mouse Y") * ROTATE_SPEED;
        }

        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ROTATE_SPEED);
        }
    }
    // after mouse click select actor in line camera-cursor. Deselect previous actor if necessary
    private void SelectManager()
    {
        DeselectPreviousActor();
        Selected = GetActorHit();
        SelectActor();
     
    }
    // manages mouse buttons events
    private void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightMouseClick();
        }
    }
    // Call out SelectManager() after mouse click
    private void LeftMouseClick()
    {
        SelectManager();
    }
    private void RightMouseClick()
    {
        MoveToCursor();
    }
    private void MoveToCursor()
    {
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        { 
            GameObject hited = hit.transform.gameObject;

            //moving to free ground location
            if (Selected)
            {
                if (Selected.GetComponent<Unit>())
                {
                    Unit unit = Selected.GetComponent<Unit>();
                    if (unit)
                    {
                        if (hited.name == "Ground")
                        {
                            unit.MoveManager(hit.point, false, null);
                        }
                        if(hited.transform.GetComponent<Resource>())
                        {
                            unit.MoveManager(hit.point, true, hited.transform.GetComponent<Resource>());
                        }
                    }
                }
            }
       
        }
    }
    // Return GameObject if ray hit a target in camera-cursor line
    private GameObject GetActorHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
           return hit.transform.gameObject;
        }
        return null;
    }
    // if actor was selected deselect him
    private void DeselectPreviousActor()
    {
        if (SomethingSelected == true)
        {
            if(Selected.GetComponent<Actor>())
            {
                Actor actor = Selected.GetComponent<Actor>();
                actor.isSelected = false;
            }
            SomethingSelected = false;
            //we
        }
    }
    private void SelectActor()
    {
        if (Selected)
        {
            if (Selected.name != "Ground")
            {
                if (Selected.GetComponent<Actor>())
                {
                    Actor actor = Selected.GetComponent<Actor>();
                    actor.isSelected = true;


                   
                    SomethingSelected = true;                  
                }
            }
        }
    }
    private void HudUpdate(string name)
    {
       int resourceAmount = 0;
       if(Selected.GetComponent<Unit>())
       {
            Unit unit = Selected.GetComponent<Unit>();
            resourceAmount = unit.GetResource();
            player.UpdateHUD(name, resourceAmount);
        }
        if (Selected.GetComponent<Resource>())
        {
            Resource resource = Selected.GetComponent<Resource>();
            resourceAmount = resource.GetResource();
            player.UpdateHUD(name, resourceAmount);
        }
        if (Selected.GetComponent<Building>())
        {
            Building building = Selected.GetComponent<Building>();
            resourceAmount = building.GetResource();
            player.UpdateHUD(name, resourceAmount);
        }
    }
}
