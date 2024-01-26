using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(FadeInOutColor(fade));

    }

    IEnumerator FadeInOutColor(bool fade)
    {
        if (fade)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                fadingImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                fadingImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        yield return null;
    }
}
