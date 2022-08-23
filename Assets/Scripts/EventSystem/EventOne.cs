using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EventOne : MonoBehaviour
{
    public int time;

    [SerializeField]
    private Button[] buttonArray;

    private Button currentButton;

    private int currentButtonNumber;

    private float currentTime;

    private bool action;

    private int listCounter;

    public List<Button> saveList;

    private void Start()
    {
        SetDeactive();
        RandomButton();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Lost");
        }
       
    }

    private void SetDeactive()
    {
        foreach(Button button in buttonArray)
        {
            button.gameObject.SetActive(false);
        }
    }

    void RandomButton()
    {
        if (time > 0)
        {
            if (saveList.Count <= 5)
            {
                currentTime = time;

                currentButtonNumber = Random.RandomRange(0, buttonArray.Length);

                currentButton = buttonArray[currentButtonNumber];

                foreach (var b in saveList)
                {
                    if (b == currentButton)
                    {
                        RandomButton();
                        return;
                    }
                }

                listCounter += 1;

                saveList.Add(currentButton);

                currentButton.gameObject.SetActive(true);

                action = true;

                Debug.Log(saveList.Count);
            }
            else
            {
                if (saveList.Count == 6)
                {
                    Debug.Log("Win");
                }
            }
        }
    }

    public void SmashButton(int number)
    {
        if (number == currentButtonNumber)
        {
            currentButton.gameObject.SetActive(false);

            if (listCounter == buttonArray.Length)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                RandomButton();
            }
        }
        //Debug.Log("Button: " + number);
    }
    private void OnButtonOne()
    {
        SmashButton(0);
    }
    private void OnButtonTwo()
    {
        SmashButton(2);
    }
    private void OnButtonThree()
    {
        SmashButton(3);
    }
    private void OnButtonFour()
    {
        SmashButton(1);
    }
    private void OnButtonFive()
    {
        SmashButton(4);
    }
    private void OnButtonSix()
    {
        SmashButton(5);
    }
}
