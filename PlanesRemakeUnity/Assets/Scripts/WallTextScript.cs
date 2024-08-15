using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTextScript : MonoBehaviour
{
    Text text;
    public static int wallAmount;
    // Start is called before the first frame update
    void Start()
    {
        wallAmount = 0;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = wallAmount.ToString();
    }
}
