using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IDCAM : MonoBehaviour
{
    public static IDCAM Instance { get; private set; }
    public List<Sprite> fakeImages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator CaptureScreenshot(Transform npcTransform, System.Action<Sprite> onCaptured)
    {

        //syield return new WaitForEndOfFrame();
        Debug.Log("CaptureScreenshot coroutine started.");

        int width = 1024;  
        int height = 1024; 
        RenderTexture tempRT = new RenderTexture(width, height, 24);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = tempRT;


        Camera cam = GetComponent<Camera>();
        cam.targetTexture = tempRT;
        cam.Render();


        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();


        cam.targetTexture = null;
        RenderTexture.active = currentRT;
        Destroy(tempRT);



        Sprite screenshotSprite = Sprite.Create(screenshot, new Rect(0.0f, 0.0f, screenshot.width, screenshot.height), new Vector2(0.5f, 0.5f), 100.0f);
        onCaptured.Invoke(screenshotSprite);

        if (screenshotSprite != null)
        {
            Debug.Log("Sprite created and being passed to callback.");
        }
        else
        {
            Debug.LogError("Sprite creation failed.");
        }

        yield return null;
    }
}
