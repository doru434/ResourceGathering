using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;
using UnityEngine;


public class Unit : Actor {

    public float lastGather;
    public int maxResource;

    private int resourceid;
    private GameObject mainBase;
    private Player myPlayer;
    private CollisionDetection collisionDetection;
    private NavMeshAgent navMeshAgent;
    private Vector3 basePosition;
    private Vector3 resourcePosition;
    private Vector3 desiredPosition;

    private float rotateSpeed = 500;
    private float gatheringSpeed;
    private int gatheringAmount;
   // private int ResourceAmount;
    private int gatheringSourceID;

    private bool move;
    private bool rotate;
    private bool wantToGather;
    private bool goingBackToBase;
    private bool isColliding;
    private bool turnOnCollider;
    private bool gathering;
    private bool isWaiting;


    protected override void Start () {
        base.Start();
        InitializeParameters();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();


        // Checing do Unit collide with another unit
        isColliding = collisionDetection.colliding;
        UpdatePosition();

        // Stoping gathering when full
        if(ResourceAmount == maxResource)
        {
            wantToGather = false;
        }

        TurnOnCollision();

        CatchGatheringExceptions();
       

    }
    public void DrawDebugLine(Vector3 resourcePosition)
    {

        Debug.DrawLine(this.transform.position, resourcePosition, Color.green); 

    }
    private void InitializeParameters()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();     
        mainBase = GameObject.FindGameObjectWithTag("Base");
        basePosition = mainBase.transform.position;

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        myPlayer = Player.transform.GetComponent<Player>();


        lastGather = 0.0f;
        gatheringSpeed = 2.0f;
        gatheringAmount = 2;
        ResourceAmount = 0;
        maxResource = 6;
        gathering = false;
        wantToGather = false;
        goingBackToBase = false;
        isSelected = false;
        move = false;
        rotate = false;
        isWaiting = false;
        turnOnCollider = false;

