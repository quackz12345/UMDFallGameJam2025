using System;
using UnityEngine;

public class ProceduralDestructibleWall : MonoBehaviour
{
    public int piecesX = 10;
    public int piecesY = 10;
    public int piecesZ = 10;
    public float pieceSpacing = 0.01f;

    [Header("Optional: Assign one or more materials")]
    public Material[] materials; // assign in Inspector

    [Header("Tag for all pieces")]
    public string pieceTag = "Finish"; // default tag

    [Header("Explosion Settings")]
    public float explosionForce = 5f; // how fast pieces fly out
    public float upwardForce = 2f;    // vertical push
    public float forwardBias = 3f;    // how strongly they move forward
    public float randomSpread = 1f;   // general randomness

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

                    // Assign material if available
                    Renderer rend = piece.GetComponent<Renderer>();
                    if (materials != null && materials.Length > 0)
                    {
                        Material matToUse = materials[UnityEngine.Random.Range(0, materials.Length)];
                        rend.material = matToUse;
                    }

                    // Assign tag
                    piece.tag = pieceTag;

                    // Collider as trigger (so it doesn't push the player)
                    Collider col = piece.GetComponent<Collider>();
                    col.isTrigger = true;

                    // Rigidbody for movement
                    Rigidbody rb = piece.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.mass = 0.5f;

                    // Explosion force biased mostly forward (+Z)
                    Vector3 randomDir = Vector3.forward * forwardBias +
                                        new Vector3(
                                            UnityEngine.Random.Range(-randomSpread, randomSpread),
                                            UnityEngine.Random.Range(0, upwardForce),
                                            UnityEngine.Random.Range(-randomSpread, randomSpread)
                                        );
                    rb.AddForce(randomDir.normalized * explosionForce, ForceMode.Impulse);

                    Destroy(piece, 5f); // cleanup
                }
            }
        }

        Destroy(gameObject);
    }
}
