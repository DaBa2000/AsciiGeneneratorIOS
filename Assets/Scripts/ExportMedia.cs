using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExportMedia : MonoBehaviour
{
    UI ui;
    TMP_Text asciiCanvas;
    public RawImage image;

    public int a, b, c, d;

    // Start is called before the first frame update
    void Start()
    {
        ui = gameObject.GetComponent<UI>();
        asciiCanvas = ui.asciiCanvas;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeScreenshot()
    {
        StartCoroutine(screenCapture());
    }

    public IEnumerator screenCapture()
    {
        yield return new WaitForEndOfFrame();

        Vector3 pos = asciiCanvas.transform.position;
        Vector3 to = pos + new Vector3(asciiCanvas.renderedWidth, asciiCanvas.renderedHeight, 0);

        // Debug.Log("Position: " + pos.x + " " + pos.y + " " + pos.z);
        // Debug.Log("Start: " + start.x + " " + start.y + " " + start.z);
        // Debug.Log("Width: " + Screen.width + " " + Screen.height + " " + 0);

        ui.disableUI();
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height);
        //screenshot.ReadPixels(new Rect(a, b, Screen.width-c, Screen.height-d), 0, 0);
        Debug.Log(-(Screen.width - asciiCanvas.renderedWidth)/2 + " " +  -Screen.height + pos.y + asciiCanvas.renderedHeight);
        screenshot.ReadPixels(new Rect(-(Screen.width - asciiCanvas.renderedWidth) / 2, -(Screen.height - asciiCanvas.renderedHeight) / 2, Screen.width - c, Screen.height - d), 0, 0);
        screenshot.Apply();
        ui.enableUI();

        image.texture = screenshot;

        byte[] itemBytes = screenshot.EncodeToJPG();
        File.WriteAllBytes(Application.dataPath + "/test.jpg", itemBytes);
        saveTexture2D(itemBytes);
    }

    private void saveTexture2D(byte[] imageBytes)
    {
        NativeGallery.SaveImageToGallery(imageBytes, "AsciiArt", "Image.jpg");
    }
}
