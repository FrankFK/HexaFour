using System.Collections.Generic;
using System;
using Woopec.Core;

internal class Program
{
    public static void WoopecMain()
    {
        Configuration configuration = GetConfiguration();

        InitializeTheGame(configuration);
        PlayTheGame(configuration);
        FinishTheGame();
    }

    #region Configuration
    private record Configuration(double TokenRadius, int MaxRow, int MaxColumn);

    private static Configuration GetConfiguration()
    {
        return new Configuration(20.0, 4, 10);
    }
    #endregion Configuration

    #region Initialization
    private record BoardElement(string Shape, Color FillColor, double Row, double Column);

    private static void InitializeTheGame(Configuration configuration)
    {
        var boardElements = new List<BoardElement>();
        boardElements.AddRange(CreateRegularBoardElements(configuration.MaxRow, configuration.MaxColumn));
        boardElements.AddRange(CreateBorderBoardElements(configuration.MaxRow, configuration.MaxColumn));
        InitializeUserInterface(configuration, boardElements);
    }

    private static List<BoardElement> CreateRegularBoardElements(int maxRow, int maxColumn)
    {
        var boardElements = new List<BoardElement>();
        for (var row = 0; row <= maxRow; row++)
        {
            var firstColumn = 0;
            var lastColumn = maxColumn;
            if (row % 2 == 1)
            {
                firstColumn = 1;
                lastColumn = maxColumn - 1;
            }

            for (var column = firstColumn; column <= lastColumn; column += 2)
            {
                var boardElement = new BoardElement("rhombus", Colors.LightGray, row, column);
                boardElements.Add(boardElement);
            }
        }
        return boardElements;
    }

    private static List<BoardElement> CreateBorderBoardElements(int maxRow, int maxColumn)
    {
        List<BoardElement> boardElements = new();

        for (var row = -0.5; row <= maxRow + 0.5; row++)
        {
            boardElements.Add(new BoardElement("leftborder", Colors.LightGray, row, -0.5));
            boardElements.Add(new BoardElement("rightborder", Colors.LightGray, row, maxColumn + 0.5));
        }

        return boardElements;
    }

    #endregion Initialization

    #region Play
    private record UserInput(bool CancelGame, int Slot);

    private static void PlayTheGame(Configuration configuration)
    {
        while (true)
        {
            UserInput userInput = AskUserForNextSlot(configuration.MaxColumn);

            if (userInput.CancelGame)
                return;
            else
                MakeMove(userInput.Slot, configuration.TokenRadius, configuration.MaxRow);
        }
    }


    private static void MakeMove(int slot, double tokenRadius, int maxRow)
    {
        // Of course, a lot is still missing here.
        // As a first step, we create a token and display it in the right place.
        BoardElement boardElement = CreateTokenBoardElementForSlot(slot, maxRow);
        DrawBoardElement(boardElement, tokenRadius);
    }

    private static BoardElement CreateTokenBoardElementForSlot(int slot, int maxRow)
    {
        var slotCol = slot + 0.5;

        var slotRow = maxRow + 0.5;

        return new BoardElement("token", Colors.DarkBlue, slotRow, slotCol);
    }

    #endregion Play

    #region Finish
    private static void FinishTheGame()
    {
        Screen.Default.Bye();
    }
    #endregion Finish

    #region UserInterface
    private static void InitializeUserInterface(Configuration configuration, List<BoardElement> boardElements)
    {
        RegisterRhombusShape(configuration.TokenRadius);
        RegisterLeftBorderShape(configuration.TokenRadius);
        RegisterRightBorderShape(configuration.TokenRadius);
        RegisterTokenShape(configuration.TokenRadius);

        DrawBoardElements(boardElements, configuration.TokenRadius);
    }

    public static void RegisterRhombusShape(double radius)
    {
        var edgeLength = 2 * radius; // length of an edge of the rhombus

        var pen = new Pen();
        pen.Move(edgeLength / 2);
        pen.BeginPoly();
        pen.Rotate(120);
        pen.Move(edgeLength);
        pen.Rotate(120);
        pen.Move(edgeLength);
        pen.Rotate(60);
        pen.Move(edgeLength);
        pen.Rotate(120);
        pen.Move(edgeLength); var polygon = pen.EndPoly();
        Shapes.Add("rhombus", polygon);
    }

    private static void RegisterLeftBorderShape(double tokenRadius)
    {
        var pen = new Pen();

        var edgeLength = 2 * tokenRadius;

        pen.Move(edgeLength / 2);

        pen.BeginPoly();

        pen.Rotate(120);
        pen.Move(edgeLength);
        pen.Rotate(150);
        pen.Move(Math.Sqrt(3) * 2 * tokenRadius);
        pen.Rotate(150);
        pen.Move(edgeLength);

        var polygon = pen.EndPoly();

        Shapes.Add("leftborder", polygon);
    }

    private static void RegisterRightBorderShape(double tokenRadius)
    {
        var pen = new Pen();

        var edgeLength = 2 * tokenRadius;

        pen.Rotate(180);
        pen.Move(edgeLength / 2);

        pen.BeginPoly();

        pen.Rotate(-120);
        pen.Move(edgeLength);
        pen.Rotate(-150);
        pen.Move(Math.Sqrt(3) * 2 * tokenRadius);
        pen.Rotate(-150);
        pen.Move(edgeLength);

        var polygon = pen.EndPoly();

        Shapes.Add("rightborder", polygon);
    }
    private static void RegisterTokenShape(double tokenRadius)
    {
        var pen = new Pen();

        var numberOfEdges = 6;

        pen.Move(tokenRadius);
        pen.Rotate(60);

        pen.BeginPoly();
        for (var edgeCounter = 0; edgeCounter < numberOfEdges; edgeCounter++)
        {
            pen.Rotate(360.0 / numberOfEdges);
            pen.Move(tokenRadius);
        }
        var polygon = pen.EndPoly();

        Shapes.Add("token", polygon);
    }

    private static void DrawBoardElements(List<BoardElement> boardElements, double radius)
    {
        foreach (var boardElement in boardElements)
        {
            DrawBoardElement(boardElement, radius);
        }
    }
    private static void DrawBoardElement(BoardElement boardElement, double radius)
    {
        var figure = new Figure()
        {
            Shape = Shapes.Get(boardElement.Shape),
            Color = boardElement.FillColor,
            Heading = 90
        };

        var boardLowerLeftX = -300;
        var boardLowerLeftY = -100;
        var rhombusWidth = 2 * radius;
        var rhombusHeight = Math.Sqrt(3) * 2 * radius;

        figure.Position = (
                    boardLowerLeftX + boardElement.Column * rhombusWidth,
                    boardLowerLeftY + boardElement.Row * rhombusHeight
                );

        figure.IsVisible = true;
    }

    private static UserInput AskUserForNextSlot(int maxCol)
    {
        int maxSlot = maxCol - 1;
        int? numInput = Screen.Default.NumInput("Choose slot", $"Enter a slot-number in the range 0..{maxSlot}", maxSlot / 2, 0, maxSlot);
        if (numInput == null)
            return new UserInput(true, 0);
        else
            return new UserInput(false, numInput.GetValueOrDefault());
    }

    #endregion UserInterface
}

