﻿using UnityEngine;

public class ObjectDestroy : MonoBehaviour {

	private bool drawerActive = false;
	public void OnMouseDown()
	{
		drawerActive = true;
	}
    
	void Update () {
			if (drawerActive) {
			    if (Input.GetKeyDown("d")) {
					    AssetManager.main.DeleteAsset(gameObject);
				        drawerActive = false;
				    }
			}
	}
}