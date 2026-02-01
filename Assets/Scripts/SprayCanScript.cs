using UnityEngine;

public class SprayCanScript : MonoBehaviour
{
    public Material mat;
    Color color;
    public Color SprayColor { get => color; set => color = value; }
    Camera cam;
    public LayerMask layer;
    public ParticleSystem ps;
    public CanvasPaintingScript canvas;
    private bool followMouse = false;
    public float zDistanceFromCamera = 0.5f;

    public Vector2 minMaxZDistance = new Vector2(0.5f, 2f);
    public float scrollAmount = 0f;

    private AudioSource audioSource;
    
    public bool FollowingMouse
    {
        get => followMouse;
        set => followMouse = value;
    }

    private void Awake()
    {
        mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
        color = mat.color;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
        canvas = FindFirstObjectByType<CanvasPaintingScript>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (followMouse)
        {
            Vector3 position = Input.mousePosition;
            position.z = zDistanceFromCamera;
            position = cam.ScreenToWorldPoint(position);
            transform.position = position;

            if (Input.GetMouseButton(0) && canvas.isPainting)
            {
                ps.transform.rotation = Quaternion.LookRotation((canvas.paintWorldPos - ps.transform.position).normalized, Vector3.up);
                ps.enableEmission = true;
                if (!audioSource?.isPlaying ?? false)
                    audioSource.Play();
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                followMouse = false;
            }
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.tag == "spraycan")
                {
                    color = hit.collider.GetComponent<MeshRenderer>().material.GetColor("_Color");
                    mat.SetColor("_Color", color);
                    var col = ps.colorOverLifetime;
                    Gradient gradient = new Gradient();
                    gradient.SetKeys( new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
                    col.color = gradient;
                }
            }
        }

        if (!Input.GetMouseButton(0))
        {
            ps.enableEmission = false;
            if (audioSource?.isPlaying ?? false)
                audioSource.Stop();
        }

        if (Input.mouseScrollDelta.y != 0 && followMouse)
        {
            scrollAmount += Input.mouseScrollDelta.y * 0.1f;
            scrollAmount = Mathf.Clamp(scrollAmount, 0, 1);
        }
        
        zDistanceFromCamera = Mathf.Lerp(minMaxZDistance.x, minMaxZDistance.y, scrollAmount);
        
    }

    private void OnMouseDown()
    {
        Debug.Log("clicka de spraya");
        if((!StencilScript.current?.FollowingMouse) ?? true)
        {
            followMouse = true;
        }
    }
}
