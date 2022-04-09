using System;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField]
    private ComputeShader CS_Heightmap_Generator;

    public RenderTexture renderTexture;
    public Texture2D targetTexture2D;
    private Renderer renderer;
    public ComputeShader CS_Heightmap;

    
    public Vector2 MapResolution;

    [Range(0, 1)]
    public float persistance;
    [Range(0.001f, 30.0f)]
    public float frequency;
    [Range(0.001f, 1.6f)]
    public float lacunarity;
    public float threshold;

    [Range(0.0f, 1.0f)]
    public float waterLevel;

    public float xOffset;
    public float yOffset;

    private Vector3 MouseWorldPosition;
    private Vector4 MouseUVPosition;

    public Plane MouseReferencePlane { get; private set; }

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        renderTexture = new RenderTexture((int)MapResolution.x, (int)MapResolution.y, 24);
        targetTexture2D = new Texture2D((int)MapResolution.x, (int)MapResolution.y, TextureFormat.RGB24, false);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        MouseReferencePlane = new Plane(Vector3.up, Vector3.zero);
    }
    
	void Start()
    {
                      

    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GenerateHeightMap();
        }
        
        UpdateMousePositionOnMap();
    }

    private void UpdateMousePositionOnMap()
    {
                
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float entry;
        if (MouseReferencePlane.Raycast(ray, out entry))
        {
            MouseWorldPosition = ray.GetPoint(entry);
            MouseUVPosition.x = Map(MouseWorldPosition.x,-transform.localScale.x/2, transform.localScale.x/2,0,1);
            MouseUVPosition.y = Map(MouseWorldPosition.z, -transform.localScale.y / 2, transform.localScale.y / 2, 0, 1);
            renderer.material.SetVector("_MousePosition", MouseUVPosition);
                    
        }
        
    }

    private float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromAbs = from - fromMin;
        float fromMaxAbs = fromMax - fromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = toMax - toMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + toMin;

        return to;
    }

    private void GenerateHeightMap()
    {
        CS_Heightmap_Generator.SetTexture(0, "output_texture", renderTexture);
        CS_Heightmap_Generator.SetFloat("frequency", frequency);
        CS_Heightmap_Generator.SetFloat("persistance", persistance);
        CS_Heightmap_Generator.SetFloat("lacunarity", lacunarity);


        CS_Heightmap_Generator.SetFloat("xOffset", xOffset);
        CS_Heightmap_Generator.SetFloat("yOffset", yOffset);
        CS_Heightmap_Generator.SetFloat("threshold", threshold);
        CS_Heightmap_Generator.SetFloat("waterLevel", waterLevel);
        CS_Heightmap_Generator.SetVector("MapResolution", MapResolution);


        CS_Heightmap_Generator.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        toTexture2D(renderTexture, targetTexture2D);



        renderer.material.mainTexture = targetTexture2D;
    }

    private void toTexture2D(RenderTexture rTex, Texture2D target)
    {

        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        target.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        target.Apply();
        return;
    }

}
