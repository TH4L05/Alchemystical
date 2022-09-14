

using UnityEngine;
using UnityEngine.Events;
using TMPro;
using TK.Audio;
using Alchemystical;

[RequireComponent(typeof(Animator))]
public class CustomerInfoUI : MonoBehaviour
{
    public UnityEvent CustomerButtonClicked;
    public UnityEvent TraderButtonClicked;

    #region SerializedFields

    [SerializeField] private TextMeshProUGUI counterTextField;
    [SerializeField] private Animator animatorComponent;
    [SerializeField] private AudioEventList audioEventList;
    [SerializeField] private GameObject traderObj;

    #endregion

    #region PrivateFields

    private int counter = 0;
    private bool merchantAvailable;

    #endregion

    private void Start()
    {
        GameInput.ToggleConversations += ToggleConversations;
        GameTime.NewDayStarted += ResetInfo;
        Trader.ChangeTraderInfo += ChangeMerchantStatus;
        OrderSystem.IncraseCustomerUICounter += UpdateInfoIncrease;
        OrderSystem.DecraseCustomerUICounter += UpdateInfoDecrease;
    }

    private void OnDestroy()
    {
        GameInput.ToggleConversations -= ToggleConversations;
        GameTime.NewDayStarted -= ResetInfo;
        Trader.ChangeTraderInfo -= ChangeMerchantStatus;
        OrderSystem.IncraseCustomerUICounter -= UpdateInfoIncrease;
        OrderSystem.DecraseCustomerUICounter -= UpdateInfoDecrease;
    }

    private void ToggleConversations()
    {
        if (merchantAvailable)
        {
            OnTraderButtonClicked();
            return;
        }

        if (counter < 1) return;
        OnCustomerButtonClicked();
    }


    public void OnTraderButtonClicked()
    {
        TraderButtonClicked?.Invoke();
    }

    public void OnCustomerButtonClicked()
    {
        CustomerButtonClicked?.Invoke();
    }


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
        if (animatorComponent == null) return;
        animatorComponent.Play("UpdateIncrease");
    }

    public void UpdateInfoDecrease()
    {
        if (animatorComponent == null) return;
        animatorComponent.Play("UpdateDecrease");
    }

    public void SetText()
    {
        counterTextField.text = counter.ToString();
    }

    private void ResetInfo()
    {
        ResetCounter();
        ChangeMerchantStatus(false);
    }

    public void ChangeMerchantStatus(bool active)
    {
        if (traderObj == null) return;
        traderObj.SetActive(active);
        merchantAvailable = active;

        if (active)
        {
            audioEventList.PlayAudioEventOneShot("MerchantNotification");
        }
    }

}
