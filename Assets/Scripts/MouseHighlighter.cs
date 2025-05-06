using UnityEngine;
using System.Collections.Generic;

public class MouseHighlighter : MonoBehaviour
{
    [Header("Highlight Settings")]
    public string selectableTag = "Interactable";
    [Range(0f, 1f)]
    public float blendAmount = 0.25f;
    public Color highlightColor = Color.yellow;
    public float maxDistance = 100f;

    struct MaterialState
    {
        public Material mat;
        public Color origColor;
        public bool hadEmission;
        public Color origEmission;
    }

    private List<MaterialState> lastStates = new List<MaterialState>();
    private Camera cam;
    private Transform currentTarget;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            Debug.LogError("MouseHighlighter requires a Camera on the same GameObject.");
    }

    void Update()
    {
        foreach (var st in lastStates)
        {
            if (st.mat == null) continue;
            if (st.mat.HasProperty("_BaseColor"))
                st.mat.SetColor("_BaseColor", st.origColor);
            else
                st.mat.SetColor("_Color", st.origColor);

            if (st.hadEmission)
            {
                st.mat.EnableKeyword("_EMISSION");
                st.mat.SetColor("_EmissionColor", st.origEmission);
            }
            else
            {
                st.mat.DisableKeyword("_EMISSION");
            }
        }
        lastStates.Clear();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, maxDistance))
        {
            currentTarget = null;
            return;
        }

        Transform t = hit.collider.transform;
        while (t != null && !t.CompareTag(selectableTag))
            t = t.parent;
        currentTarget = t;
        if (t == null)
            return;

        foreach (var rend in t.GetComponentsInChildren<Renderer>())
        {
            var materials = rend.materials;
            foreach (var mat in materials)
            {
                var state = new MaterialState();
                state.mat = mat;
                state.hadEmission = mat.IsKeywordEnabled("_EMISSION");
                state.origEmission = state.hadEmission
                    ? mat.GetColor("_EmissionColor")
                    : Color.black;

                string colorProp = mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";
                state.origColor = mat.GetColor(colorProp);

                lastStates.Add(state);

                Color orig = state.origColor;
                Color blended = Color.Lerp(orig, highlightColor, blendAmount);
                blended.a = orig.a;

                mat.SetColor(colorProp, blended);

                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", highlightColor * blendAmount);
            }
        }

        if (Input.GetMouseButtonDown(0) && currentTarget != null)
        {
            var src = currentTarget.GetComponentInChildren<AudioSource>();
            if (src != null && src.clip != null)
            {
                src.PlayOneShot(src.clip);
            }
            else
            {
                Debug.LogWarning($"No AudioSource with clip found on '{currentTarget.name}'");
            }
        }
    }
}
