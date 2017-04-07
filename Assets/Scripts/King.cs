using UnityEngine;

public class King : Piece
{
    protected string BuildPosition(Vector2 pos, Vector2 dest, Vector2 orig)
    {
        Piece house = manager.board[(int)pos.x, (int)pos.y];
        string route = "";

        if (house == null)
            if (pos == dest)
            {
                if(Mathf.Abs(orig.x - pos.x) == 1)
                    route += "D";
                else if (IsRoque(pos, orig))
                    route += "D";
                else
                    route += "0";
            }   
            else
                route += "0";
        else
            if (house == this)
                route += "O";
            else if (pos == dest && house.team != team)
                route += "D";
            else
                route += "1";

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
                if (Mathf.Abs(orig.x - x) > 2 || Mathf.Abs(orig.y - y) > 1)
                    continue;

                if (x == orig.x && x == dest.x)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);

                if (y == orig.y && y == dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);

                if (x + y == orig.x + orig.y && x + y == dest.x + dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);

                if (x - y == orig.x - orig.y && x - y == dest.x - dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest, orig);
            }
        }

        return routes;
    }

    private bool IsRoque(Vector2 pos, Vector2 orig)
    {
        Piece tower = manager.board[orig.x > pos.x ? 0 : 7, (int) orig.y];

        if(tower != null && firstMovement && tower.firstMovement)
        {
            manager.roque = true;
            return true;
        }
        
        return false;
    }
}
