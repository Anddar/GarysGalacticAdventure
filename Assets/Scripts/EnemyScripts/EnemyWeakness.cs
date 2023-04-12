using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakness : MonoBehaviour
{
    [SerializeField] private bool turnOnWeakness;
    private SpriteRenderer weaknessRenderer;
    private List<Color> weakness_colors;

    private string weakness_color;
    private bool colorSet;

    // Start is called before the first frame update
    void Start()
    {
        weakness_colors = new List<Color>();
        weakness_colors.Add(new Color32(228, 89, 22, 255)); // Orange
        weakness_colors.Add(new Color32(19, 155, 252, 255)); // Blue
        weakness_colors.Add(new Color32(121, 216, 4, 255)); // Green
        weakness_colors.Add(new Color32(126, 4, 197, 255)); // Purple

        weakness_color = "";
        weaknessRenderer = gameObject.GetComponent<SpriteRenderer>();

        colorSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOnWeakness && !colorSet) {
            // Generating a random weakness for the enemy
            int index = Random.Range(0, weakness_colors.Count);
            weaknessRenderer.material.color = weakness_colors[index];
            weakness_color = getWeaknessColor(index);
            colorSet = true;
        } else if (!turnOnWeakness) { gameObject.SetActive(false); }
    }

    // This function will set the color that this enemy is weak to so other scripts can reference this
    // variable. RandomIndex is from the random index generated in the start function of this script.
    private string getWeaknessColor(int randomIndex) {
        switch (randomIndex) {
            case 0:
                return "Orange";
            case 1:
                return "Blue";
            case 2:
                return "Green";
            case 3:
                return "Purple";
            default:
                return "Orange";
        }
    }

    // Used in other scripts to get the current enemies weakness type
    public string getWeakness() {
        return weakness_color;
    }
}
