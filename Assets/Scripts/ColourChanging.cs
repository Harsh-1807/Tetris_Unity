using UnityEngine;
using System.Collections;

public class ColorChangingBorder : MonoBehaviour
{
    private SpriteRenderer squareRenderer;
    private SpriteRenderer borderRenderer;
    private float hue;

    // Background color values
    private Camera mainCamera;
    private float backgroundHue;
    private Color backgroundColor;

    void Start()
    {
        // Get the SpriteRenderer for the square and the border
        squareRenderer = GetComponent<SpriteRenderer>();

        // Find the border GameObject, ensure it has a SpriteRenderer component
        Transform borderTransform = transform.Find("Border");
        if (borderTransform != null)
        {
            borderRenderer = borderTransform.GetComponent<SpriteRenderer>();
        }

        // Set the square color to black
        squareRenderer.color = Color.black;

        // Get the main camera to change the background color dynamically
        mainCamera = Camera.main;
        backgroundHue = Random.Range(0f, 1f); // Start with a random hue

        // Start the color-changing effects
        StartCoroutine(ChangeBorderColor());
        StartCoroutine(ChangeBackgroundColor());
    }

    private IEnumerator ChangeBorderColor()
    {
        while (true)
        {
            // Increment the hue value to cycle through colors for the border
            hue += Time.deltaTime * 5f; // Adjust speed here

            // Keep hue value in the range [0, 1]
            if (hue > 1) hue -= 1;

            // Set the border color using the hue value
            if (borderRenderer != null)
            {
                borderRenderer.color = Color.HSVToRGB(hue, 1, 1);
            }

            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator ChangeBackgroundColor()
    {
        while (true)
        {
            // Slowly shift the hue of the background
            backgroundHue += Time.deltaTime * 0.03f; // Slower transition for the background

            // Keep background hue within range
            if (backgroundHue > 1) backgroundHue -= 1;

            // Create a gradient-like color effect
            backgroundColor = Color.HSVToRGB(backgroundHue, 0.8f, 0.9f); // Slightly desaturated, soft brightness

            // Apply the color to the camera background
            if (mainCamera != null)
            {
                mainCamera.backgroundColor = backgroundColor;
            }

            yield return null; // Wait for the next frame
        }
    }
}
