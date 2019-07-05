using UnityEngine;
using System.Collections;

public class BackGround_Color_Control : MonoBehaviour {
    public Camera camera;
    public float blinkers=0.1f;
	// Use this for initialization
    void Start()
    {
        StartCoroutine(ColorChange());
    }
	IEnumerator ColorChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkers);
            Color bgColor;
            bgColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            Color currentColor = camera.backgroundColor;
            camera.backgroundColor = Color.Lerp(currentColor, bgColor, 1.0f);
        }
    }
}
