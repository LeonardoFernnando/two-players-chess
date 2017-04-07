using UnityEngine;

public class Horse : Piece
{
    protected override string BuildRoute(Vector2 dest)
    {
        string route = "";

        Vector2 orig = new Vector2(transform.localPosition.x, transform.localPosition.y);

        for (int x = 0; x < GameManager.width; x++)
        {
            for (int y = 0; y < GameManager.height; y++)
            {
                if (orig.x == x && orig.y == y)
                    route += BuildPosition(new Vector2(x, y), dest);

                if (x == orig.x)
                    continue;

                if (y == orig.y)
                    continue;

                if (x + y == orig.x + orig.y)
                    continue;

                if (x - y == orig.x - orig.y)
                    continue;

                if (Mathf.Abs(orig.x - x) > 2 || Mathf.Abs(orig.y - y) > 2)
                    continue;

                if (dest.x == x && dest.y == y)
                    route += BuildPosition(new Vector2(x, y), dest);
            }
        }

        return route;
    }
}
