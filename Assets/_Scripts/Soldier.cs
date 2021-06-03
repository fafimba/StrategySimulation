using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{

    [SerializeField] Material Selected;
    [SerializeField] Material Unselecteted;
   // public GameObject Unit { get; set; }
    public  GameObject Destination { get; set; }
    NavMeshAgent navMeshAgent;

    public string Unit_uid { get; set; }
    
    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Player_Controller.current.OnUnitSelected += SwitchSelected;
    }

    void Update()
    {
        navMeshAgent.SetDestination(Destination.transform.position);
    }

    public void SwitchSelected(string id,bool selected = true)
    {
        if (id == Unit_uid || string.IsNullOrEmpty(id))
        {
            GetComponent<MeshRenderer>().material = selected ? Selected : Unselecteted;
        }
    }
}
