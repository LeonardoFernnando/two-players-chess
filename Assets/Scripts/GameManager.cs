using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int width = 8;
    public static int height = 8;

    [HideInInspector]
    public Piece[,] board;

    [HideInInspector]
    public Vector2 orig;

    [HideInInspector]
    public Vector2 dest;

    //[HideInInspector]
    public Team player;

    [HideInInspector]
    public bool enPassant;

    [HideInInspector]
    public bool roque;

    private RaycastHit hit;
    
    private void Start()
    {
        UpdateBoard();

        orig = new Vector2(-1, -1);
        dest = new Vector2(-1, -1);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector2 pos = new Vector2((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.y);

                if (!InsideBoard(pos))
                    return;

                if (orig == new Vector2(-1,-1) && board[(int)pos.x, (int)pos.y] != null && board[(int)pos.x, (int)pos.y].team == player)
                    orig = pos;
                    
                if(orig != new Vector2(-1, -1) && orig != pos)
                    dest = pos;
            }
        }

        if (orig == new Vector2(-1, -1) || dest == new Vector2(-1, -1))
            return;

        if (board[(int)orig.x, (int)orig.y].ValidMovement(new Vector2((int)dest.x, (int)dest.y)))
            MovePiece();

        orig = new Vector2(-1, -1);
        dest = new Vector2(-1, -1);

        enPassant = false;
        roque = false;
    }
    
    public void UpdateBoard()
    {
        board = new Piece[width, height];
        
        foreach(Transform child in transform)
        {
            Piece piece = child.GetComponent<Piece>();

            board[(int)child.localPosition.x, (int)child.localPosition.y] = piece;

            if (piece.specialMove && piece.team != player)
                piece.specialMove = false;
        }
    }

    public static bool InsideBoard(Vector2 position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    public void MovePiece()
    {
        DestroyPiece();

        if (roque)
        {
            Vector2 destT = new Vector2(orig.x > dest.x ? 3 : 5, orig.y);

            Piece tower = board[orig.x > dest.x ? 0 : 7, (int)orig.y];

            tower.transform.localPosition = destT;
            tower.firstMovement = false;
        }

        Piece piece = board[(int)orig.x, (int)orig.y];

        piece.transform.localPosition = dest;
        piece.firstMovement = false;

        UpdateBoard();

        player = player == Team.black ? Team.white : Team.black;

        InCheck();
    }

    private void DestroyPiece()
    {
        if (board[(int)dest.x, (int)dest.y] != null)
        {
            Destroy(board[(int)dest.x, (int)dest.y].gameObject);
            return;
        }
        
        if (enPassant)
        {
            Peon peon = board[(int)orig.x, (int)orig.y].GetComponent<Peon>();
            Destroy(board[(int)dest.x, (int)dest.y - peon.direction].gameObject);
            return;
        }
    }

    private bool InCheck()
    {
        foreach (Transform child in transform)
        {
            Vector2 king = FindObjectsOfType<King>().ToList().Find(x=>x.team == player).transform.localPosition;

            Piece piece = child.GetComponent<Piece>();

            if (piece.team == player)
                continue;

            if (!piece.ValidMovement(king))
                continue;

            Debug.Log("Em check");
            return true;
        }

        return false;
    }
}
