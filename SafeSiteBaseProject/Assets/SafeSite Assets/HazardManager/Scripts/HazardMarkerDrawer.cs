﻿using UnityEngine;
using System.Collections;

public class HazardMarkerDrawer : MonoBehaviour {


    
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseClick = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Vector3 worldPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                worldPoint -= ray.direction*0.1f;
                GameObject newHazardMarker = (GameObject)Instantiate(HazardManager.main.HazardMarkerPrefab, HazardManager.main.Hazards.transform);
                newHazardMarker.name = "Hazard";
                
				newHazardMarker.transform.position = worldPoint;
                newHazardMarker.transform.rotation.SetEulerAngles(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
                
				HazardManager.main.listOfHazards.Add(newHazardMarker);
                
				//Move Camera to hazard
				CameraSwitch.main.SetTarget(newHazardMarker.transform);

                TreeViewManager.main.TreeView.AddChild(HazardManager.main.Hazards, newHazardMarker);
                this.enabled = false;
            }
        }
    }
}
