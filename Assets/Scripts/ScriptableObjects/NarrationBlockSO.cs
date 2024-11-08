using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Narration Block", menuName = "ScriptableObjects/NarrationBlockSO")]
public class NarrationBlockSO : ScriptableObject
{
    public string[] lines;
}
