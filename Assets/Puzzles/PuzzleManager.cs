using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public GameObject piecePrefab;

    public Texture2D[] textures;

    public Material baseMaterial;

    public int size = 3;

    public float spacing = 1.5f;

    public float moveDuration = 0.2f;

    private PuzzlePiece selectedPiece;

    private bool swapping;

    public ParticleSystem winParticles;

    private List<PuzzlePiece> pieces =
        new List<PuzzlePiece>();

    void Start()
    {
        GeneratePuzzle();

        Shuffle();
    }

    void GeneratePuzzle()
    {
        int index = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject obj =
                    Instantiate(
                        piecePrefab,
                        transform);

                obj.transform.localPosition =
                    GridToWorld(x, y);

                PuzzlePiece piece =
                    obj.GetComponent<PuzzlePiece>();

                piece.currentPos =
                    new Vector2Int(x, y);

                piece.correctPos =
                    new Vector2Int(x, y);

                piece.manager = this;

                Material mat =
                    new Material(baseMaterial);

                mat.mainTexture =
                    textures[index];

                piece.rend.material =
                    mat;

                pieces.Add(piece);

                index++;
            }
        }
    }

    Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(
            x * spacing,
            -y * spacing,
            0);
    }

    public void SelectPiece(PuzzlePiece piece)
    {
        if (swapping)
            return;

        if (selectedPiece == null)
        {
            selectedPiece = piece;
            selectedPiece.SetSelected(true);
            return;
        }

        if (selectedPiece == piece)
        {
            selectedPiece.SetSelected(false);
            selectedPiece = null;
            return;
        }

        selectedPiece.SetSelected(false);

        StartCoroutine(
            SwapPieces(
                selectedPiece,
                piece));

        selectedPiece = null;
    }

    IEnumerator SwapPieces(
        PuzzlePiece a,
        PuzzlePiece b)
    {
        swapping = true;

        Vector2Int tempPos =
            a.currentPos;

        a.currentPos =
            b.currentPos;

        b.currentPos =
            tempPos;

        Vector3 aTarget =
            GridToWorld(
                a.currentPos.x,
                a.currentPos.y);

        Vector3 bTarget =
            GridToWorld(
                b.currentPos.x,
                b.currentPos.y);

        Vector3 aStart =
            a.transform.localPosition;

        Vector3 bStart =
            b.transform.localPosition;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime /
                 moveDuration;

            a.transform.localPosition =
                Vector3.Lerp(
                    aStart,
                    aTarget,
                    t);

            b.transform.localPosition =
                Vector3.Lerp(
                    bStart,
                    bTarget,
                    t);

            yield return null;
        }

        a.transform.localPosition =
            aTarget;

        b.transform.localPosition =
            bTarget;

        a.UpdateColor();
        b.UpdateColor();

        CheckSolved();

        swapping = false;
    }

    void Shuffle()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int random =
                Random.Range(
                    i,
                    pieces.Count);

            PuzzlePiece a =
                pieces[i];

            PuzzlePiece b =
                pieces[random];

            Vector2Int tempPos =
                a.currentPos;

            a.currentPos =
                b.currentPos;

            b.currentPos =
                tempPos;
        }

        foreach (PuzzlePiece p in pieces)
        {
            p.transform.localPosition =
                GridToWorld(
                    p.currentPos.x,
                    p.currentPos.y);
        }
        foreach (PuzzlePiece p in pieces)
        {
            p.UpdateColor();
        }
    }

    void CheckSolved()
    {
        foreach (PuzzlePiece p in pieces)
        {
            if (p.currentPos != p.correctPos)
                return;
        }

        swapping = true;

        Debug.Log("Puzzle Resuelto");

        if (winParticles != null)
            winParticles.Play();
        
        StartCoroutine(AssemblePuzzle());
    }

    IEnumerator AssemblePuzzle()
    {
        foreach (PuzzlePiece p in pieces)
        {
            p.rend.material.color = Color.white;
        }

        float finalSpacing = 1.0f;

        List<Vector3> startPositions =
            new List<Vector3>();

        List<Vector3> targetPositions =
            new List<Vector3>();

        foreach (PuzzlePiece p in pieces)
        {
            startPositions.Add(
                p.transform.localPosition);

            targetPositions.Add(
                new Vector3(
                    p.correctPos.x * finalSpacing,
                    -p.correctPos.y * finalSpacing,
                    0));
        }

        float duration = 0.7f;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            for (int i = 0; i < pieces.Count; i++)
            {
                pieces[i].transform.localPosition =
                    Vector3.Lerp(
                        startPositions[i],
                        targetPositions[i],
                        t);

                pieces[i].transform.localScale =
                    Vector3.Lerp(
                        Vector3.one,
                        Vector3.one * 1.05f,
                        t);
            }

            yield return null;
        }
    }
}