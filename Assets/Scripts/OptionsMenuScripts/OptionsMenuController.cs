using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    // Options Menu Buttons
    [SerializeField] private Button generalButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Button exitButton;

    // Options Menu Tabs
    [SerializeField] private GameObject generalTab;
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject videoTab;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // ------------------ BUTTON FUNCTIONS ------------------
    public void generalButtonAction() {
        audioTab.SetActive(false);
        videoTab.SetActive(false);
        generalTab.SetActive(true);
    }

    public void audioButtonAction() {
        generalTab.SetActive(false);
        videoTab.SetActive(false);
        audioTab.SetActive(true);
    }

    public void videoButtonAction() {
        audioTab.SetActive(false);
        generalTab.SetActive(false);
        videoTab.SetActive(true);
    }

    public void exitButtonAction() {
        // Makes sure that general settings tab is the first to come up.
        generalTab.SetActive(true);
        audioTab.SetActive(false);
        videoTab.SetActive(false);

        gameObject.SetActive(false); // Closes the game options menu
    }

    // ------------------------------------------------------
}
