using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class NumberDisplay : MonoBehaviour
{
    public Image backgroundImage;  // The empty slot background
    public Image numberImage;      // The actual number sprite
    public bool isCollected { get; private set; }

    private void Start()
    {
        // Show the empty background slot
        backgroundImage.gameObject.SetActive(true);
        // Hide the number until collected
        numberImage.gameObject.SetActive(false);
        isCollected = false;
    }

    public void SetNumber(Sprite numberSprite)
    {
        numberImage.sprite = numberSprite;
        numberImage.gameObject.SetActive(true);
        isCollected = true;
    }
}