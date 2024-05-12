using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class Cheats : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventBus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(1000));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventBus<ChangeEnergyEvent>.Raise(new ChangeEnergyEvent(1000));
        }
    }
}
