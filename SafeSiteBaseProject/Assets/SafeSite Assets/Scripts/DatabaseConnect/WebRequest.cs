﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WebRequest : MonoBehaviour {

    public bool processing = false;
    const float WWW_TIMEOUT = 300f;
    const float WWW_WAIT = 0.166f;

    public WWW download;

    public delegate void WebRequestEvent();
    public WebRequestEvent onProcessingFinished;


    private void onProcessingFinishedHandler()
    {
        Debug.Log("Finished Processing " + download.text);
    }

    private IEnumerator GETRequest(string url)
    {
        Debug.Log("Requesting: " + url);
        processing = true;
        download = new WWW(url);
        var timeOutIndex = 0;
        while (!download.isDone && (timeOutIndex < WWW_TIMEOUT) && string.IsNullOrEmpty(download.error))
        {
            timeOutIndex++;
            yield return new WaitForSeconds(WWW_WAIT);
        }
        processing = false;
        if(onProcessingFinished != null) onProcessingFinished();
    }

    private IEnumerator POSTRequest(string url, byte[] postData, Dictionary<string, string> headers )
    {
        processing = true;
        download = new WWW(url, postData, headers);
        var timeOutIndex = 0;
        while (!download.isDone && (timeOutIndex < WWW_TIMEOUT) && string.IsNullOrEmpty(download.error))
        {
            timeOutIndex++;
            yield return new WaitForSeconds(WWW_WAIT);
        }
        processing = false;
        if (onProcessingFinished != null) onProcessingFinished();
    }

    
   
    public void HTTPRequest(string url, WebRequestEvent callbackFunction)
    {
        if (processing)
        {
            Debug.LogError("Already processing request");
            return;
        }
        else
        {
            onProcessingFinished = callbackFunction;
            Debug.Log("Start : " + url);
            StartCoroutine(GETRequest(url));
        }     
    }

    public void HTTPRequest(string url, WebRequestEvent callbackFunction, byte[] postData, Dictionary<string, string> headers)
    {
        if (processing)
        {
            Debug.LogError("Already processing request");
            return;
        }
        else
        {
            onProcessingFinished = callbackFunction;
            StartCoroutine(POSTRequest(url, postData, headers));
        }   
    }

    


}
