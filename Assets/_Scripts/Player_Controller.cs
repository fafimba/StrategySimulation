using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    public static Player_Controller current;

    Camera _camera;
    GameObject _Unit_Selected;

    [SerializeField] LayerMask ground_Layer;
    [SerializeField] LayerMask Unit_Layer;

    [SerializeField] GameObject GO_BattleBlob;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
     
        if (_Unit_Selected != null)
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity,ground_Layer))
            {
                _Unit_Selected.GetComponent<Unit>().UnitPointer.transform.position = hit.point;

                if (Input.mouseScrollDelta.y > 0)
                {
                    _Unit_Selected.GetComponent<Unit>().UnitPointer.transform.rotation  *= Quaternion.Euler(0, 8, 0); 
                }

                if (Input.mouseScrollDelta.y < 0)
                {
                    _Unit_Selected.GetComponent<Unit>().UnitPointer.transform.rotation *= Quaternion.Euler(0, -8, 0);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    OnUnitSelected(null, false);
                    _Unit_Selected = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity,Unit_Layer))
            {
                Transform objectHit = hit.transform;

                var unit_hit = objectHit.gameObject.GetComponent<Unit>();

                if (unit_hit != null)
                {
                    if (_Unit_Selected != null)
                    {
                        OnUnitSelected(null, false);
                    }
                    _Unit_Selected = unit_hit.gameObject;
                    OnUnitSelected(unit_hit.uid,true);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (_Unit_Selected != null)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Unit_Layer))
                {
                    Transform objectHit = hit.transform;

                    var unit_hit = objectHit.gameObject.GetComponent<Unit>();
                    if (unit_hit.uid != _Unit_Selected.GetComponent<Unit>().uid)
                    {
                        var blob = Instantiate(GO_BattleBlob,hit.point - Vector3.up*hit.point.y, _Unit_Selected.transform.rotation);
                        blob.GetComponent<BattleBlob>().BattleBlob_Init(_Unit_Selected.GetComponent<Unit>(),unit_hit);
                    }
                }
                else
                {
                    _Unit_Selected.transform.SetPositionAndRotation(_Unit_Selected.GetComponent<Unit>().UnitPointer.transform.position, _Unit_Selected.GetComponent<Unit>().UnitPointer.transform.rotation);
                }

            }
        }
    }

    public event Action<string,bool> OnUnitSelected;
    public void UnitSelected(string uid = null,bool selected = true)
    {
        OnUnitSelected?.Invoke(uid, selected);
    }
}
