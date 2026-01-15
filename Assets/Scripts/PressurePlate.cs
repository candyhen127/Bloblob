using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    float massRequired = 100; //Mass required to activate button
    [SerializeField]
    float initialCastDistance = 0.2f; //Initial Check distance for first object on the button
    float dynamicCastDistance; //This number will be used to change the check distance of the box cast
    [SerializeField]
    LayerMask objectLayerMask; //Objects to weigh.They must have a rigidbody & collider
    bool totalMassCalculated;  //Used to exit the while loop after adding all masses
    RaycastHit2D[] raycastHits; //Array that will contain all objects placed on the button
    [SerializeField] int currentNumberOfObjectsOnButton; //Dynamic Counter
    [SerializeField] float totalMass; //Sum of masses of all placed objects
    Vector2 halfExtents; //The size of the button collider used for boxcasting
    float distanceFromTheTop; //highest point in the stack
    bool isActivated;

    [SerializeField] TextMeshProUGUI weightCounter;

    
    [SerializeField] UnityEvent onPlateActivated; // Events to trigger when activated
    [SerializeField] UnityEvent onPlateDeactivated; // Events to trigger when deactivated

    private void Awake()
    {
        Vector2 extents = GetComponent<BoxCollider2D>().bounds.extents;
        halfExtents = new Vector2(extents.x+1, extents.y);
    }

    void Update() {
        weightCounter.text = ((int) totalMass).ToString() + "/" + ((int)massRequired).ToString();
    }
    
    void FixedUpdate()
    {
        if (totalMass >= massRequired && !isActivated)
        {
            isActivated = true;
            Debug.Log("Pressure Plate ON! Total Mass: " + totalMass);
            onPlateActivated.Invoke(); // Trigger Unity events for activation
        }
        else if (totalMass < massRequired && isActivated)
        {
            isActivated = false;
            Debug.Log("Pressure Plate OFF! Total Mass: " + totalMass);
            onPlateDeactivated.Invoke(); // Trigger Unity events for deactivation
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Blob")
        {
            CheckTotalMass();

            if (totalMass >= massRequired)
            {
                Debug.Log("Button is ON");
            }
            else
            {
                Debug.Log("Button is OFF");
            }
        }
    }

    void OnCollisionExit2D() {
        CheckTotalMass();
    }

    void CheckTotalMass()
    {
        // Reset all the values
        currentNumberOfObjectsOnButton = 0;
        totalMass = 0;
        totalMassCalculated = false;
        distanceFromTheTop = 0;
        dynamicCastDistance = initialCastDistance;

        //While loop used for determining the number and mass of placed objects
        while (!totalMassCalculated)
        {
            //Check for objects using the dynamic distance
            raycastHits = Physics2D.BoxCastAll(transform.position, halfExtents, 0, Vector2.up,
                dynamicCastDistance, objectLayerMask);

            //Check if we reached the top object
            if (raycastHits.Length > currentNumberOfObjectsOnButton)
            {
                //Calculate the distance from the top
                for (int j = 0; j < raycastHits.Length; j++)
                {
                    if (distanceFromTheTop < (raycastHits[j].distance
                        + raycastHits[j].transform.GetComponent<Collider2D>().bounds.size.y))
                    {
                        distanceFromTheTop = raycastHits[j].distance
                        + raycastHits[j].transform.GetComponent<Collider2D>().bounds.size.y;
                    }
                }

                //Increase the check distance
                dynamicCastDistance = distanceFromTheTop + initialCastDistance;

                //Increase the number of placed object to check for in the next loop cycle
                currentNumberOfObjectsOnButton++;
            }
            else
            {
                //This is used to exit the while loop when we reach the top of the stack
                totalMassCalculated = true;

                //For loop to add the masses of the placed object
                for (int i = 0; i < raycastHits.Length; i++)
                {
                    totalMass += raycastHits[i].transform.GetComponent<Rigidbody2D>().mass;
                }
            }
        }

        Debug.Log("Total mass is " + totalMass);

        Debug.Log("Total number of objects is " + raycastHits.Length);

        Debug.Log("Approximate Distance from top is " + distanceFromTheTop);
    }

}
