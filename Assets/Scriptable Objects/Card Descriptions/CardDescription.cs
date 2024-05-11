using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Description", menuName = "Scriptable Objects/New Card Description")]
public class CardDescription : ScriptableObject
{
    public string cardTitle;
    public string cardDescription;
}
