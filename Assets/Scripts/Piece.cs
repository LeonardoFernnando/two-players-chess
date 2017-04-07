using UnityEngine;

public enum Team
{
    black,
    white
}

public abstract class Piece : MonoBehaviour
{
    public Team team;
    public bool firstMovement = true;

    protected GameManager manager;

    [HideInInspector]
    public bool specialMove = false;
    
    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }
    
    public bool ValidMovement(Vector2 dest)
    {
        string route = BuildRoute(dest);

        if (route != "" && ValidRoute(route))
        {
            specialMove = route.Contains("S");
            return true;
        }

        return false;
    }

    private bool ValidRoute(string route)
    {
        bool foundO = false, foundD = false;

        foreach (char pos in route)
        {
            if (pos == 'O')
                foundO = true;

            if (pos == 'D')
                foundD = true;

            if (foundO && foundD)
                return true;

            if ((foundO || foundD) && pos == '1')
                return false;
        }

        return false;
    }

    protected abstract string BuildRoute(Vector2 dest);

    protected string BuildPosition(Vector2 pos, Vector2 dest)
    {
        Piece house = manager.board[(int)pos.x, (int)pos.y];
        string route = "";

        if (house == null)
            if (pos == dest)
                route += "D";
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
}
