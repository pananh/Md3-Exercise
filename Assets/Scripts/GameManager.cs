using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList;
    
    private List<Renderer> playerRendererList = new List<Renderer>();
    private List<Renderer> selectedPlayerRenderList = new List<Renderer>();

    [System.Serializable]
    public class ColorData // JSON khong luu duoc Color nen phai tao class nay de luu
    {
        public float r, g, b, a;    // RGBA values 
    }
    [System.Serializable]
    public class ColorDataList // JSON khong luu duoc List<> nen phai tao class nay de luu
    {
        public List<ColorData> list = new List<ColorData>();
    }

    public ColorDataList colorList = new ColorDataList();
    private string filePath; 
    
    private Material orginalMaterial;
    private Color originalColor = Color.blue;
    private bool sharedMaterial;

    void Awake()
    {
        //filePath = Application.persistentDataPath + "/Resources/color.json";
        filePath = "C:/Temp/color.json";
    }

    void Start()
    {
        GameInit();

        // Test code: Slected only the first player
        selectedPlayerRenderList.Add(playerRendererList[0]);

    }

    private void ChangeSelectedItemsColor(Color color)
    {
        foreach (Renderer renderer in selectedPlayerRenderList)
        {
            SetColorToPlayer(renderer, color, sharedMaterial);
        }
    }

    private void InitNewColor()
    {
        sharedMaterial = true;
        SetAllColor(originalColor);
    }

    private void SetAllColor(Color color)
    {
        if (sharedMaterial == true)
        {
            playerRendererList[0].sharedMaterial.color = color;
        }
        else
        {
            for (int i = 0; i < playerRendererList.Count; i++)
            {
                playerRendererList[i].material.color = color;
            }
        }
        UpdateColorList();
    }

    private void UpdateColorList()
    {
        for (int i = 0; i < playerRendererList.Count; i++)
        {
            Color color;
            if (sharedMaterial)
            {
                color = playerRendererList[0].sharedMaterial.color;
            }
            else
            {
                color = playerRendererList[i].material.color;
            }
            ColorData colorData = new ColorData { r = color.r, g = color.g, b = color.b, a = color.a };
            if (i < colorList.list.Count)
            {
                colorList.list[i] = colorData;
            }
            else
            {
                colorList.list.Add(colorData);
            }
        }

    }

    private void LoadColorFromFile()
    {
        string jsonString = System.IO.File.ReadAllText(filePath);
        colorList = JsonUtility.FromJson<ColorDataList>(jsonString);

        if (colorList.list.Count != playerRendererList.Count)
        {
            Debug.LogWarning("Color count does not match player count. Initializing new colors.");
            return;
        }
        for (int i = 0; i < colorList.list.Count; i++)
        {
            ColorData colorData = colorList.list[i];
            Debug.Log($"Color {i}: R={colorData.r}, G={colorData.g}, B={colorData.b}, A={colorData.a}");
        }



        sharedMaterial = false;
        for (int i = 0; i < colorList.list.Count; i++)
        {
            ColorData colorData = colorList.list[i];
            Color color = new Color(colorData.r, colorData.g, colorData.b, colorData.a);
            SetColorToPlayer(playerRendererList[i], color, sharedMaterial);
        }


    }

    private void SetColorToPlayer(Renderer renderer, Color color, bool isSharedMaterial)
    {
        if (isSharedMaterial == true)
        {
            renderer.sharedMaterial.color = color;
        }
        else
        {
            renderer.material.color = color;
        }
    }


    public void SaveColorFile()
    {
        string jsonString = JsonUtility.ToJson(colorList, true);

        System.IO.File.WriteAllText(filePath, jsonString);
    }

    private void GameInit()
    {
        GetRendererList();
        orginalMaterial = playerRendererList[0].sharedMaterial;
        InitColor();
    }


    private void GetRendererList()
    {
        foreach (GameObject player in playerList)
        {
            Renderer renderer = player.GetComponentInChildren<Renderer>();
            playerRendererList.Add(renderer);
        }
        
    }

    private void InitColor()
    {
        if ( System.IO.File.Exists(filePath))
            { LoadColorFromFile(); }
        else
            { InitNewColor(); }
    }    

      public void SwitchSharedMaterial(int index)
    {
        bool previousSharedMaterial = sharedMaterial;
        switch (index)
        {
            case 0: sharedMaterial = false; break;
            case 1: sharedMaterial = true; break;
        }
        if (previousSharedMaterial == false && sharedMaterial == true)
        {
            RestoreOriginalMaterial();
        }

    }

    public void OnColorButtonClicked(int colorIndex)
    {
        Color currentColor = Color.white;
        switch (colorIndex)
        {
            case 0: currentColor = originalColor; break;
            case 1: currentColor = Color.red; break;
            case 2: currentColor = Color.green; break;
            case 3: currentColor = Color.yellow; break;
        }
        ChangeColorByButton(currentColor);
    }

    private void ChangeColorByButton(Color color)
    {
        if (sharedMaterial)
        {
            SetAllColor(color);
        }
        else
        {
            ChangeSelectedItemsColor(color);
            UpdateColorList();
        }
    }


    private void RestoreOriginalMaterial()
    {   
        foreach (Renderer renderer in playerRendererList)
        {
            renderer.sharedMaterial = orginalMaterial;
        }
    }

}
