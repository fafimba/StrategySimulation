using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{

    public int _Unit_Size = 15;
    [SerializeField] int _Max_Column = 5;
    [SerializeField] float _Column_Width = 1;
    [SerializeField] float _Row_Width = 1;

    [SerializeField] GameObject GO_Soldier;
    [SerializeField] GameObject GO_Soldier_Position;

    [SerializeField] GameObject GO_Unit_Pointer;
    [SerializeField] GameObject GO_Soldier_Position_Pointer;

    [SerializeField] GameObject GO_Models_Position;

   public string uid { get; set; }

    List<GameObject> _positions = new List<GameObject>();
    public List<Soldier> _soldiers = new List<Soldier>();

    [HideInInspector]
    public GameObject UnitPointer {
        get {
            if (unitPointer == null)
            {
                unitPointer = Instantiate(GO_Unit_Pointer);
                foreach (var soldierPosition in _positions)
                {
                    var go = Instantiate(GO_Soldier_Position_Pointer, unitPointer.transform.GetChild(0));
                    go.transform.localPosition = soldierPosition.transform.localPosition;
                    unitPointer.transform.GetChild(0).transform.localPosition = new Vector3((((float)_Max_Column / 2) * _Column_Width * -1) + _Column_Width / 2, 0, (((float)(_Unit_Size / _Max_Column) / 2) * _Row_Width * -1) + _Row_Width / 2);
                }
            }
            return unitPointer;
        }
        set { unitPointer = value; }
    }
    GameObject unitPointer;



    void Start()
    {
        uid = Guid.NewGuid().ToString();
        Create_Unit();
        Check_Formation();
        UnitPointer.SetActive(false);
        Player_Controller.current.OnUnitSelected += UnitSelected;
    }

    private void Update()
    {
        Check_Formation();
    }

    void Create_Unit()
    {
        for (int i = 0; i < _Unit_Size; i++)
        {
            var soldier_position = Instantiate(GO_Soldier_Position,GO_Models_Position.transform);
            _positions.Add(soldier_position);

            var soldier = Instantiate(GO_Soldier,transform.position,transform.rotation).GetComponent<Soldier>();
            _soldiers.Add(soldier);
            _soldiers[i].Destination = soldier_position;
            _soldiers[i].Unit_uid = uid;
        }
        GetComponent<BoxCollider>().size = new Vector3(_Column_Width * _Max_Column, 2, (_Row_Width * (_Unit_Size / _Max_Column)));
    }

    void Check_Formation()
    {
        for (int y = 0; y < (_Unit_Size / _Max_Column) + _Unit_Size%_Max_Column ; y++)
        {
            for (int x = 0; x < _Max_Column; x++)
            {
                _positions[y*_Max_Column + x].transform.localPosition = new Vector3(x,0,y);
            }
        }

        GO_Models_Position.transform.localPosition = new Vector3( (((float)_Max_Column / 2) * _Column_Width * -1) + _Column_Width/2 , 0, (((float)(_Unit_Size / _Max_Column) / 2) * _Row_Width * -1) + _Row_Width/2);
    }

    public void UnitSelected(string unit_uid, bool selected = true)
    {
        if (unit_uid == uid || string.IsNullOrEmpty(unit_uid))
        {
            UnitPointer.SetActive(selected);
            GO_Models_Position.SetActive(selected);
        }
        UnitPointer.transform.position = Input.mousePosition;
    }
}
