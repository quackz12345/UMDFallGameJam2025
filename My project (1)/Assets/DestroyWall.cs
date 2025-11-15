using System;
using UnityEngine;

public class ProceduralDestructibleWall : MonoBehaviour
{
    public int piecesX = 10;
    public int piecesY = 10;
    public int piecesZ = 10;
    public float pieceSpacing = 0.01f;

    public float explosionForce = 350;
    public float explosionRadius = 3f;
    public float explosionUpward = 0.8f;

    private bool broken = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
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
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward, ForceMode.Impulse);

                    Destroy(piece, 5f); // optional: cleanup pieces
                }
            }
        }

        Destroy(gameObject);
    }
}
