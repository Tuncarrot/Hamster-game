using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackShopFunction : MonoBehaviour
{
    public GameObject source;
    public GameObject current_page;

    public void GoBack()
    {
        current_page.SetActive(false);
        source.SetActive(true);
    }
}
