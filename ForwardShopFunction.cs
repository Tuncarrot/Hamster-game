using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShopFunction : MonoBehaviour
{
    public GameObject forward_page;
    public GameObject current_page;

    public void GoForward()
    {
        current_page.SetActive(false);
        forward_page.SetActive(true);
    }
}
