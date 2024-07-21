using UnityEngine;

public class ApplyBendShader : MonoBehaviour
{
    public Shader bendShader;
    public float bendAmount = 0.1f;

    void Start()
    {
        if (bendShader == null)
        {
            Debug.LogError("Bend shader is not assigned.");
            return;
        }

        ApplyShaderToChildren(transform);
    }

    void ApplyShaderToChildren(Transform parent)
    {
        int childCount = parent.childCount;

        for(int i = 0; i < childCount; i++)
        {
            Renderer renderer = parent.GetChild(i).GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.shader = bendShader;
                    mat.SetFloat("_BendAmount", bendAmount);
                }
            }
            ApplyShaderToChildren(parent.GetChild(i)); // Recursively apply to children
        }
    }
}
