using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TerminalInputDisplay : MonoBehaviour
{
    [Header("Terminal Screen")]
    [SerializeField] private Renderer terminalScreenRenderer; // Reference to the terminal screen's renderer

    [Header("Input Symbols")]
    [SerializeField] private Sprite[] numberSymbols; // Drag and drop sprites in order 1-9

    [Header("Layout Settings")]
    private const int maxInputLength = 4; // Fixed to 4
    [SerializeField] private Vector2 symbolSize = new Vector2(0.01f, 0.01f); // Scaled down to 0.01
    [SerializeField] private float spacingX = 0.02f; // Reduced spacing
    [SerializeField] private Vector2 startPosition = new Vector2(-0.04f, 0.2f); // Adjusted start position

    [Header("UI Buttons")]
    [SerializeField] private Button[] inputButtons; // Drag 9 buttons in order
    [SerializeField] private Button clearButton; // Optional clear button

    private List<GameObject> currentSymbols = new List<GameObject>();

    public List<GameObject> GetCurrentSymbols()
    {
        return new List<GameObject>(currentSymbols);
    }

    private void Awake()
    {
        // Validate terminal screen renderer
        if (terminalScreenRenderer == null)
        {
            Debug.LogError("Terminal screen renderer is not assigned!");
            return;
        }

        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        for (int i = 0; i < inputButtons.Length; i++)
        {
            int buttonNumber = i; // Use the index directly
            inputButtons[i].onClick.AddListener(() => AddSymbol(buttonNumber));
        }

        if (clearButton != null)
        {
            clearButton.onClick.AddListener(ClearSymbols);
        }
    }

    public void AddSymbol(int number)
    {
        // Ensure we don't exceed max input length
        if (currentSymbols.Count >= maxInputLength) return;

        // Create a new symbol as a quad
        GameObject symbolObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        symbolObject.name = $"Symbol_{number}";

        // Remove collider as we don't need it
        Destroy(symbolObject.GetComponent<Collider>());

        // Set up the renderer
        Renderer symbolRenderer = symbolObject.GetComponent<Renderer>();
        Material symbolMaterial = new Material(Shader.Find("Unlit/Transparent"));

        // Correct sprite indexing
        int spriteIndex = number; // Directly use the number as index
        symbolMaterial.mainTexture = ConvertSpriteToTexture(numberSymbols[spriteIndex]);
        symbolRenderer.material = symbolMaterial;

        // Position the symbol on the terminal screen
        Vector3 localPosition = CalculateSymbolPosition(currentSymbols.Count);
        symbolObject.transform.SetParent(terminalScreenRenderer.transform, false);
        symbolObject.transform.localPosition = localPosition;

        // Scale down to 0.01 and rotate 90 degrees on X axis
        symbolObject.transform.localScale = new Vector3(symbolSize.x, symbolSize.y, 1f);
        symbolObject.transform.localRotation = Quaternion.Euler(90, 0, 0);

        currentSymbols.Add(symbolObject);
    }

    private Vector3 CalculateSymbolPosition(int symbolIndex)
    {
        // Calculate local position based on screen space
        float xPos = startPosition.x + (symbolIndex * (symbolSize.x + spacingX));
        return new Vector3(xPos, startPosition.y, 0.01f); // Slight offset to prevent z-fighting
    }

    public void ClearSymbols()
    {
        // Remove all existing symbols
        foreach (var symbol in currentSymbols)
        {
            Destroy(symbol);
        }
        currentSymbols.Clear();
    }

    // Utility method to convert Sprite to Texture2D
    private Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        // Create a new texture from the sprite
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.rect.x,
            (int)sprite.rect.y,
            (int)sprite.rect.width,
            (int)sprite.rect.height
        );
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private void OnDestroy()
    {
        // Clean up dynamically created materials
        foreach (var symbol in currentSymbols)
        {
            if (symbol != null)
            {
                Renderer renderer = symbol.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    Destroy(renderer.material);
                }
            }
        }
    }
}