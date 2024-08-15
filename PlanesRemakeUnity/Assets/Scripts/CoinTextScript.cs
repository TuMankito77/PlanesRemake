﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTextScript : MonoBehaviour
{
    Text text;
    public static int coinAmount=0;

    // Start is called before the first frame update
    void Start()
    {
        coinAmount = 0;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = coinAmount.ToString();
    }
}
