using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "start":
                Debug.Log("Delete Base");
                Destroy(gameObject);
                collider.tag = "ball";
                break;
        }
    }
}
