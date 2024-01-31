using UnityEngine;


public class UtilityBox : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Start()
    {
        // Récupère le composant BoxCollider attaché à ce GameObject
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmos()
    {
        // Si le BoxCollider est null ou n'est pas actif, ne rien faire
        if (boxCollider == null || !boxCollider.enabled)
            return;

        // Récupère les dimensions du BoxCollider
        Vector3 size = boxCollider.size;
        Vector3 center = boxCollider.center;

        // Calcule les points du contour du BoxCollider
        Vector3 topLeftFront = transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
        Vector3 topRightFront = transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
        Vector3 bottomLeftFront = transform.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z) * 0.5f);
        Vector3 bottomRightFront = transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
        Vector3 topLeftBack = transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
        Vector3 topRightBack = transform.TransformPoint(center + new Vector3(size.x, size.y, size.z) * 0.5f);
        Vector3 bottomLeftBack = transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
        Vector3 bottomRightBack = transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);

        // Dessine les lignes du contour du BoxCollider
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(topLeftFront, topRightFront);
        Gizmos.DrawLine(topRightFront, topRightBack);
        Gizmos.DrawLine(topRightBack, topLeftBack);
        Gizmos.DrawLine(topLeftBack, topLeftFront);

        Gizmos.DrawLine(bottomLeftFront, bottomRightFront);
        Gizmos.DrawLine(bottomRightFront, bottomRightBack);
        Gizmos.DrawLine(bottomRightBack, bottomLeftBack);
        Gizmos.DrawLine(bottomLeftBack, bottomLeftFront);

        Gizmos.DrawLine(topLeftFront, bottomLeftFront);
        Gizmos.DrawLine(topRightFront, bottomRightFront);
        Gizmos.DrawLine(topRightBack, bottomRightBack);
        Gizmos.DrawLine(topLeftBack, bottomLeftBack);
    }
}
