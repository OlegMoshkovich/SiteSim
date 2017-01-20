﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZonePanel : MonoBehaviour {
    public Toggle alertToggle;

    private ExclusionZone ez;

	public void OnAlertToggleChangeHandler(bool val)
    {
        if(ez != null ) ez.alert = val;
    }

    private void OnEnable()
    {
        ez = AssetPanel.main.selectedAsset.GetComponent<ExclusionZone>();
        if(alertToggle != null)
        {
            alertToggle.isOn = ez.alert;
        }
        else
        {
            Debug.LogError("Missing alertToggleValue");
        }
    }
}
