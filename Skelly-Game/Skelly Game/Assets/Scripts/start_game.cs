using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.IO.File.WriteAllText("Assets/Scripts/back-end/Main_menu/Signals.txt", "0");
    }

}
