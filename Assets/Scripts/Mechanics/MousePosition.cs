using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] public int _ammo = 3;

    public int ammo
    {
        get { return _ammo; }
        set { _ammo = value; }
    }

    bool _clicking;
    private void Start()
    {
        if (mainCamera == null)
        {
            Debug.Log("No camera has bee set for the mouse!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));

        //Grabbing Mouse Position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        transform.position = mouseWorldPosition;

        //Click Check 
        if (Input.GetMouseButtonDown(0))
        {
            if (_ammo > 0)
            {
                Debug.Log("Ammo used!");
                _ammo--;
            } else
            {
                Debug.Log("Out of Ammo!");
            }
        }

    }

    //Checks if player collides with a duck.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetMouseButton(0))
        {
            _ammo--;

            if (_ammo > 0)
            {
                if (other.gameObject.CompareTag("Duck"))
                {
                    Debug.Log("Hit!");
                    Destroy(other.gameObject);
                }
            } else
            {
                Debug.Log("Out of Ammo!");
            }
        }
    }
}
