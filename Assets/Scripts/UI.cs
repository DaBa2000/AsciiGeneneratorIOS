using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    // Panel for AsciiImage
    public TMP_Text asciiCanvas;

    // control elements
    public Slider textWidth;
    public Toggle textColored;

    // loading animation
    public GameObject panel_loading;
    public Slider slider_progress;
    public TMP_Text text_progress;

    // UI Panel
    public GameObject panel_UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getTextWidth()
    {
        return (int)textWidth.value;
    }

    public bool getColored()
    {
        return textColored.isOn;
    }

    public void renderAsciiText(string asciiText)
    {
        asciiCanvas.text = asciiText;
    }

    /*** Loading Screen ***/

    public void startLoading()
    {
        panel_loading.SetActive(true);
    }

    public void endLoading()
    {
        panel_loading.SetActive(false);
    }

    public void setLoadingProgress(float progress, string text)
    {
        progress = Mathf.Clamp01(progress);
        slider_progress.value = progress;
        text_progress.text = text;
    }

    public float getCanvasWidth()
    {
        return asciiCanvas.renderedWidth;
    }

    public float getCanvasHeight()
    {
        return asciiCanvas.renderedHeight;
    }

    public void disableUI()
    {
        panel_UI.SetActive(false);
    }

    public void enableUI()
    {
        panel_UI.SetActive(true);
    }
}
