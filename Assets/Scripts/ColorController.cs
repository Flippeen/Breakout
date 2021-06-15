using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorController : MonoBehaviour
{
    [SerializeField] Color portalColor = Color.white;
    SpriteRenderer[] portals;

    private void Awake()
    {
    }

    private void OnValidate()
    {
        portals = transform.GetComponentsInChildren<SpriteRenderer>();
        if (portals.Length > 0)
            foreach (var portal in portals)
            {
                portal.color = portalColor;
                portal.GetComponentInChildren<ParticleSystem>().startColor = portalColor;
            }
    }
}
