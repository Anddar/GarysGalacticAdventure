using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsToggles : MonoBehaviour
{
    [SerializeField] private GameObject crouchToggleBackground;
    [SerializeField] private Sprite crouchToggleChecked;
    [SerializeField] private Sprite crouchToggleUnchecked;

    public void toggleCrouch(bool state) {
        // Changes between checked and unchecked sprite when clicking the checkbox
        if (state) {
            crouchToggleBackground.GetComponent<Image>().sprite = crouchToggleChecked;
        } else {
            crouchToggleBackground.GetComponent<Image>().sprite = crouchToggleUnchecked;
        }
        GameplayManager.flipToggleCrouch();
    }
}
