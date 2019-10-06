using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Submit")) Submit();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void Submit()
    {
        this.gameObject.SetActive(false);
    }
}
