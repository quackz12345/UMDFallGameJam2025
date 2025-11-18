using UnityEngine;

public class ProceduralDestructibleWall : MonoBehaviour
{
    [Header("Wall Settings")]
    public int piecesX = 3;
    public int piecesY = 3;
    public int piecesZ = 1;
    public float pieceSpacing = 0.01f;

    [Header("Explosion Settings")]
    public float explosionForce = 200f;
    public float explosionRadius = 2f;
    public float explosionUpward = 0.5f;

    [Header("Forward Motion")]
    public float forwardVelocity = 5f; // Speed to push pieces forward (Z axis)

    private bool broken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (broken) return;
        if (other.CompareTag("Player"))
        {
            BreakWall();
        }
    }

    public void BreakWall()
    {
        broken = true;

        Vector3 pieceSize = new Vector3(
            transform.localScale.x / piecesX,
            transform.localScale.y / piecesY,
            transform.localScale.z / piecesZ
        );

        Vector3 startPos = transform.position - transform.localScale / 2f + pieceSize / 2f;

        for (int x = 0; x < piecesX; x++)
        {
            for (int y = 0; y < piecesY; y++)
            {
                for (int z = 0; z < piecesZ; z++)
                {
                    Vector3 pos = startPos + new Vector3(
                        x * (pieceSize.x + pieceSpacing),
                        y * (pieceSize.y + pieceSpacing),
                        z * (pieceSize.z + pieceSpacing)
                    );

                    GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    piece.transform.position = pos;
                    piece.transform.localScale = pieceSize;

                    Rigidbody rb = piece.AddComponent<Rigidbody>();
                    rb.mass = 1f;

                    // Apply explosion force
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward, ForceMode.Impulse);

                    // Push forward along Z
                    rb.velocity = Vector3.forward * forwardVelocity;

                    Destroy(piece, 5f); // optional: cleanup pieces
                }
            }
        }

        Destroy(gameObject);
    }
}
