using UnityEngine;

[CreateAssetMenu(menuName = "Stencil")]
public class StencilScobj : ScriptableObject
{
    public Texture texture;
    public float size = 1f;
    public new string name;
    public string description;
    public StencilTag tags;
}
