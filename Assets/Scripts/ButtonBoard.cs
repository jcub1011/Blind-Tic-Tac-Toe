using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.Board
{
    internal enum Player
    {
        None,
        X,
        O
    }

    internal record TextButton
    {
        public readonly Button Button;
        public readonly TextMeshProUGUI Text;

        public TextButton(Button bttn, TextMeshProUGUI text)
        {
            Button = bttn;
            Text = text;
        }
    }

    public class ButtonBoard : MonoBehaviour
    {
        readonly List<TextButton> _cells = new();

        private void Awake()
        {
            foreach (var button in GetComponentsInChildren<Button>())
            {
                if (!button.name.Contains("Cell", System.StringComparison.InvariantCultureIgnoreCase)) continue;
                var textButton = new TextButton(button, button.GetComponentInChildren<TextMeshProUGUI>());
                textButton.Text.text = "";
                _cells.Add(textButton);
                button.onClick.AddListener(delegate { OnButtonPressed(textButton); });
            }
        }

        void OnButtonPressed(TextButton textButton)
        {
            print($"Button {textButton.Button.name} pressed.");

            bool isX = Random.value > 0.5f;
            textButton.Text.text = isX ? "X" : "O";
            textButton.Text.color = isX ? new Color(6f / 255, 114f / 255, 202f / 255) : new Color(242f / 255, 84f / 255, 91f / 255);
            textButton.Button.interactable = false;

            var winner = CheckForWinner(out var winningLine);
            if (winner == Player.X)
            {
                print($"X Won");
            }
            else if (winner == Player.O)
            {
                print("O Won");
            }
        }

        public void ResetBoard()
        {
            foreach (var cell in _cells)
            {
                cell.Text.text = "";
                cell.Button.interactable = true;
            }
        }

        Player CheckForWinner(out bool[,] winningLine)
        {
            if (CheckForWinningLines('x', out winningLine))
            {
                return Player.X;
            }
            else if (CheckForWinningLines('o', out winningLine))
            {
                return Player.O;
            }
            else return Player.None;
        }

        bool CheckForWinningLines(char winningCharacter, out bool[,] winningLine)
        {
            var board = GenerateBoolBoard(winningCharacter);

            if (CheckColumnWin(board, out winningLine)) return true;
            else if (CheckRowWin(board, out winningLine)) return true;
            else if (CheckForwardDiagonalWin(board, out winningLine)) return true;
            else if (CheckBackwardDiagonalWin(board, out winningLine)) return true;
            else
            {
                winningLine = null;
                return false;
            }
        }

        bool CheckColumnWin(bool[,] board, out bool[,] winningLine)
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (!board[row, col]) break;
                    else if (row == 2)
                    {
                        winningLine = new bool[3, 3];
                        for (row = 0; row < 3; row++)
                        {
                            winningLine[row, col] = true;
                        }
                        return true;
                    }
                }
            }

            winningLine = null;
            return false;
        }

        bool CheckRowWin(bool[,] board, out bool[,] winningLine)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (!board[row, col]) break;
                    else if (col == 2)
                    {
                        winningLine = new bool[3, 3];
                        for (col = 0; col < 3; col++)
                        {
                            winningLine[row, col] = true;
                        }
                        return true;
                    }
                }
            }

            winningLine = null;
            return false;
        }

        bool CheckForwardDiagonalWin(bool[,] board, out bool[,] winningLine)
        {
            for (int col = 0; col < 3; col++)
            {
                if (!board[col, col])
                {
                    winningLine = null;
                    return false;
                }
                else if (col == 2)
                {
                    winningLine = new bool[3, 3];
                    for (col = 0; col < 3; col++)
                    {
                        winningLine[col, col] = true;
                    }
                    return true;
                }
            }

            winningLine = null;
            return false;
        }

        bool CheckBackwardDiagonalWin(bool[,] board, out bool[,] winningLine)
        {
            for (int col = 0; col < 3; col++)
            {
                if (!board[2 - col, col])
                {
                    winningLine = null;
                    return false; 
                }
                else if (col == 2)
                {
                    winningLine = new bool[3, 3];
                    for (col = 0; col < 3; col++)
                    {
                        winningLine[2 - col, col] = true;
                    }
                    return true;
                }
            }

            winningLine = null;
            return false;
        }

        bool[,] GenerateBoolBoard(char matchChar)
        {
            matchChar = matchChar.ToString().ToLowerInvariant()[0];
            bool[,] bitBoard = new bool[3, 3];

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    int index = y * 3 + x;
                    var text = _cells[index].Text.text.Trim().ToLowerInvariant();
                    if (string.IsNullOrEmpty(text))
                    {
                        bitBoard[y, x] = false;
                    }
                    else
                    {
                        bitBoard[y, x] = text[0] == matchChar;
                    }
                }
            }

            return bitBoard;
        }
    }
}
