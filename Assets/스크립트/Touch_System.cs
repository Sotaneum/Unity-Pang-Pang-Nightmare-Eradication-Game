using UnityEngine;
using System.Collections;

public class Touch_System : MonoBehaviour {
    public float _deleteSpeed = 0;
    public bool Active = false;
    public GameObject GM;
    public Camera camera;
    public float time = 0f;
    void Start()
    {
        StartCoroutine(deletePoint());
    }



    void Update()
    {
        if (Active == true)
        {
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        GM.SendMessage("GetScore");
                        Destroy(gameObject);
                    }
                }
            }
        }
    }


    IEnumerator deletePoint()
    {
        
        while (Active == false) yield return new WaitForSeconds(0.1f);
        while (time <= _deleteSpeed)
        {
            yield return new WaitForSeconds(1f);
            time += 1f;
            while (Active == false) yield return new WaitForSeconds(0.001f);
        }
        GM.SendMessage("LoseLife");
        Destroy(gameObject);
    }
}