using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : Actor {
    //private int ResourceAmount;
    private int MuleCost;
    public Unit newMule;
    private Vector3 spawnPoint;
    private Vector3 rallyingPoint;
    private Player player;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        player = FindObjectOfType<Player>();
        MuleCost = 20;
        spawnPoint = new Vector3(transform.position.x-4, transform.position.y + 2.5f, transform.position.z - 6);
        rallyingPoint = spawnPoint;
    }


    protected override void Update () {
        base.Update();  
	}
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Unit>())
        {
            Unit unit = other.transform.GetComponent<Unit>();
            if(unit.GetGoingBackToBase())
            {
                FindObjectOfType<Player>().AddResource(unit.GetResource());
                unit.TransferResources();
            }
        }
    }
    public bool CreateMule()
    {
        if(player.AbleToPay(MuleCost))
        {
            while (Physics.CheckBox(spawnPoint, new Vector3(1.0f, 1.0f, 0.5f)))
            {
                spawnPoint.x += 2.0f;
            }
            //Transform temp = Instantiate(newMule, spawnPoint, Quaternion.identity);
            Unit clone = (Unit)Instantiate(newMule, spawnPoint, transform.rotation);
            player.DecreseResourceCount(MuleCost);
            //clone.GetComponent<NavMeshAgent>().SetDestination(rallyingPoint);
            clone.MoveManager(rallyingPoint, ToWho.FreeGround, 0);
            spawnPoint = new Vector3(transform.position.x - 4, transform.position.y + 2.5f, transform.position.z - 6);          
            return true;
        }
        return false;
    }
    public void SetRallyingPoint(Vector3 hitPoint)
    {
        rallyingPoint = hitPoint;
    }
}
