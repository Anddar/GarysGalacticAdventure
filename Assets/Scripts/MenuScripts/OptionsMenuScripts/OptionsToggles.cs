using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsToggles : MonoBehaviour
{
    [SerializeField] private GameObject crouchToggleBackground;
    [SerializeField] private GameObject invincibilityToggleBackground;
    [SerializeField] private Sprite toggleChecked;
    [SerializeField] private Sprite toggleUnchecked;

    private PlayerUILogicScript gameLogic;

    void Start() {
        // Updates the toggle crouch checkbox if the state of the toggle was found to be false from an external Options File
        if (!GameplayManager.getToggleCrouchState()) {
            gameObject.GetComponent<Toggle>().isOn = GameplayManager.getToggleCrouchState();
            crouchToggleBackground.GetComponent<Image>().sprite = toggleUnchecked;
        }

        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
    }

    public void toggleCrouch(bool state) {
        // Changes between checked and unchecked sprite when clicking the checkbox
        if (state) {
            crouchToggleBackground.GetComponent<Image>().sprite = toggleChecked;
        } else {
            crouchToggleBackground.GetComponent<Image>().sprite = toggleUnchecked;
        }
        GameplayManager.setToggleCrouch(state);
    }

    public void toggleInvicibility(bool state) {
        // Changes between whether the player will be invicible or not invincible
        gameLogic.setPlayerInvicibleState(state);
        if (state) {
            invincibilityToggleBackground.GetComponent<Image>().sprite = toggleChecked;
        } else {
            invincibilityToggleBackground.GetComponent<Image>().sprite = toggleUnchecked;
        }
    }
}
