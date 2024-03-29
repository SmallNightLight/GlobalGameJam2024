using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] Image fadingImage;
    [SerializeField] bool fade;
    // Start is called before the first frame update
    void Start()
    {
        if(fadingImage == null)
        {
            Debug.LogWarning("No fade image with canvas found");
            return;
        }
/*        StartCoroutine(FadeInOutColor(fade));*/

    }

    public IEnumerator FadeInOutColor(bool fade)
    {
        if (fade)
        {
            fadingImage.color = new Color(0, 0, 0, 255);
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                if (fadingImage != null)
                // set color with i as alpha
                {
                    fadingImage.color = new Color(0, 0, 0, i);
                    yield return null;
                }
                else yield break;

            }
        }
        // fade from transparent to opaque
        else
        {
            fadingImage.color = new Color(0, 0, 0, 0);
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                if (fadingImage != null)
                // set color with i as alpha
                {
                    fadingImage.color = new Color(0, 0, 0, i);
                    yield return null;
                }
                else
                    yield break;
            }
        }
        yield return null;
    }
}
