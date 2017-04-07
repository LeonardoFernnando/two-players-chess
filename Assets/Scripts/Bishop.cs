using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    protected override string BuildRoute(Vector2 dest)
    {
        string routes = "";

        Vector2 orig = new Vector2(transform.localPosition.x, transform.localPosition.y);

        for (int x = 0; x < GameManager.width; x++)
        {
            for (int y = 0; y < GameManager.height; y++)
            {
                if (x + y == orig.x + orig.y && x + y == dest.x + dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest);

                if (x - y == orig.x - orig.y && x - y == dest.x - dest.y)
                    routes += BuildPosition(new Vector2(x, y), dest);
            }
        }

        return routes;
    }
}
