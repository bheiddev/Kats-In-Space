using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TerminalInputDisplay : MonoBehaviour
{
    [Header("Terminal Screen")]
    [SerializeField] private Renderer terminalScreenRenderer;

    [Header("Input Symbols")]
    [SerializeField] private Sprite[] numberSymbols; // Make sure these are assigned in inspector
    private Material symbolMaterialTemplate; // Template material to clone from

    [Header("Layout Settings")]
    private const int maxInputLength = 4;
    [SerializeField] private Vector2 symbolSize = new Vector2(0.01f, 0.01f);
    [SerializeField] private float spacingX = 0.02f;
    [SerializeField] private Vector2 startPosition = new Vector2(-0.04f, 0.2f);

    [Header("UI Buttons")]
    [SerializeField] private Button[] inputButtons;
    [SerializeField] private Button clearButton;

    private List<GameObject> currentSymbols = new List<GameObject>();

    private void Awake()
    {
        if (terminalScreenRenderer == null)
        {
            Debug.LogError("Terminal screen renderer is not assigned!");
            return;
        }

        // Create and cache the template material at startup
        InitializeSymbolMaterial();
        SetupButtonListeners();
    }

    private void InitializeSymbolMaterial()
    {
        // Load the shader from Resources folder
        Shader unlit = Shader.Find("Unlit/Transparent");
        if (unlit == null)
        {
            // Fallback to mobile shader if standard unlit isn't found
            unlit = Shader.Find("Mobile/Unlit (Supports Lightmap)");
        }

        if (unlit == null)
        {
            Debug.LogError("Could not find required shader! Please ensure Unlit/Transparent or Mobile/Unlit shader is included in build.");
            return;
        }

        symbolMaterialTemplate = new Material(unlit);
        symbolMaterialTemplate.name = "SymbolMaterialTemplate";
    }

    private void SetupButtonListeners()
    {
        // Validate number symbols array
        if (numberSymbols == null || numberSymbols.Length == 0)
        {
            Debug.LogError("Number symbols array is empty or not assigned!");
            return;
        }

        for (int i = 0; i < inputButtons.Length; i++)
        {
            if (inputButtons[i] == null)
            {
                Debug.LogError($"Button at index {i} is not assigned!");
                continue;
            }

            int buttonNumber = i;
            inputButtons[i].onClick.AddListener(() => AddSymbol(buttonNumber));
        }

        if (clearButton != null)
        {
            clearButton.onClick.AddListener(ClearSymbols);
        }
    }

    public void AddSymbol(int number)
    {
        if (currentSymbols.Count >= maxInputLength || symbolMaterialTemplate == null) return;

        // Validate sprite index
        if (number < 0 || number >= numberSymbols.Length || numberSymbols[number] == null)
        {
            Debug.LogError($"Invalid sprite index or missing sprite: {number}");
            return;
        }

        GameObject symbolObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        symbolObject.name = $"Symbol_{number}";

        Destroy(symbolObject.GetComponent<Collider>());

        // Create a new instance of the material for this symbol
        Renderer symbolRenderer = symbolObject.GetComponent<Renderer>();
        Material instanceMaterial = new Material(symbolMaterialTemplate);
        instanceMaterial.mainTexture = ConvertSpriteToTexture(numberSymbols[number]);
        symbolRenderer.material = instanceMaterial;

        Vector3 localPosition = CalculateSymbolPosition(currentSymbols.Count);
        symbolObject.transform.SetParent(terminalScreenRenderer.transform, false);
        symbolObject.transform.localPosition = localPosition;
        symbolObject.transform.localScale = new Vector3(symbolSize.x, symbolSize.y, 1f);
        symbolObject.transform.localRotation = Quaternion.Euler(90, 0, 0);

        currentSymbols.Add(symbolObject);
    }

    public List<GameObject> GetCurrentSymbols()
    {
        return new List<GameObject>(currentSymbols);
    }

    private Vector3 CalculateSymbolPosition(int symbolIndex)
    {
        float xPos = startPosition.x + (symbolIndex * (symbolSize.x + spacingX));
        return new Vector3(xPos, startPosition.y, 0.001f); // Slightly adjusted z-offset
    }

    public void ClearSymbols()
    {
        foreach (var symbol in currentSymbols)
        {
            if (symbol != null)
            {
                Renderer renderer = symbol.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    Destroy(renderer.material);
                }
                Destroy(symbol);
            }
        }
        currentSymbols.Clear();
    }

    private Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        if (sprite.texture == null)
        {
            Debug.LogError($"Sprite texture is null for sprite: {sprite.name}");
            return null;
        }

        Texture2D texture = new Texture2D(
            (int)sprite.rect.width,
            (int)sprite.rect.height,
            TextureFormat.RGBA32,
            false
        );

        try
        {
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
        catch (System.Exception e)
        {
            Debug.LogError($"Error converting sprite to texture: {e.Message}");
            Destroy(texture);
            return null;
        }
    }

    private void OnDestroy()
    {
        ClearSymbols();
        if (symbolMaterialTemplate != null)
        {
            Destroy(symbolMaterialTemplate);
        }
    }
}