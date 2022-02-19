using UnityEngine;
using UnityEngine.UI;

public class FlickeringScript : MonoBehaviour
{
    private Text txt;
    private bool isFading;

    private void Start()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        if (isFading)
        {
            Color c = txt.color;
            c.a -= Time.deltaTime * 2;
            txt.color = c;
            if (c.a < 0) isFading = false;
        }
        else
        {
            Color c = txt.color;
            c.a += Time.deltaTime * 2;
            txt.color = c;
            if (c.a > 1) isFading = true;
        }
    }
}
