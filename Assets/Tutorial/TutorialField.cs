using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialField : MonoBehaviour
{
    public GameObject UIElement;

    public void PointOut()
    {
        this.gameObject.SetActive(true);
        //UIElement.SetActive(true);
    }

    public void HideOut()
    {
        this.gameObject.SetActive(false);
    }
}
