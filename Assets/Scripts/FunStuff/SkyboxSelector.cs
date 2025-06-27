using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class SkyboxSelector : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject optionsMenu;
    public GameObject buttonPrefab;
    public Transform scrollContent;

    [Header("Preview Settings")]
    public Camera previewCamera; // Temporary camera for rendering previews
    public RenderTexture renderTexture; // Render texture for previews

    [Header("Skybox Settings")]
    public float rotationSpeed = 0f; // Initial rotation speed


    private List<Material> skyboxMaterials = new List<Material>();

    private void Start()
    {
        LoadSkyboxMaterials();
        PopulateScrollRect();
        ToggleOptionsMenu(false); // Start with the menu hidden
    }
    private void Update()
    {
        // Rotate the skybox if a speed is set
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
        }
    }
    private void LoadSkyboxMaterials()
    {
        skyboxMaterials = new List<Material>(Resources.LoadAll<Material>("Skyboxes"));
        if (skyboxMaterials.Count == 0)
        {
            Debug.LogError("No skybox materials found in Resources/Skyboxes.");
        }
    }

    private void PopulateScrollRect()
    {
        foreach (Material skyboxMaterial in skyboxMaterials)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, scrollContent);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            Image buttonImage = buttonObj.transform.Find("Image").GetComponent<Image>();

            buttonText.text = skyboxMaterial.name;
            button.onClick.AddListener(() => SetSkybox(skyboxMaterial));

            // Generate and assign preview image
            buttonImage.sprite = GeneratePreview(skyboxMaterial);
        }
    }

    private Sprite GeneratePreview(Material skyboxMaterial)
    {
        // Set the skybox for the preview camera
        RenderSettings.skybox = skyboxMaterial;
        previewCamera.targetTexture = renderTexture;

        // Render the preview
        previewCamera.Render();

        // Convert RenderTexture to Texture2D
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Create a Sprite from the Texture2D
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void SetSkybox(Material skyboxMaterial)
    {
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }

    public void ToggleOptionsMenu(bool active)
    {
        optionsMenu.SetActive(active);
    }
    // Update rotation speed from the slider value
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    public void SetSkyboxLightness(float lightlevel)
    {
        RenderSettings.skybox.SetFloat("_Exposure", lightlevel);
    }
}
