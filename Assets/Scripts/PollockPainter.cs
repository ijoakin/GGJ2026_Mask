using UnityEngine;

public class PollockPainter : MonoBehaviour
{
    public Transform player;
    public float distanceToPaint = 5f;
    public int textureSize = 512;

    private Texture2D paintTexture;
    private float nextPaintTime = 0f;

    void Start()
    {
        // 1. Forzar un cubo de Unity si no hay malla (para asegurar UVs)
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (mf == null) mf = gameObject.AddComponent<MeshFilter>();
            mf.sharedMesh = temp.GetComponent<MeshFilter>().sharedMesh;
            DestroyImmediate(temp);
        }

        // 2. Crear textura y pintarla de blanco
        paintTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        Color[] c = new Color[textureSize * textureSize];
        for (int i = 0; i < c.Length; i++) c[i] = Color.white;
        paintTexture.SetPixels(c);
        paintTexture.Apply();

        // 3. CREAR MATERIAL UNLIT (Funciona en URP y Estándar)
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) rend = gameObject.AddComponent<MeshRenderer>();

        // Intentar usar el shader más básico posible
        Shader unlit = Shader.Find("Unlit/Texture");
        if (unlit == null) unlit = Shader.Find("Standard");

        Material mat = new Material(unlit);
        mat.mainTexture = paintTexture;
        rend.material = mat;

        Debug.Log("Sistema de pintura listo");
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < distanceToPaint && Time.time >= nextPaintTime)
        {
            AddSplash();
            nextPaintTime = Time.time + 0.05f;
        }
    }

    void AddSplash()
    {
        Color col = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.5f, 1f);
        int x = Random.Range(0, textureSize);
        int y = Random.Range(0, textureSize);
        int r = Random.Range(10, 30);

        for (int i = -r; i < r; i++)
        {
            for (int j = -r; j < r; j++)
            {
                int px = x + i; int py = y + j;
                if (px >= 0 && px < textureSize && py >= 0 && py < textureSize)
                {
                    if (i * i + j * j <= r * r)
                        paintTexture.SetPixel(px, py, col);
                }
            }
        }
        paintTexture.Apply();
    }
}