using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Saven saven;

    public List<string> fishList;

    public string savePath = "saveGame.sv";


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //saven.Save(savePath, fishList);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            //fishList = saven.Load(savePath, fishList);
        }
    }
}
