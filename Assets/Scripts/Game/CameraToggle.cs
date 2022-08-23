using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraToggle : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI buttonTextField;

    private int index = 0;

    public void ChangeCam()
    {
        if (index == 0)
        {
            index++;
            
            anim.Play("CameraViewBook");
            buttonTextField.text = "Cooking";
        }
        else
        {
            index--;
            anim.Play("CameraViewReturn");
            buttonTextField.text = "Recipe Book";
        }      
    }
}