        collisionDetection = transform.GetComponentInChildren<CollisionDetection>();  
    }
    /// <summary>
    /// Sending unit to desired position and stoping, when target reached.
    /// </summary>
    private void UpdatePosition()
    {
        if (gathering == true)
        {
            navMeshAgent.radius = 0.1f;
        }
        if(wantToGather == false && gathering == false)
        {
            navMeshAgent.radius = 0.8f;
        }
        if(move == true)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(desiredPosition);            
        }
        if(rotate == true)
        {
            if (navMeshAgent.enabled == false)
            {
                RotateObject();
            }
        }

        if(desiredPosition == this.transform.position)
        {
            move = false;
            rotate = false;
        }

    }
    /// <summary>
    /// Changing layer on whole object tree.
    /// </summary>
    static void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// Turning on, only when we don't collide with other unit.
    /// </summary>
    private void TurnOnCollision()
    {
        if (turnOnCollider == true)
        {
            if (isColliding == false)
            {
                turnOnCollider = false;
                ChangeLayerToUnit();
            }
        }
    }
    private void ChangeLayerToUnit()
    {
        //SetLayerOnAll(this.gameObject, LayerMask.NameToLayer("Unit"));
        transform.gameObject.layer = LayerMask.NameToLayer("Unit");
    }
    private void ChangeLayerToGathering()
    {
        //SetLayerOnAll(this.gameObject, LayerMask.NameToLayer("Gathering"));
        transform.gameObject.layer = LayerMask.NameToLayer("Gathering");
    }

    public bool GetStateOfNavMeshAgent()
    {
        return navMeshAgent.enabled;
    }
    public bool GetIsWaiting()
    {
        return isWaiting;
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
        return ResourceAmount;
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
    public void SetIsWaiting(bool set)
    {
        isWaiting = set;
    }
    public void SetGathering(bool set)
    {
        gathering = set;
    }
    /// <summary>
    /// Adding resources to unit.
    /// </summary>
    public void SetResourceCount(int lastPart, int SourceResource)
    {
        if (lastPart == 0)
        {
            ResourceAmount += gatheringAmount;
            lastGather = 0;
        }
        if(lastPart == 1)
        {
            ResourceAmount += SourceResource;
            lastGather = 0;
        }
        if (lastPart == 2)
        {
            ResourceAmount += ResourceSpace();
            lastGather = 0;
        }
    }
    /// <summary>
    /// Checks if unit is able to gather full amount of current gatheringAmount.
    /// </summary>
    public bool EnoughtSpace()
    {
        if(ResourceAmount+gatheringAmount <=  maxResource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Returns number of free space for resources.
    /// </summary>
    public int ResourceSpace()
    {
        return maxResource - ResourceAmount;
    }
    /// <summary>
    /// Sending unit bact to gathering.
    /// </summary>
    public void TransferResources()
    {
        ResourceAmount = 0;
        goingBackToBase = false;
        wantToGather = true;
        MoveObject(resourcePosition);        
    }
    /// <summary>
    /// Checks if it still free space for resources.
    /// </summary>
    public bool SpaceForResource()
    {
        if (ResourceAmount + gatheringAmount <= maxResource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Checks if unit is already full.
    /// </summary>
    public bool IsFull()
    {
        if (ResourceAmount == maxResource)
            return true;
        else
            return false;
    }
    /// <summary>
    /// Menages unit movement depending on destination objective.
    /// </summary>
    public void MoveManager(Transform destination, ToWho where, int resourceID)
    {

        if (where==ToWho.Resource )
        {
            gatheringSourceID = resourceID;
            wantToGather = true;
 
            MoveObject(destination.position);
            ChangeLayerToGathering();   
        }
        if (where==ToWho.FreeGround)
        {
            MoveObject(destination.position);
            wantToGather = false;
            gathering = false;
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
            MoveObject(destination.position);
            goingBackToBase = true;
            ChangeLayerToGathering();
        }

    }
    /// <summary>
    /// Stops movement.
    /// </summary>
    public void StopMoving()
    {
        desiredPosition=transform.position;
    }
    /// <summary>
    /// Sending unit to destination location.
    /// </summary>
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
    /// <summary>
    /// Rotate object in one axis to pointed location.
    /// </summary>
    protected override void RotateObject()
    {
        base.RotateObject();

        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, this.transform.position-desiredPosition, Time.deltaTime * rotateSpeed, 0.0f);

        newDir.y = 0;

        transform.rotation = Quaternion.LookRotation(newDir);
    }
    /// <summary>
    /// Stoping gathering and sending unit to base.
    /// </summary>
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
    /// <summary>
    /// Finding next, closest gathering source.
    /// </summary>
    public void FindNextSource(int resourceID)
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
                    if (resourceID == 0)
                    {
                        closest = temp;
                        destinationResource = i;
                    }
                    if(i.gameObject.GetInstanceID()!=resourceID)
                    {
                        closest = temp;
                        destinationResource = i;
                    }
                }                             
            }
        }
        if (destinationResource!=null)
        {
            MoveManager(destinationResource.transform, ToWho.Resource, destinationResource.gameObject.GetInstanceID());
        }
        if (destinationResource == null)
        {
            Wait(resourceID);
        }
    }
    /// <summary>
    /// Waiting for slot at resource source.
    /// </summary>
    private void Wait(int resourceID)
    {
        MoveManager(this.transform, ToWho.Resource, resourceID);
        isWaiting = true;       
    }
    /// <summary>
    /// Changing state of isKinematic depending on whether or not NavMeshAgent is activated.
    /// </summary>
    public void ChangeStateOfRigidBody(bool boolean)
    {
        transform.GetComponent<NavMeshAgent>().enabled = boolean;
        Rigidbody thisRigidbody = transform.GetComponent<Rigidbody>();
        if (boolean == true)
        {
           thisRigidbody.isKinematic = true;
        }
        if(boolean == false)
        {
           thisRigidbody.isKinematic = false;
        }
    }
    /// <summary>
    /// Checks if resource source empty.
    /// </summary>
    public bool CheckIfEmpty()
    {        
        foreach (Resource i in myPlayer.resourcesList)
        {
            if (i != null)
            {
                if (i.gameObject.GetInstanceID() == gatheringSourceID)
                {
                    if (i.Resource == 0)
                    {
                        return true;
                    }
                    if (i.Resource != 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    /// <summary>
    /// Catching movement exceptions when we are gathering
    /// </summary>
    private void CatchGatheringExceptions()
    {
        if (move == false && wantToGather == true && gathering == false && isWaiting == false)
        {
            FindNextSource(0);
        }
        if (move = true && wantToGather == true && gathering == false && isWaiting == false)
        {
            if (CheckIfEmpty())
            {
                FindNextSource(0);
            }
        }
    }
}
