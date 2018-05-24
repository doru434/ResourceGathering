using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Unit : Actor {
    // Use this for initialization
    private GameObject mainBase;
    private Player myPlayer;

    private float moveSpeed = 3;
    private float rotateSpeed = 5;

    private float gatheringSpeed;
    private int gatheringAmount;
    private int resource;

    public float lastGather;
    public int maxResource;
    private int gatheringSourceID;

    private Vector3 desiredPosition;
    private bool move;
    private bool rotate;
    private bool wantToGather;
    private bool goingBackToBase;
    private bool isColliding;
    private bool turnOnCollider;
    private bool gathering;

    private CollisionDetection collisionDetection;
    private Vector3 basePosition;
    private Vector3 resourcePosition;
    
    protected override void Start () {
        base.Start();
        

        mainBase = GameObject.FindGameObjectWithTag("Base");
        basePosition = mainBase.transform.position;

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        myPlayer = Player.transform.GetComponent<Player>();

        lastGather = 0.0f;
        gatheringSpeed = 2.0f;
        gatheringAmount = 2;
        resource = 0;
        maxResource = 6;
        gathering = false;
        wantToGather = false;
        goingBackToBase = false;
        isSelected = false;
        move = false;
        rotate = false;
        
        if (transform.GetComponentInChildren<CollisionDetection>())
        {
            collisionDetection = transform.GetComponentInChildren<CollisionDetection>();
            
        }
        else
            Debug.Log("Cant find child with collisionDetection");
        turnOnCollider = false;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();

        isColliding = collisionDetection.colliding;
        UpdatePosition();
        if(resource == maxResource)
        {
            wantToGather = false;
        }

        if(turnOnCollider == true)
        {
            if(isColliding == false)
            {
                turnOnCollider = false;
                TurnOnCollision();
            }
        }
        if(move==false && wantToGather == true && gathering ==false)
        {
            FindNextSource();
        }

    }
    private bool IsMoving()
    {

        Vector3 before = transform.position;
        new WaitForSecondsRealtime(1);
        Vector3 after = transform.position;
        if(after != before)
        {
            return true;
        }
        return false;

    }
    private void UpdatePosition()
    {
        if(move == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        }
        if(rotate == true)
        {
            RotateObject();
        }

        if(desiredPosition == this.transform.position)
        {
            move = false;
            rotate = false;
        }

    }
    static void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }
    private void TurnOnCollision()
    {
        //SetLayerOnAll(this.gameObject, LayerMask.NameToLayer("Unit"));
        transform.gameObject.layer = LayerMask.NameToLayer("Unit");
    }
    private void TurnOffCollision()
    {
        //SetLayerOnAll(this.gameObject, LayerMask.NameToLayer("Gathering"));
        transform.gameObject.layer = LayerMask.NameToLayer("Gathering");
    }
    public int GetGatheringSourceID()
    {
        return gatheringSourceID;
    }
    public float GetGatheringSpeed()
    {
        return gatheringSpeed;
    }
    public int GetGatheringAmount()
    {
        return gatheringAmount;
    }
    public int GetResource()
    {
        return resource;
    }
    public bool GetWantToGather()
    {
        return wantToGather;
    }
    public bool GetGoingBackToBase()
    {
        return goingBackToBase;
    }
    public bool GetGathering()
    {
        return gathering;
    }
    public void SetGathering(bool set)
    {
        gathering = set;
    }
    public void SetResourceCount(int lastPart, int SourceResource)
    {
        if (lastPart == 0)
        {
            resource += gatheringAmount;
            lastGather = 0;
        }
        if(lastPart == 1)
        {
            resource += SourceResource;
            lastGather = 0;
        }
        if (lastPart == 2)
        {
            resource += ResourceSpace();
            lastGather = 0;
        }
    }
    public bool EnoughtSpace()
    {
        if(resource+gatheringAmount <=  maxResource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int ResourceSpace()
    {
        return maxResource - resource;
    }
    public void TransferResources()
    {
        resource = 0;
        goingBackToBase = false;
        wantToGather = true;
        MoveObject(resourcePosition);        
    }
    public bool SpaceForResource()
    {
        if (resource + gatheringAmount <= maxResource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsFull()
    {
        if (resource == maxResource)
            return true;
        else
            return false;
    }
    public void MoveManager(Vector3 destination, ToWho where, int resourceID)
    {

        if (where==ToWho.Resource )
        {
            gatheringSourceID = resourceID;
            wantToGather = true;
            MoveObject(destination);
            TurnOffCollision();   
        }
        if (where==ToWho.FreeGround)
        {
            MoveObject(destination);
            wantToGather = false;
            goingBackToBase = false;

            //isColliding avoiding collision after we change layer
            if(isColliding == false)
            {
                TurnOnCollision();
            }
            if(isColliding == true)
            {
                turnOnCollider = true;
            }          
        }
        if(where==ToWho.Building)
        {
            MoveObject(destination);
            goingBackToBase = true;
            TurnOffCollision();
        }

    }
    public void StopMoving()
    {
        desiredPosition=transform.position;
    }
    protected override void MoveObject(Vector3 destination)
    {
        base.MoveObject(destination);
        desiredPosition.x = destination.x;
        desiredPosition.y = this.transform.position.y;
        desiredPosition.z = destination.z;

        if(this.transform.position != desiredPosition)
        {
            move = true;
            rotate = true;
        }
        
    }
    protected override void RotateObject()
    {
        base.RotateObject();

        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, this.transform.position-desiredPosition, Time.deltaTime * rotateSpeed, 0.0f);

        newDir.y = 0;
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    public void ReturnResources()
    {
        wantToGather = false;
        goingBackToBase = true;
        MoveObject(basePosition);
    }
    public void RememberResourcePosition(Vector3 resourceLocation)
    {
        resourcePosition = resourceLocation;
    }
    public void FindNextSource()
    {
        float closest=99999;

        Resource destinationResource = null;
        foreach (Resource i in myPlayer.resourcesList)
        {
            if (i.GetResource() != 0)
            {      
                float temp = Vector3.Distance(this.transform.position, i.transform.position);
                if(temp < closest)
                {
                    closest = temp;
                    destinationResource = i;
                }                             
            }
        }
        if(destinationResource!=null)
        {
            MoveManager(destinationResource.transform.position, ToWho.Resource, destinationResource.gameObject.GetInstanceID());
        }
    }
}
