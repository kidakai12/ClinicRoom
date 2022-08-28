using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using TMPro;
public class GetAPIData : MonoBehaviour
{
    private string urlHead;
    private const float API_CHECK_MAXTIME = 1 * 60.0f; //1 minute
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    public TextMeshPro TEXT;
    public GameObject quad;
    private Renderer rend;
    public Material material;
    private RawImage Image;
    private TextMeshPro textShow;
    public VideoPlayer videoPlayer;
    private string videoUrl;

    void Start()
    {
        StartCoroutine(GetListData(getURL));
        rend = quad.GetComponent<Renderer>();
        rend.enabled = true;
        urlHead = "http://localhost:1337";
        //urlHead = "http://192.168.250.198:1337";
    }
    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            apiCheckCountdown = API_CHECK_MAXTIME;
            StartCoroutine(GetListData(getURL));
        }
    }
    public void getURL(TotalAPI onSuccess)
    {

        /*TEXT.text = onSuccess.data[4].attributes.text;
        StartCoroutine(LoadImage3D(urlHead + onSuccess.data[3].attributes.picture.data.attributes.url));
        /*
        int i = 0;
        foreach (APIobject obj in onSuccess.data)
        {
            Debug.Log("Text: " + i + " " + obj.attributes.text);
            Debug.Log("URL: " + i + " " + obj.attributes.url1);
            i++;
        }
        */
        /*videoPlayer.url = urlHead + onSuccess.data[5].attributes.picture.data.attributes.url;
        //videoPlayer.url = "https://media.istockphoto.com/videos/austin-texas-downtown-aerial-footage-video-id1350010606";
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Prepare();
        videoPlayer.Play();*/
    }

    IEnumerator LoadImage(string url)
    {
        WWW www = new WWW(url);
        //loading.SetActive(true);
        yield return www;

        if (www.error == null)
        {
            //loading.SetActive(false);
            Texture2D texture = www.texture;
            texture.anisoLevel = 16;
            RectTransform rectRaw = Image.GetComponent<RectTransform>();
            double fraction = (float)texture.width / (float)texture.height;
            Debug.Log("Height: " + fraction);
            float height = 1920 / (float)fraction;
            Debug.Log("Height: " + height);
            rectRaw.sizeDelta = new Vector2(1920F, height);
            Image.texture = texture;
            SizeToParent(Image, 0);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator LoadImage3D(string url)
    {
        WWW www = new WWW(url);
        //loading.SetActive(true);
        yield return www;

        if (www.error == null)
        {
            //loading.SetActive(false);
            Texture texture = www.texture;
            rend.material.SetTexture("_MainTex", texture);
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    public static Vector2 SizeToParent(RawImage image, float padding)
    {
        float w = 0, h = 0;
        var parent = image.GetComponentInParent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();

        // check if there is something to do
        if (image.texture != null)
        {
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
            padding = 1 - padding;
            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
            {
                //Invert the bounds if the image is rotated
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            //Size by height first
            h = bounds.height * padding;
            w = h * ratio;
            if (w > bounds.width * padding)
            { //If it doesn't fit, fallback to width;
                w = bounds.width * padding;
                h = w / ratio;
            }
        }
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        return imageTransform.sizeDelta;
    }
    IEnumerator GetListData(Action<TotalAPI> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(
            String.Format("http://localhost:1337/api/unity-jsons?populate=*")))
            //String.Format("http://192.168.250.198:1337/api/unity-jsons?populate=*")))
        {
            yield return req.Send();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string JSON = System.Text.Encoding.Default.GetString(result);
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            TotalAPI info = JsonConvert.DeserializeObject<TotalAPI>(JSON, settings);
            onSuccess(info);
        }
    }
}
