using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoTestScript : MonoBehaviour
{
    [SerializeField] private SerialController serialController;

    public void PlayAudio()
    {

    }

    public void TurnLightOn()
    {
        serialController.SendSerialMessage("1");
    }

    public void TurnLightOff()
    {
        serialController.SendSerialMessage("0");
    }
}
