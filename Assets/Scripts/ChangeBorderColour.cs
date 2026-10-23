using UnityEngine;
using UnityEngine.UI;

public class ChangeBorderColour : MonoBehaviour
{

    private Image borderImage; // Reference to the Image component of the border
    [SerializeField] private float speed = 1.0f; // Speed of color change
    private Color currentColor; // Current color of the border
    private Color targetColor; // Target color of the border

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        borderImage = GetComponent<Image>();
        currentColor = borderImage.color;
        targetColor = currentColor; // Initialize target color to current color

    }

    // Update is called once per frame
    void Update()
    {
        borderImage.color = Color.Lerp(borderImage.color, targetColor, speed * Time.deltaTime);

        if (borderImage.color == targetColor)
        {
            changeTargetColor(); // Change the target color when the current color reaches the target
        }
    }

    void changeTargetColor()
    {
        targetColor = new Color(Random.value, Random.value, Random.value); // Generate a random color
    }
}
