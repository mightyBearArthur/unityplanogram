using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class test : MonoBehaviour
{

    private Texture2D asq;

    private Texture2D asq1;

    private bool finish = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("started");
        StartCoroutine(asd());
    }

    // Update is called once per frame
    void Update()
    {
        if (finish)
        {
            var a = 1;
        }
    }

    private IEnumerator asd()
    {
        var www = new WWW("http://cdn.networkice.com/gen_screenshots/en-US/windows/jpg-to-pdf/large/jpeg-to-pdf-02-535x535.png");
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            asq = www.texture;
            var a = GetComponent<Image>();

            var t = new Texture2D(50, 80, TextureFormat.ARGB32, false);
            t.LoadImage(www.bytes);
            t.Apply();

            asq1 = t;

            a.sprite = Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f));
            Debug.Log("done");
            finish = true;
        }
    }

}
