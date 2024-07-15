using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] public int ammo = 3;
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
            if (ammo > 0)
            {
                Debug.Log("Miss!");
                ammo--;
            } else
            {
                Debug.Log("Out of Ammo!");
            }
        }
   

    }

    //Checks if player collides with a duck.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ammo--;

            if (ammo > 0)
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
