﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CameraSwitch : MonoBehaviour {

	public Dropdown dropdown;
	public List<Camera> listOfCameras = new List<Camera>();
	public float locationSmoothTime = 0.3F;
	public float rotateSmoothFactor = 2.0F;
	public bool followTarget = false;

	public Transform targetTransform;

	public static CameraSwitch main;

	private float extraOffsetX = 1F;
	private float extraOffsetY = 1F;
	private float extraOffsetZ = 1F;

    private Camera selectedCamera;
	private Vector3 cameraVelocity = Vector3.zero;
	private Vector3 cameraOffset = new Vector3 ();
  

    // Use this for initialization
    void Start() {
        main = this;
        selectedCamera = listOfCameras[0];
        targetTransform = selectedCamera.transform;
        listOfCameras.Select(c => c.enabled = false);
		populateCameraDropDown ();
	}

	private void populateCameraDropDown(){
		dropdown.AddOptions(listOfCameras.Select(i => i.name).ToList());
	}


	public void dropDown_IndexChange(int index){//this function is tirggered by the dropdown change
		selectedCamera = listOfCameras[index];// get the index of the selected camera
		targetTransform = selectedCamera.transform;// get the transform of the selected camera
		Camera.main.GetComponent<CameraControl>().isometric = true; // Change the isometric boolean of the main camera to true - QUESTION
		this.SetTarget(targetTransform);//pass the transform of the selected camera to the SetTarget Function
		//Debug.Log(targetTransform.position);
	}

	public void SetTarget(Transform transform) //pass the selected camera transform to the function
	{
		followTarget = true; // when the function is called trigger the followTarget Switch of the Manager
		targetTransform = transform;//set the public targetTransform variable to the selected camera transorm
	}



    void Update()// watch for change of the follow Target to true
    {
        if (followTarget) // if follow target switch is triggered
        {
            updateCameraWithOffset(targetTransform);//Update the Main camera with the Transform of the selected Camera / Target
        }
    }
		
    private void updateCameraWithOffset(Transform transform)
    {
        if(transform != null)// check for the valifity of the transform variabl
        {
            Bounds b = CalculateBounds(transform.gameObject); // Calculate the bounding box of the target GameObject

            float frustrumHeight = b.size.y;

			Vector3 extraOffset = new Vector3 (extraOffsetX, extraOffsetY, extraOffsetZ);// added the variable to set the exta offset of the camera

            float distance = frustrumHeight * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            cameraOffset = transform.forward * -distance + transform.up * distance;

			Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, transform.position + cameraOffset + extraOffset, ref cameraVelocity, locationSmoothTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, transform.rotation, Time.deltaTime * rotateSmoothFactor);
        }
    }

    private Bounds CalculateBounds(GameObject go)
    {
        Bounds b = new Bounds(go.transform.position, Vector3.zero);// Bounding Box - axis aligned - fully enclosed - the method accepts Center + Size
        Object[] rList = go.GetComponentsInChildren(typeof(Renderer));// get all the of the renderer components attacehd to the current target ???
       
		foreach (Renderer r in rList)// clarify?
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }



//    private void FocusCameraOnGameObject(Camera c, GameObject go)
//    {
//        Bounds b = CalculateBounds(go);
//        Vector3 max = b.size;
//        float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
//        float dist = radius / (Mathf.Sin(c.fieldOfView * Mathf.Deg2Rad / 2f));
//        Debug.Log("Radius = " + radius + " dist = " + dist);
//        Vector3 pos = Random.onUnitSphere * dist + b.center;
//        c.transform.position = pos;
//        c.transform.LookAt(b.center);
//    }
		



}