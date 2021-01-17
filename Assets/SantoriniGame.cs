using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantoriniGame : MonoBehaviour
{
    public int playerPlaceUnits = 0;

    public int playerTurn = 0;

    string phase = "placeunits";

    int playerSelectedRow = -1;
    int playerSelectedCol = -1;

    public int winner = -1;

    public class Tile
    {
        public int building = -1;
        public int player = -1;

        public bool playerSelected = false;
        public bool possibleToMove = false;
        public bool possibleToBuild = false;
    }

    Tile[,] grid = new Tile[5, 5];

    public SantoriniGame()
    {
        for (int col = 0; col < grid.GetLength(0); col++)
        {
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                grid[row, col] = new Tile();
            }
        }
    }

    // public void startGame()
    // {

    // }



    public Tile[,] stepForward(int row, int col)
    {

        if (phase.Equals("placeunits"))
        {
            placeUnits(row, col);
        }
        else if (phase.Equals("selectunit"))
        {
            playerSelected(row, col);
        }
        else if (phase.Equals("movePlayer"))
        {
            movePlayer(row, col);
        }
        else if (phase.Equals("buildBuilding"))
        {
            buildBuilding(row, col);
        }


        if (phase == "playerWin")
        {

        }

        if (playerTurn >= 2)
        {
            playerTurn = 0;
        }

        return grid;
    }

    public void playerSelected(int row, int col)
    {
        Tile currentTile = grid[row, col];

        if (currentTile.player == playerTurn)
        {
            grid[row, col].playerSelected = true;
            updatePossiblePlacesToMove(row, col);
            playerSelectedRow = row;
            playerSelectedCol = col;
            phase = "movePlayer";
        }
    }

    public void movePlayer(int row, int col)
    {
        if (grid[row, col].possibleToMove == true)
        {
            grid[row, col].player = playerTurn;
            grid[playerSelectedRow, playerSelectedCol].player = -1;
            grid[playerSelectedRow, playerSelectedCol].playerSelected = false;
            resetPossiblePlacesToMoveAndBuild();
            updatePossiblePlacesToBuild(row, col);
            phase = "buildBuilding";

            if (grid[row, col].building == 3)
            {
                winner = playerTurn;
            }
        }
        // updatedPossilbePlacesToBuild
        // Change State
    }

    public void buildBuilding(int row, int col)
    {
        if (grid[row, col].possibleToBuild == true)
        {
            grid[row, col].building++;

            resetPossiblePlacesToMoveAndBuild();
            playerTurn++;

            phase = "selectunit";
        }
    }


    public void resetPossiblePlacesToMoveAndBuild()
    {
        for (int col = 0; col < grid.GetLength(0); col++)
        {
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                grid[row, col].possibleToMove = false;
                grid[row, col].possibleToBuild = false;
            }
        }
    }

    public void updatePossiblePlacesToMove(int row, int col)
    {
        // x x x
        // x x x
        // x x x 

        Tile playerTile = grid[row, col];

        int[] rows = new int[] { 1, -1, 0, 0, 1, -1, -1, 1 };
        int[] cols = new int[] { 0, 0, 1, -1, 1, 1, -1, -1 };

        for (int i = 0; i < rows.Length; i++)
        {
            int nRow = rows[i] + row;
            int nCol = cols[i] + col;

            if (nRow >= 0 && nCol >= 0 && nRow < grid.GetLength(0) && nCol < grid.GetLength(1))
            {
                Debug.Log("NEW CLICK possible to move");
                Debug.Log(grid.Length);
                Debug.Log(nRow);
                Debug.Log(nCol);
                Tile currentTile = grid[nRow, nCol];

                // TODO: Dont highlight building with a cap or 2 lvls higher
                if (currentTile.player == -1 && currentTile.building < 3 && (currentTile.building - playerTile.building) <= 1)
                {
                    // Debug.Log("HIGHLIGHT")
                    grid[nRow, nCol].possibleToMove = true;
                }
            }
        }

    }

    public void updatePossiblePlacesToBuild(int row, int col)
    {
        // x x x
        // x x x
        // x x x 

        int[] rows = new int[] { 1, -1, 0, 0, 1, -1, -1, 1 };
        int[] cols = new int[] { 0, 0, 1, -1, 1, 1, -1, -1 };

        for (int i = 0; i < rows.Length; i++)
        {
            int nRow = rows[i] + row;
            int nCol = cols[i] + col;

            if (nRow >= 0 && nCol >= 0 && nRow < grid.GetLength(0) && nCol < grid.GetLength(1))
            {
                Debug.Log("NEW CLICK possible to build");
                Debug.Log(grid.Length);
                Debug.Log(nRow);
                Debug.Log(nCol);

                Tile currentTile = grid[nRow, nCol];

                // TODO: Dont highlight building with a cap or 2 lvls higher
                if (currentTile.player == -1 && currentTile.building < 3)
                {
                    // Debug.Log("HIGHLIGHT")
                    grid[nRow, nCol].possibleToBuild = true;
                }
            }
        }

    }

    public void placeUnits(int row, int col)
    {
        if (playerPlaceUnits < 4 && grid[row, col].player == -1)
        {
            grid[row, col].player = playerTurn;
            playerPlaceUnits++;
            playerTurn++;

        }

        if (playerPlaceUnits == 4)
        {
            phase = "selectunit";
        }
    }

    public void playerMove(int row, int col)
    {

    }

    public Tile[,] getState()
    {
        return grid;
    }
}
