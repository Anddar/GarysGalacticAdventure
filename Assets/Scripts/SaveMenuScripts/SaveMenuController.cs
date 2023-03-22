using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenuController : MonoBehaviour
{
    // Options Menu Buttons
    [SerializeField] private Button save1Button;
    [SerializeField] private Button save2Button;
    [SerializeField] private Button save3Button;
    [SerializeField] private Button save4Button;
    [SerializeField] private Button exitButton;

    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // ------------------ BUTTON FUNCTIONS ------------------
    public void save1ButtonAction() {
        // Start Save 1
    }

    public void save2ButtonAction() {
        // Start Save 2
    }

    public void save3ButtonAction() {
        // Start Save 3
    }

    public void save4ButtonAction() {
        // Start Save 4
    }

    public void exitButtonAction() {
        gameObject.SetActive(false); // Closes the game saves menu
    }
    // ------------------------------------------------------

}
