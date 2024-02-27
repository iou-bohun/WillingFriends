using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitResult;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                Vector3 direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                TempMonster tempmonster = ObjectPoolManager.GetObject("TempMonster").GetComponent<TempMonster>();
                tempmonster.transform.position = transform.position + direction.normalized;
                tempmonster.MoveForward(direction.normalized);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hitResult;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                Vector3 direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                Car2 tempmonster = ObjectPoolManager.GetObject("Car2").GetComponent<Car2>();
                tempmonster.transform.position = transform.position + direction.normalized;
            }
        }
    }
}
