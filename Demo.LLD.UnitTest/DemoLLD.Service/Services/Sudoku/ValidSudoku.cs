using System.Threading.Channels;

public class ValidSudoku
{
    private int _N = 9;
    private int[][] rows;
    private int[][] cols;
    private int[][] boxes;

    public ValidSudoku()
    {
        rows = new int[_N][];
        cols = new int[_N][];
        boxes = new int[_N][];
        for (int i = 0; i < _N; i++)
        {
            rows[i] = new int[_N];
            cols[i] = new int[_N];
            boxes[i] = new int[_N];
        }
    }

    public bool IsValidSudoku(string[][] board)
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                if (board[i][j] == ".") continue;
                int pos = char.Parse(board[i][j]) - '1';
                if (rows[i][pos] == -2) return false;
                rows[i][pos] = -2;
                if (cols[j][pos] == -2) return false;
                cols[j][pos] = -2;
                int idx = (i / 3) * 3 + j / 3;
                if (boxes[idx][pos] == -2) return false;
                boxes[idx][pos] = -2;
            }
        }
        return true;

    }

    public bool SolveSudoku(string[][] board)
    {
       for(int i = 0; i < board.Length; i++)
         {
            for(int j = 0; j < board[0].Length; j++)
            {
                if(board[i][j] == ".")
                {
                    for(char c = '1'; c <= '9'; c++)
                    {
                        if(IsValid(board, i, j,c))
                        {
                            board[i][j] = c.ToString();
                            if(SolveSudoku(board))
                                return true;
                            else
                                board[i][j] = ".";
                        }
                    }
                    
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsValid(string[][] board, int row, int col, char c)
    {
        for (int i = 0; i < board[0].Length; i++)
        { 
            if(board[row][i] == c.ToString()) return false;
        }
        for (int j = 0; j < board.Length; j++)
        { 
            if(board[j][col] == c.ToString()) return false;
        }

        int boxi = (row / 3) * 3;
        int boxj = (col / 3) * 3;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            { 
                if(board[boxi + x][boxj + y] == c.ToString()) return false; 
            }
        }
        return true;
    }
}