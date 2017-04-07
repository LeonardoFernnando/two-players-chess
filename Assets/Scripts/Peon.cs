using UnityEngine;

public class Peon : Piece
{
    [HideInInspector]
    public int direction;
    
    void Start()
    {
        if (transform.localPosition.y == 1)
            direction = 1;
        else
            direction = -1;
    }

    private string BuildPosition(Vector2 pos, Vector2 dest, Vector2 orig)
    {
        Piece house = manager.board[(int)pos.x, (int)pos.y];
        string route = "";

        if (orig == pos)
        {
            route += "O";
        }
        else
        {
            if (house == null)
            {
                if(pos == dest)
                {
                    if (orig.x == pos.x)
                    {
                        if (Mathf.Abs(orig.y - pos.y) == 1)
                            route += "D";
                        else if (Mathf.Abs(orig.y - pos.y) == 2 && firstMovement)
                            route += "DS";
                        else
                            route += "0";
                    }
                    else
                    {
                        if (orig.x != pos.x && IsEnPassant(pos))
                            route += "D";
                        else
                            route += "0";
                    }
                }
            }
            else
            {
                if (orig.x != pos.x && house.team != team)
                    route += "D";
                else
                    route += "1";
            }
        }

        return route;
    }

    protected override string BuildRoute(Vector2 dest)
    {
        string routes = "";

        Vector2 orig = new Vector2(transform.localPosition.x, transform.localPosition.y);

        for (int x = 0; x < GameManager.width; x++)
        {
            for (int y = 0; y < GameManager.height; y++)
            {
                if (Mathf.Abs(orig.x - x) > 1 || Mathf.Abs(orig.y - y) > 2) //Duas casa mudar aqui!!!
                    continue;

                if(orig.x != x || orig.y != y)
                {
                    if (direction > 0 && y <= orig.y)
                        continue;

                    if (direction < 0 && y >= orig.y)
                        continue;
                }
                
                if (orig.x == x && dest.x == x)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);
                    
                if (x + y == orig.x + orig.y && x + y == dest.x + dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);

                if (x - y == orig.x - orig.y && x - y == dest.x - dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);
            }
        }
        Debug.Log(routes);
        return routes;
    }

    private bool IsEnPassant(Vector2 pos)
    {
        Piece piece = manager.board[(int)pos.x, (int)pos.y - direction];

        if(piece != null && piece.team != team && piece.specialMove)
        {
            manager.enPassant = true;
            return true;
        }

        return false;
    }
}
