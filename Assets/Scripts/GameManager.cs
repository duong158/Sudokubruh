using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private float _offsetX, _offsetY;
    [SerializeField] private SubGrid _subGridPrefab;
    [SerializeField] private TMP_Text _levelText;

    public StopwatchTimer StopwatchTimer;

    private bool hasGameFinished;
    private Cell[,] cells;
    private Cell selectedCell;

    private int[,] sampleGrid = Generator.GetOriginalGrid();

    private bool pencilOn = false;


    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;

    private void Start()
    {
        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;
        SpawnCells();


    }



    private void SpawnCells()
    {
        int[,] puzzleGrid = Generator.GeneratePuzzle(Generator.DifficultyLevel.DIFFICULT);

        for (int i = 0; i < GRID_SIZE; i++) //tạo cell cho bảng sudoku
        {
            Vector3 spawnPos = _startPos + i % 3 * _offsetX * Vector3.right + i / 3 * _offsetY * Vector3.up;
            SubGrid subGrid = Instantiate(_subGridPrefab, spawnPos, Quaternion.identity);
            List<Cell> subgridCells = subGrid.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                subgridCells[j].Row = startRow + j / 3;
                subgridCells[j].Col = startCol + j % 3;
                int cellValue = puzzleGrid[subgridCells[j].Row, subgridCells[j].Col];
                subgridCells[j].Init(cellValue);
                cells[subgridCells[j].Row, subgridCells[j].Col] = subgridCells[j];
            }
        }
    }

    private void GoToNextLevel()
    {
        RestartGame();
    }

    private void Update()
    {
        if (hasGameFinished || !Input.GetMouseButton(0)) { return; }
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Cell tempCell;
        if (hit && hit.collider.gameObject.TryGetComponent(out tempCell) && tempCell != selectedCell)
        {
            ResetGrid();
            selectedCell = tempCell;
            Highlight();
        }


    }

    private void ResetGrid()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].Reset();
            }
        }
    }
    //hàm nhận input của người chơi

    public void UpdateCellValue(int value)
    {
        if (hasGameFinished || selectedCell == null) { return; }
        if (!selectedCell.IsLocked)
        {
            if (pencilOn) //nhận input người dùng  vào lưới pencil
            {
                selectedCell.UpdateValue(value);
                foreach (var note in selectedCell.notes)
                {
                    if (isValid(selectedCell, cells))
                    {
                        note.UpdateNoteValue(value);
                    }


                }
                selectedCell.UpdateValue(0);
            }
            else
            {
                if (selectedCell.Value == value)
                {
                    selectedCell.UpdateValue(0); //nhập hai lần cùng giá trị thì sẽ reset
                }
                else
                {
                    selectedCell.UpdateValue(value);
                    if (selectedCell.Value == sampleGrid[selectedCell.Row, selectedCell.Col])
                    {
                        selectedCell.IsLocked = true; //lock ô lại sau khi người chơi nhập đáp án đúng(dùng lại khi làm được hàm check đáp án duy nhất)
                    }

                }

                
                foreach (var note in selectedCell.notes)
                {
                    note.Reset();
                }
            }
        }
        //xóa ô gợi ý nếu đã có số điền hợp lệ (vẫn đang bug: khi điền vào vẫn sẽ mất bất kể input đúng hay sai)
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (!cells[i, j].IsLocked && cells[i,j] != selectedCell && !selectedCell.IsIncorrect)
                {
                    foreach (var note in cells[i, j].notes)
                    {
                        cells[i, j].UpdateValue(note.Value);
                        if (!isValid(cells[i, j], cells))
                        {
                            note.Reset();
                        }
                        cells[i, j].UpdateValue(0);
                    }
                }
            }

        }

        Highlight();
        CheckWin();
    }

    private void CheckWin()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].IsIncorrect || cells[i, j].Value == 0) return;
                
            }
        }
        hasGameFinished = true;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].UpdateWin();
            }
        }

        Invoke("GoToNextLevel", 2f);

    }
    private void Highlight()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].Value == 0) cells[i, j].IsIncorrect = false;
                else { cells[i, j].IsIncorrect = !(cells[i, j].Value == sampleGrid[i, j]); }//check tính hợp lệ của kết quả
            }
        }
        int currentRow = selectedCell.Row;
        int currentCol = selectedCell.Col;
        int subGridRow = currentRow - currentRow % SUBGRID_SIZE;
        int subGridCol = currentCol - currentCol % SUBGRID_SIZE;
        //highlight cả các số giống với số trong ô được chọn
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                //reset lại màu sau mỗi lần chuyển số
                if (cells[i, j].Value == cells[currentRow, currentCol].Value && cells[i, j].IsLocked)
                {
                    cells[i, j].IsSimilar = true;
                    cells[i, j].Highlight();

                }
                else
                {
                    cells[i, j].IsSimilar = false;
                    cells[i, j].Reset();
                }

            }
            
        }

        for (int i = 0; i < GRID_SIZE; i++)
        {
            cells[i, currentCol].Highlight();
            cells[currentRow, i].Highlight();
            cells[subGridRow + i % 3, subGridCol + i / 3].Highlight();
        }

        cells[currentRow, currentCol].Select();
    }
    //Hàm xét tính hợp lệ của input
    private bool isValid(Cell cell, Cell[,] cells)
    {
        int row = cell.Row;
        int col = cell.Col;
        int value = cell.Value;
        cell.Value = 0;

        if (value == 0)
        {
            return true; // Ô trống luôn hợp lệ
        }

        // Kiểm tra trùng lặp trong hàng và cột
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (cells[row, i].Value == value && i != col) // Trùng trong hàng
            {
                cell.Value = value;
                return false;
            }

            if (cells[i, col].Value == value && i != row) // Trùng trong cột
            {
                cell.Value = value;
                return false;
            }
        }
        

        // Kiểm tra trùng lặp trong subgrid
        int subGridRow = row - row % SUBGRID_SIZE;
        int subGridCol = col - col % SUBGRID_SIZE;

        for (int r = subGridRow; r < subGridRow + SUBGRID_SIZE; r++)
        {
            for (int c = subGridCol; c < subGridCol + SUBGRID_SIZE; c++)
            {
                if (cells[r, c].Value == value && (r != row || c != col))
                {
                    cell.Value = value;
                    return false;
                }
            }
        }
        cell.Value = value;
        return true;
    }
    
    public void pencilToggles() //bật/tắt pencil
    {
        pencilOn = !pencilOn;
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        StopwatchTimer.ResetTimer();
    }

   
}