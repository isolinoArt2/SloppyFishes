using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public float animationSpeed = 1f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // Corrected 'meshRenderer' capitalization
    }

    private void Update()
    {
        // Calculate the new texture offset based on the animationSpeed
        float offsetX = Time.time * animationSpeed;
        Vector2 offset = new Vector2(offsetX, 0);

        // Apply the new texture offset to create the parallax effect
        meshRenderer.material.mainTextureOffset = offset;
    }
}
