using UnityEngine;

public class TrailConstraint : MonoBehaviour
{
    public bool lockYAlignment = true; // Si vrai, bloque l'alignement du Trail Renderer sur l'axe Y

    void Update()
    {
        // R�cup�re le Trail Renderer attach� � cet objet
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();

        // Calcule la nouvelle position du Trail Renderer
        Vector3 newPosition = transform.position;

        // Bloque l'alignement sur l'axe Y si n�cessaire
        if (lockYAlignment)
        {
            newPosition.y = transform.position.y; // Garde la m�me position Y
        }

        // Affecte la nouvelle position au Trail Renderer
        trailRenderer.transform.position = newPosition;
    }
}
