using UnityEngine;

public class Tower : Piece
{
    protected override string BuildRoute(Vector2 dest)
    {
        string routes = "";

        Vector2 orig = new Vector2(transform.localPosition.x, transform.localPosition.y);

        for (int x = 0; x < GameManager.width; x++)
        {
            for (int y = 0; y < GameManager.height; y++)
            {
                if (x == orig.x && x == dest.x)
                    routes += BuildPosition(new Vector2(x, y), dest);

                if (y == orig.y && y == dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest);
            }
        }

        return routes;
    }
}
