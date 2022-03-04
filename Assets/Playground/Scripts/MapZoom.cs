using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapZoom : MonoBehaviour
{
    public float zoomMax, zoomMin;
    public float zoomSpeed;
    
    public float xBounds, yBounds;
    [SerializeField] [Range(0f, 5f)] float lerpSpeed;
    public float cameraSpeed;

    public GameObject[] floorPlans;
    public GameObject[] floors;
    private int floorCounter = 0;

    void Update()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Move the camera UP
        if (Input.GetKeyDown(GlobalVariables.S.leftSlider)) {
            if (yPos <= yBounds) yPos += cameraSpeed;
        }
        // Move the camera DOWN
        if (Input.GetKeyDown(GlobalVariables.S.rightSlider)) {
            if (yPos >= -yBounds) yPos -= cameraSpeed;
        }

        // Move the camera LEFT
        if (Input.GetKeyDown(GlobalVariables.S.knobLeft)) {
            if (xPos >= -xBounds) xPos -= cameraSpeed;
        }
        // Move the camera RIGHT
        if (Input.GetKeyDown(GlobalVariables.S.knobRight)) {
            if (xPos <= xBounds) xPos += cameraSpeed;
        }

        float zoom = GetComponent<Camera>().orthographicSize;

        // Zoom OUT the camera
        if (Input.GetKeyDown(GlobalVariables.S.downCrank)) {
            if (zoom <= zoomMax) zoom += zoomSpeed;
        }

        // Zoom IN the camera
        if (Input.GetKeyDown(GlobalVariables.S.upCrank)) {
            if (zoom >= zoomMin) zoom -= zoomSpeed;
        }

        //Change floor plan displayed
        //Highlight ISO floor
        if (Input.GetKeyDown(GlobalVariables.S.deviceButton))
        {
            //Hide current displayed map
            floorPlans[floorCounter].SetActive(false);
            
            floorCounter++; //increase count
            //If our new number is greater than the number of floors, loop around
            if (floorCounter > floorPlans.Length-1) floorCounter = 0;
            
            //Show new floor plan 
            floorPlans[floorCounter].SetActive(true);
            
            //NEED TO IMPLEMENT
            //Highlight ISO floor
        }
        
        GetComponent<Camera>().orthographicSize = zoom;

        // Change the position of the camera
        Vector3 targetPosition = new Vector3(xPos, yPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}