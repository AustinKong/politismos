using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasterEgg : MonoBehaviour
{
    public Sprite chungus;
    public Sprite dwayne;
    public Sprite merchant;

    public Image img;

    private int toggleIndex = 0;
    public void ToggleImage()
    {
        if (toggleIndex == 0) img.sprite = dwayne;
        else if (toggleIndex == 1) img.sprite = chungus;
        else if (toggleIndex == 2) img.sprite = merchant;

        toggleIndex++;
        if (toggleIndex > 2) toggleIndex = 0;
    }
}
