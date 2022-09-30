using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsciiGenerator : MonoBehaviour
{
    public Texture2D startImage;

    // AsciiTexture
    private Texture2D image;

    // Panel for AsciiImage
    public TMP_Text asciiCanvas;

    public void Awake()
    {
        image = startImage;
    }

    public void Update()
    {
        
    }

    private void setImage(Texture2D newImage)
    {
        if (image != null) UnityEngine.Object.Destroy(image);
        image = newImage;
    }

    public string processImage(Texture2D texture, int width, bool colored)
    {
        if (texture != null) setImage(texture);
        return createAsciifromImage(width, getHeightLines(width), colored);
    }

    private string createAsciifromImage(int width, int height, bool color = false)
    {
        string ascii = "<line-height=80%><mspace=0.9e>\n";

        // pixel steps in which image is sampled
        float x_step = (float)image.width / width;
        float y_step = (float)image.height / height;

        for (float i = 0; i < image.height; i += y_step)
        {
            for (float j = 0; j < image.width; j += x_step)
            {
                Color col = image.GetPixel((int)j, image.height - (int)i);
                if (color) ascii += "<color=#" + ColorUtility.ToHtmlStringRGBA(col) + ">";
                ascii += rgbToChar(col.r, col.g, col.b);
            }
            ascii += "\n";
        }

        return ascii;
    }

    private char rgbToChar(float r, float g, float b)
    {
        string ramp = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'.";

        float sum = (0.3f * r + 0.59f * g + 0.11f * b);
        sum = Mathf.Round(sum * (ramp.Length + 1)) - 1;
        if (sum < 0) sum = 0;
        if (sum > (ramp.Length - 1)) sum = ramp.Length - 1;
        return ramp[(int)sum];
    }

    private int getHeightLines(int width)
    {
        float ratio = (float)image.height / (float)image.width;

        string tmp = asciiCanvas.text;
        asciiCanvas.SetText("<line-height=80%><mspace=0.9e>\n" + new String('.', width));

        float char_width = asciiCanvas.renderedWidth / width;
        float char_height = 1.6f * char_width;

        asciiCanvas.text = tmp;

        return (int)(ratio * asciiCanvas.renderedWidth / char_height);
    }
}
