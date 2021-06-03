using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBlob : MonoBehaviour
{

    public string uid { get; set; }
    public List<Unit> Units { 
        get {
            if (_units ==null)
            {
                _units = new List<Unit>();
            }
            return _units;

            }
        set
        { _units = value; }
    }
    List<Unit> _units;
    int blobSize;

    [SerializeField] GameObject GO_BlobPosition;

    public void  BattleBlob_Init (Unit unit1, Unit unit2)
    {
        blobSize =  unit1._Unit_Size + unit2._Unit_Size;

        Units.Add(unit1);
        Units.Add(unit2);

        Unit largestUnit;
        Unit smallestUnit;
        if (unit1._Unit_Size > unit2._Unit_Size)
        {
            largestUnit = unit1;
            smallestUnit = unit2;
        }
        else
        {
            largestUnit = unit2;
            smallestUnit = unit1;
        }
      

        GetComponent<SphereCollider>().radius = largestUnit._Unit_Size / 3;

        Vector3[] CardinalPosition = new[] { Vector3.forward,Vector3.right , Vector3.back ,Vector3.left};
        int x = 0, z = 1;
        for (int i = 0; i < smallestUnit._Unit_Size; i++)
        {
            var blob_position = Instantiate(GO_BlobPosition,this.transform);
            blob_position.transform.localPosition = CardinalPosition[x] * (z);
            x++;
            if (x==4)
            {
                x = 0;
                z++;
            }

            blob_position.transform.rotation = Quaternion.Euler(0,Random.Range(0, 360), 0);

            unit1._soldiers[i].Destination = blob_position.transform.GetChild(0).gameObject;
            unit2._soldiers[i].Destination = blob_position.transform.GetChild(1).gameObject;
             Player_Controller.current.UnitSelected(null, false);
            unit1.gameObject.SetActive(false);
            unit2.gameObject.SetActive(false);
        }
    }

    void AddUnit(Unit unit)
    {
        Units.Add(unit);
        unit.gameObject.SetActive(false);
    }
}
