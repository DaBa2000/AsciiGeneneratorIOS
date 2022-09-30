using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class loadImage : MonoBehaviour
{
    // GameObjects
    private GameObject camera;
    public TMP_Text asciiCanvas;

    // AsciiGenerator
    private AsciiGenerator asciiGenerator;
    private UI ui;

    private string[] asciiText;
    private int numReadyFrames;

    private Boolean videoPlaying = false;

    VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        videoPlayer = camera.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;
        videoPlayer.isLooping = true;

        ui = gameObject.GetComponent<UI>();
        ui.textWidth.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        ui.textColored.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        asciiGenerator = gameObject.GetComponent<AsciiGenerator>();
        string ascii = asciiGenerator.processImage(null, ui.getTextWidth(), ui.getColored());
        ui.renderAsciiText(ascii);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pickImage()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                endVideo();
                videoPlaying = false;

                // path, max size, notReadable
                string text = asciiGenerator.processImage(NativeGallery.LoadImageAtPath(path, int.MaxValue, false), ui.getTextWidth(), ui.getColored());
                ui.renderAsciiText(text);
            }
        });
    }

    public void pickVideo()
    {
        NativeGallery.GetVideoFromGallery((path) =>
        {
            if(path != null)
            {
                endVideo();
                StartCoroutine(loadVideo(path));
            }
        });
    }

    IEnumerator loopVideo()
    {
        int frameCount = asciiText.Length;
        int i = 0;

        while(true)
        {
            yield return new WaitForSecondsRealtime(1.0f/24);
            if (asciiText != null) asciiCanvas.text = asciiText[i++ % frameCount];
        }
    }

    /// <summary>
    /// Preapre videoplayer to convert image
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator loadVideo(string path)
    {
        if (path != null)
        {
            videoPlaying = true;
            ui.startLoading();

            videoPlayer.url = "file://" + path;
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
            {
                yield return new WaitForSeconds(1);
            }

            int frameCount = (int)videoPlayer.frameCount + 1;
            asciiText = new string[frameCount];

            videoPlayer.frameReady += OnFrameReady;
            videoPlayer.Play();

            while (numReadyFrames != frameCount)
            {
                yield return new WaitForSeconds(1);
            }

            ui.endLoading();
            videoPlayer.Stop();
            StartCoroutine(loopVideo());
        }
    }

    /// <summary>
    /// For every videoframe create texture, convert it to ascii and save it to ascii array
    /// </summary>
    /// <param name="source">Videoplayer which plays video</param>
    /// <param name="frameIndex">number of frame to be converted</param>
    private void OnFrameReady(VideoPlayer source, long frameIndex)
    {
        if (frameIndex < asciiText.Length && asciiText[frameIndex] == null)
        {
            RenderTexture renderTexture = source.texture as RenderTexture;
            RenderTexture.active = renderTexture;

            Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            image.Apply();

            asciiText[frameIndex] = asciiGenerator.processImage(image, ui.getTextWidth(), ui.getColored());

            UnityEngine.Object.Destroy(image);

            numReadyFrames++;
            ui.setLoadingProgress((float)numReadyFrames / asciiText.Length, numReadyFrames + " / " + asciiText.Length + " Frames loaded");
        }
    }

    public void ValueChangeCheck()
    {
        if (!videoPlaying)
        {
            string text = asciiGenerator.processImage(null, ui.getTextWidth(), ui.getColored());
            ui.renderAsciiText(text);
        }
    }

    private void endVideo()
    {
        videoPlayer.Stop();
        ui.endLoading();
        numReadyFrames = 0;
        StopAllCoroutines();
    }
}
