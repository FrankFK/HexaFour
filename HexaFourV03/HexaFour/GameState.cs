using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaFour
{
    /// <summary>
    /// An instance of this class represents the current state of the board (where are which tokens) 
    /// and can check whether a player has reached the necessary conditions for victory.
    /// </summary>
    internal class GameState
    {
        private readonly int[,] _tokens;

        public GameState(int maxRow, int maxColumn)
        {
            _tokens = new int[maxRow + 1, maxColumn + 1];
            MaxRow = maxRow;
            MaxColumn = maxColumn;
        }

        public int MaxRow { get; }
        public int MaxColumn { get; }

        public void SetToken(int playerNumber, int row, int column)
        {
            _tokens[row, column] = playerNumber;
        }
        public bool PositionIsOccupied(int row, int column)
        {
            return _tokens[row, column] != 0;
        }
        public int PlayerOnPosition(int row, int column)
        {
            return _tokens[row, column];
        }

        public bool PlayerHasConsecutiveTokens(int playerNumber, int count)
        {
            for (int row = MaxRow; row >= count - 1; row--)
            {
                for (int column = MaxColumn; column >= count - 1; column--)
                {
                    if (PlayerHasConsecutiveTokensStartingAt(playerNumber, count, row, column, -1))
                        return true;
                    if (PlayerHasConsecutiveTokensStartingAt(playerNumber, count, row, column, +1))
                        return true;
                }
            }

            return false;
        }

        private bool PlayerHasConsecutiveTokensStartingAt(int playerNumber, int count, int startRow, int startColumn, int columnDelta)
        {
            int row = startRow;
            int column = startColumn;
            for (int testIndex = 1; testIndex <= count; testIndex++)
            {
                if (row < 0 || column < 0 || column > MaxColumn)
                    return false;
                if (PlayerOnPosition(row, column) != playerNumber)
                    return false;
                row--;
                column = column + columnDelta;
            }
            return true;
        }

    }
}
