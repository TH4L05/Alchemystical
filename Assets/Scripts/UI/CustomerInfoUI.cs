using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TK.Audio;

public class CustomerInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterTextField;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioEventList audioEventList;
    [SerializeField] private GameObject merchantObj;
    private int counter = 0;

    public void IncreaseCounter()
    {
        audioEventList.PlayAudioEventOneShot("CustomerNotification");
        counter++;
        SetText();
    }

    public void ResetCounter()
    {
        counter = 0;
        SetText();
    }

    public void DecreaseCounter()
    {
        counter--;
        if (counter < 1)
        {
            counter = 0;
        }
        SetText();
    }

    public void UpdateInfoIncrease()
    {
        if (anim == null) return;
        //Debug.Log("Play");
        anim.Play("UpdateIncrease");
    }

    public void UpdateInfoDecrease()
    {
        if (anim == null) return;
        //Debug.Log("Play2");
        anim.Play("UpdateDecrease");
    }

    public void SetText()
    {
        counterTextField.text = counter.ToString();
    }


    public void ChangeMerchantStatus(bool active)
    {
        merchantObj.SetActive(active);

        if (active)
        {
            audioEventList.PlayAudioEventOneShot("MerchantNotification");
        }
    }


}
