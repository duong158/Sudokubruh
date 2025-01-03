using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator
{
    public enum DifficultyLevel
    {
        SAMPLE,
        EASY,
        MEDIUM,
        DIFFICULT
    }
    private const int BOARD_SIZE = 9;
    private const int SUBGRID_SIZE = 3;
    private const int GRID_SIZE = 9;
    private const int MIN_SQUARES_REMOVED = 5;
    private const int MAX_SQUARES_REMOVED = 50;

    private static int[,] originalGrid = new int[GRID_SIZE, GRID_SIZE];

    public static int[,] GeneratePuzzle(DifficultyLevel level)
    {
        var grid = new int[GRID_SIZE, GRID_SIZE];
        int squaresToRemove = 0;

        switch (level)
        {
            case DifficultyLevel.SAMPLE:
                squaresToRemove = 0;
                break;
            case DifficultyLevel.EASY:
                squaresToRemove = Random.Range(MIN_SQUARES_REMOVED, MIN_SQUARES_REMOVED + 5);
                break;
            case DifficultyLevel.MEDIUM:
                squaresToRemove = Random.Range(MIN_SQUARES_REMOVED + 5, MIN_SQUARES_REMOVED + 10);
                break;
            case DifficultyLevel.DIFFICULT:
                squaresToRemove = Random.Range(MIN_SQUARES_REMOVED + 40, MAX_SQUARES_REMOVED);
                break;
            default:
                break;
        }

        IntializedGrid(grid);
        SaveOriginalGrid(grid);

        while (squaresToRemove > 0)
        {
            int randRow = Random.Range(0, BOARD_SIZE);
            int randCol = Random.Range(0, BOARD_SIZE);

            if (grid[randRow, randCol] != 0)
            {
                int temp = grid[randRow, randCol];
                grid[randRow, randCol] = 0;

                if (Solver.HasUniqueSolution(grid))
                {
                    squaresToRemove--;
                }
                else
                {
                    grid[randRow, randCol] = temp;
                }
            }
        }
        return grid;
    }

    private static void SaveOriginalGrid(int[,] grid)
    {
        for (int r = 0; r < GRID_SIZE; r++)
        {
            for (int c = 0; c < GRID_SIZE; c++)
            {
                originalGrid[r, c] = grid[r, c];
            }
        }
    }

    public static int[,] GetOriginalGrid()
    {
        return originalGrid;
    }

    public static void IntializedGrid(int[,] grid)
    {
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Shuffle(numbers);
        for (int i = 0; i < GRID_SIZE; i++)
        {
            grid[0, i] = numbers[i];
        }
        FillGrid(1, 0, grid);
    }

    private static bool FillGrid(int r, int c, int[,] grid)
    {
        if (r == GRID_SIZE)
        {
            return true;
        }
        if (c == GRID_SIZE)
        {
            return FillGrid(r + 1, 0, grid);
        }
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Shuffle(numbers);
        foreach (var num in numbers)
        {
            if (IsValid(grid, r, c, num))
            {
                grid[r, c] = num;
                if (FillGrid(r, c + 1, grid))
                {
                    return true;
                }

            }
        }
        grid[r, c] = 0;
        return false;
    }

    private static bool IsValid(int[,] board, int row, int col, int val)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[i, col] == val) { return false; }
        }
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[row, i] == val) { return false; }
        }
        int subGridRow = row / SUBGRID_SIZE * SUBGRID_SIZE;
        int subGridCol = col / SUBGRID_SIZE * SUBGRID_SIZE;
        for (int r = subGridRow; r < subGridRow + SUBGRID_SIZE; r++)
        {
            for (int c = subGridCol; c < subGridCol + SUBGRID_SIZE; c++)
            {
                if (board[r, c] == val) { return false; }
            }
        }
        return true;
    }

    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
}
