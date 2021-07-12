using UnityEngine;
using FileHandling;
using System;
/// <summary>
/// Class which implements the functionality of taking
/// screenshots in the simulation. With this the user can for example save an image of
/// a graph.
/// One should be able take a picture each second, if click is faster it will be overwritten.
/// 
/// The attached date has the format: yyyyMMddTHHmmss
/// 
/// Screenshots can be taken with the space key.
/// </summary>
public class ScreenshotHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            string pathToFolder = FileHandler.GetScreenshotFilePath();     
            DateTime date = DateTime.Now;
            //appending date in seconds in ISO 8601 format 
            string filename = pathToFolder + "/"+"Screenshot" + "_" + FileHandler.SelectedFileName +"_"+date.ToString("yyyyMMddTHHmmss");
            ScreenCapture.CaptureScreenshot(filename + ".png");
            Debug.Log("Picture saved !");
        }
    }
}
