using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace Kwyjibo
{
    /// <summary>
    /// Highest level Scrabble class, controls the running of a Scrabble game. This module is stateful.
    /// </summary>
    /// <remarks></remarks>
    public class ScrabbleGame
    {
        /// <summary>
        /// Stores the scrabble board.
        /// </summary>
        private Board board;

        /// <summary>
        /// Gets or sets the board.
        /// </summary>
        /// <value>The board.</value>
        /// <remarks></remarks>
        public Board Board
        {
            get { return board; }
            set { board = value; }
        }

        /// <summary>
        /// Stores the players.
        /// </summary>
        private List<Player> players;

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>The players.</value>
        /// <remarks></remarks>
        public List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        /// <summary>
        /// Stores the Scrabble word dictionary.
        /// </summary>
        private WordDict validWords;

        /// <summary>
        /// Gets or sets the valid words.
        /// </summary>
        /// <value>The valid words.</value>
        /// <remarks></remarks>
        public WordDict ValidWords
        {
            get { return validWords; }
            set { validWords = value; }
        }

        /// <summary>
        /// If game has started, stores the player who's turn it currently is.
        /// </summary>
        private Player currentPlayer;

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        /// <value>The current player.</value>
        /// <remarks></remarks>
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        /// <summary>
        /// Stores the index number of the current player.
        /// </summary>
        private int currentPlayerIndex;

        /// <summary>
        /// Gets or sets the index of the current player.
        /// </summary>
        /// <value>The index of the current player.</value>
        /// <remarks></remarks>
        public int CurrentPlayerIndex
        {
            get { return currentPlayerIndex; }
            set { currentPlayerIndex = value; }
        }

        /// <summary>
        /// Stores wether the game has started and is in progress.
        /// </summary>
        private bool hasStarted;

        /// <summary>
        /// Gets or sets a value indicating whether this game has started.
        /// </summary>
        /// <value><c>true</c> if this instance has started; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool HasStarted
        {
            get { return hasStarted; }
            set { hasStarted = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public ScrabbleGame()
        {
            players = new List<Player>();
            board = new Board();
            hasStarted = false;
        }

        /// <summary>
        /// Ends the previous game, clearing the board and players, ready for a new game.
        /// </summary>
        /// <remarks></remarks>
        public void New()
        {
            Board.ClearCells();
            Players.Clear();
            HasStarted = false;
        }

        /// <summary>
        /// Starts a game, clearing the board.
        /// </summary>
        /// <remarks></remarks>
        public void Start()
        {
            if (Players.Count == 0) 
                return;

            foreach (Player p in Players) 
                p.Words.Clear();

            Board.ClearCells();
            HasStarted = true;
            CurrentPlayerIndex = 0;
            CurrentPlayer = Players[0];
        }

        /// <summary>
        /// Ends the current player's turn and switches to the next player.
        /// </summary>
        /// <remarks></remarks>
        public void EndTurn()
        {
            if (!HasStarted) 
                return;

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CurrentPlayer = Players[CurrentPlayerIndex];
        }

        /// <summary>
        /// Finds the new words placed on the board.
        /// </summary>
        /// <returns>A list of the new words found.</returns>
        /// <remarks></remarks>
        public List<Word> FindNewWords()
        {
            List<Word> newWords = new List<Word>();

            for (int r = 0; r < Board.Rows; r++)
            {
                Word cw = new Word(CurrentPlayer);
                bool seenNewLetter = false;

                for (int c = 0; c < Board.Cols; c++)
                {
                    if (!Board.Cells[c, r].IsEmpty && !Board.Cells[c, r].contents.IsPlaced) 
                        seenNewLetter = true;

                    if (!Board.Cells[c, r].IsEmpty) 
                        cw.Cells.Add(Board.Cells[c, r]);

                    if ((Board.Cells[c, r].IsEmpty || c == Board.Cols - 1) && cw.Cells.Count > 0)
                    {
                        if (seenNewLetter && cw.Cells.Count > 1)
                            newWords.Add(cw);

                        cw = new Word(CurrentPlayer);
                        seenNewLetter = false;
                        continue;
                    }
                }
            }

            for (int c = 0; c < Board.Cols; c++)
            {
                Word cw = new Word(CurrentPlayer);
                bool seenNewLetter = false;

                for (int r = 0; r < Board.Rows; r++)
                {
                    if (!Board.Cells[c, r].IsEmpty && !Board.Cells[c, r].contents.IsPlaced) 
                        seenNewLetter = true;

                    if (!Board.Cells[c, r].IsEmpty) 
                        cw.Cells.Add(Board.Cells[c, r]);

                    if ((Board.Cells[c, r].IsEmpty || r == Board.Rows - 1) && cw.Cells.Count > 0)
                    {
                        if (seenNewLetter && cw.Cells.Count > 1)
                            newWords.Add(cw);
                        cw = new Word(CurrentPlayer);
                        seenNewLetter = false;
                        continue;
                    }
                }
            }

            return newWords;

        }
    }

    /// <summary>
    /// Stores a dictionary of words and provides a method for checking if a word is valid.
    /// </summary>
    /// <remarks></remarks>
    public class WordDict
    {
        /// <summary>
        /// The word dictionary.
        /// </summary>
        private Dictionary<string, bool> words;

        /// <summary>
        /// Gets or sets the word dictionary.
        /// </summary>
        /// <value>The word dictionary.</value>
        /// <remarks></remarks>
        public Dictionary<string, bool> Words
        {
            get { return words; }
            set { words = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordDict"/> class.
        /// </summary>
        /// <param name="path">The path to the text file to load words from.</param>
        /// <remarks></remarks>
        public WordDict(string path)
        {
            string[] lines = File.ReadAllLines(path);

            words = new Dictionary<string, bool>(lines.Length);

            foreach (string line in lines)
            {
                words.Add(line, true);
            }
        }

        /// <summary>
        /// Determines wether a word exists in the dictionary, as a string.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns><c>true</c> if the word is in the dictionary; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool Exists(string word)
        {
            return Words.ContainsKey(word);
        }

        /// <summary>
        /// Determines wether a word exists in the dictionary, as a Word object.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns><c>true</c> if the word is in the dictionary; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool Exists(Word word)
        {
            return Exists(word.ToString());
        }
    }

    /// <summary>
    /// Represents a player in a Scrabble game.
    /// </summary>
    /// <remarks></remarks>
    public class Player
    {
        /// <summary>
        /// The name of the player.
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The player's current score.
        /// </summary>
        private int score;

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        /// <remarks></remarks>
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// The list of words the player has formed on the board.
        /// </summary>
        private List<Word> words;

        /// <summary>
        /// Gets or sets the words.
        /// </summary>
        /// <value>The words.</value>
        /// <remarks></remarks>
        public List<Word> Words
        {
            get { return words; }
            set { words = value; }
        }

        /// <summary>
        /// The player's colour.
        /// </summary>
        private Color col;

        /// <summary>
        /// Gets or sets the col.
        /// </summary>
        /// <value>The col.</value>
        /// <remarks></remarks>
        public Color Col
        {
            get { return col; }
            set { col = value; }
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <remarks></remarks>
        public int Points
        {
            get
            {
                int points = 0;
                foreach (Word w in Words) 
                    points += w.Points;
                return points;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <remarks></remarks>
        public Player(string name)
        {
            this.name = name;
            words = new List<Word>();
            Random r = new Random();
            col = Color.FromArgb(100 + r.Next(155), 100 + r.Next(155), 100 + r.Next(155));
        }
    }

    /// <summary>
    /// Represents a Scrabble board.
    /// </summary>
    /// <remarks></remarks>
    public class Board
    {
        /// <summary>
        /// The number of columns. Should be 15.
        /// </summary>
        private int cols;

        /// <summary>
        /// Gets or sets the cols.
        /// </summary>
        /// <value>The cols.</value>
        /// <remarks></remarks>
        public int Cols
        {
            get { return cols; }
            set { cols = value; }
        }

        /// <summary>
        /// The number of rows. Should be 15.
        /// </summary>
        private int rows;

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        /// <remarks></remarks>
        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// The array of board cells.
        /// </summary>
        private Cell[,] cells;

        /// <summary>
        /// Gets or sets the cells.
        /// </summary>
        /// <value>The cells.</value>
        /// <remarks></remarks>
        public Cell[,] Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        /// <summary>
        /// A cached background image.
        /// </summary>
        private Bitmap background;

        /// <summary>
        /// Default layout of premium word cells.
        /// </summary>
        // '.' = normal, '+' = double, '*' = triple
	    private static string[] WordPremiumLayout = {
	        "*......*......*",
	        ".+...........+.",
	        "..+.........+..",
	        "...+.......+...",
	        "....+.....+....",
	        "...............",
	        "...............",
	        "*......+......*",
	        "...............",
	        "...............",
	        "....+.....+....",
	        "...+.......+...",
	        "..+.........+..",
	        ".+...........+.",
	        "*......*......*"
	    };

        /// <summary>
        /// Default layout of premium letter cells.
        /// </summary>
        // '.' = normal, '+' = double, '*' = triple
        private static string[] LetterPremiumLayout = {
	        "...+.......+...",
	        ".....*...*.....",
	        "......+.+......",
	        "+......+......+",
	        "...............",
	        ".*...*...*...*.",
	        "..+...+.+...+..",
	        "...+.......+...",
	        "..+...+.+...+..",
	        ".*...*...*...*.",
	        "...............",
	        "+......+......+",
	        "......+.+......",
	        ".....*...*.....",
	        "...+.......+..."
	    };

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Board()
        {
            cols = 15;
            rows = 15;
            cells = new Cell[cols, rows];
            GenerateCells();
            background = CreateBackground(320, 320);
        }

        /// <summary>
        /// Places a tile in the specified cell.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="cell">The cell.</param>
        /// <remarks></remarks>
        public void PlaceTile(Player player, string letter, Cell cell)
        {
            cell.Place(new Tile(letter, player));
        }

        /// <summary>
        /// Places a tile in the specified cell, given row and column.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="col">The col.</param>
        /// <param name="row">The row.</param>
        /// <remarks></remarks>
        public void PlaceTile(Player player, string letter, int col, int row)
        {
            Cell cell = Cells[col, row];
            cell.Place(new Tile(letter, player));
        }

        /// <summary>
        /// Places a tile in the specified cell, given coordinates.
        /// If the cell already contains a tile it will not place the new one.
        /// This method is custom for the use with the Scrabble Referee system.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="rect">The rect.</param>
        /// <remarks></remarks>
        public void PlaceTile(Player player, string letter, float x, float y, float width, float height, Rectangle rect)
        {
            Cell cell = GetCellAt(x, y, width, height);

            if (!cell.IsEmpty && cell.contents.IsPlaced) 
                return;

            if (!cell.IsEmpty && cell.contents.Rect.Width * cell.contents.Rect.Height > rect.Width * rect.Height) 
                return;

            Tile t = new Tile(letter, player);
            t.Rect = rect;
            cell.Place(t);
        }

        /// <summary>
        /// Gets the cell given coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The cell.</returns>
        /// <remarks></remarks>
        public Cell GetCellAt(float x, float y, float width, float height) 
        {
            return Cells[(int)Math.Floor((x / width) * Cols), (int)Math.Floor((y / height) * Rows)];
        }

        /// <summary>
        /// Generates the Scrabble board cells.
        /// </summary>
        /// <remarks></remarks>
        private void GenerateCells()
        {
            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    if (LetterPremiumLayout[r][c] == '+')
                    {
                        Cells[c, r] = new Cell(CellType.DoubleLetter);
                    }
                    else if (LetterPremiumLayout[r][c] == '*')
                    {
                        Cells[c, r] = new Cell(CellType.TripleLetter);
                    }
                    else if (WordPremiumLayout[r][c] == '+')
                    {
                        Cells[c, r] = new Cell(CellType.DoubleWord);
                    }
                    else if (WordPremiumLayout[r][c] == '*')
                    {
                        Cells[c, r] = new Cell(CellType.TripleWord);
                    }
                    else
                    {
                        Cells[c, r] = new Cell(CellType.SingleLetter);
                    }
                }
            }
        }

        /// <summary>
        /// Clears the cells.
        /// </summary>
        /// <remarks></remarks>
        public void ClearCells()
        {
            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    Cells[c, r].Clear();
                }
            }
        }

        /// <summary>
        /// Clears the unplaced cells.
        /// </summary>
        /// <remarks></remarks>
        public void ClearUnplacedCells()
        {
            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    if (!Cells[c, r].IsEmpty && !Cells[c, r].contents.IsPlaced) 
                        Cells[c, r].Clear();
                }
            }
        }

        /// <summary>
        /// Creates the background.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Bitmap CreateBackground(int width, int height)
        {
            Bitmap bg = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bg);

            float cellWidth = width / Cols;
            float cellHeight = height / Rows;

            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    Cell cell = Cells[c, r];
                    Color colour = ScrabbleColor.SingleLetter;

                    switch (cell.Type)
                    {
                        case CellType.SingleLetter: colour = ScrabbleColor.SingleLetter; break;
                        case CellType.DoubleLetter: colour = ScrabbleColor.DoubleLetter; break;
                        case CellType.TripleLetter: colour = ScrabbleColor.TripleLetter; break;
                        case CellType.DoubleWord: colour = ScrabbleColor.DoubleWord; break;
                        case CellType.TripleWord: colour = ScrabbleColor.TripleWord; break;
                    }

                    SolidBrush brush = new SolidBrush(colour);
                    g.FillRectangle(brush, c * cellWidth, r * cellHeight, cellWidth, cellHeight);

                    Pen pen = new Pen(Color.FromArgb(50, 50, 50));
                    g.DrawRectangle(pen, c * cellWidth, r * cellHeight, cellWidth, cellHeight);
                }
            }

            return bg;
        }

        /// <summary>
        /// Draws a representation of this board using the given graphics object.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <remarks></remarks>
        public void Draw(Graphics g, float x, float y, float width, float height)
        {
            float cellWidth = width / Cols;
            float cellHeight = height / Rows;

            //g.DrawImageUnscaled(Background, 4, 4);

            g.Clear(ScrabbleColor.SingleLetter);

            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    Cell cell = Cells[c, r];
                    
                    Color colour = ScrabbleColor.SingleLetter;

                    switch (cell.Type)
                    {
                        case CellType.SingleLetter: colour = ScrabbleColor.SingleLetter; break;
                        case CellType.DoubleLetter: colour = ScrabbleColor.DoubleLetter; break;
                        case CellType.TripleLetter: colour = ScrabbleColor.TripleLetter; break;
                        case CellType.DoubleWord: colour = ScrabbleColor.DoubleWord; break;
                        case CellType.TripleWord: colour = ScrabbleColor.TripleWord; break;
                    }
                    
                    if (cell.IsEmpty)
                    {
                        if (cell.Type != CellType.SingleLetter)
                        {
                            SolidBrush brush = new SolidBrush(colour);
                            g.FillRectangle(brush, c * cellWidth, r * cellHeight, cellWidth, cellHeight);
                        }
                    }
                    else
                    {
                        SolidBrush brush;

                        if (cell.contents.IsHighlight)
                        {
                            brush = new SolidBrush(Color.Yellow);
                        }
                        else if (cell.contents.IsPlaced)
                        {
                            brush = new SolidBrush(ScrabbleColor.Tile);
                        }
                        else
                        {
                            brush = new SolidBrush(Color.FromArgb(200, ScrabbleColor.Tile));
                        }

                        RectangleF rect = new RectangleF(c * cellWidth, r * cellHeight, cellWidth, cellHeight);
                        g.FillRectangle(brush, rect);
                        g.DrawRectangle(new Pen(cell.contents.Player.Col, 3), c * cellWidth + 2, r * cellHeight + 2, cellWidth - 4, cellHeight - 4);
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Center;
                        strFormat.LineAlignment = StringAlignment.Center;

                        if (cell.contents.Letter != "?")
                            g.DrawString(cell.contents.Letter, new Font("Tahoma", 10), Brushes.Black, rect, strFormat);
                    }
                    
                    //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    Pen pen = new Pen(Color.FromArgb(50, 50, 50));
                    g.DrawRectangle(pen, c * cellWidth, r * cellHeight, cellWidth, cellHeight);
                    
                }
            }
        }
    }

    /// <summary>
    /// The possible types of cell on a Scrabble board. SingleLetter, DoubleLetter, TripleLetter, DoubleWord, TripleWord.
    /// </summary>
    /// <remarks></remarks>
    public enum CellType { SingleLetter = 1, DoubleLetter, TripleLetter, DoubleWord, TripleWord }

    /// <summary>
    /// The colours representing each CellType.
    /// </summary>
    /// <remarks></remarks>
    public static class ScrabbleColor
    {
        public static readonly Color SingleLetter = Color.FromArgb(55, 142, 113);
        public static readonly Color DoubleLetter = Color.FromArgb(114, 138, 186);
        public static readonly Color TripleLetter = Color.FromArgb(42, 111, 208);
        public static readonly Color DoubleWord = Color.FromArgb(255, 132, 154);
        public static readonly Color TripleWord = Color.FromArgb(241, 68, 128);
        public static readonly Color Tile = Color.FromArgb(253, 232, 183);
    }

    /// <summary>
    /// Represents a single cell on a board.
    /// </summary>
    /// <remarks></remarks>
    public class Cell
    {
        /// <summary>
        /// The contents of this cell.
        /// </summary>
        public Tile contents;

        /// <summary>
        /// Gets or sets the contents.
        /// </summary>
        /// <value>The tile.</value>
        /// <remarks></remarks>
        public Tile Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        /// <summary>
        /// The type of this cell.
        /// </summary>
        private CellType type;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        /// <remarks></remarks>
        public CellType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Stores wether this cell is empty, or contains a tile.
        /// </summary>
        private bool isEmpty;

        /// <summary>
        /// Gets or sets a value indicating whether this cell is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool IsEmpty
        {
            get { return isEmpty; }
            set { isEmpty = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="type">The cell type.</param>
        /// <remarks></remarks>
        public Cell(CellType type) 
        {
            this.type = type;
            isEmpty = true;
        }

        /// <summary>
        /// Places the specified tile in this cell.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <remarks></remarks>
        public void Place(Tile tile)
        {
            contents = tile;
            IsEmpty = false;
        }

        /// <summary>
        /// Clears this cell.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            IsEmpty = true;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Enum.GetName(typeof(CellType), Type);
        }
    }

    /// <summary>
    /// Represents the standard Scrabble tile bag, with the standard distribution of tiles.
    /// </summary>
    /// <remarks></remarks>
    public class TileBag
    {
        /*
             2 blank tiles (scoring 0 points)
             1 point: E ×12, A ×9, I ×9, O ×8, N ×6, R ×6, T ×6, L ×4, S ×4, U ×4
             2 points: D ×4, G ×3
             3 points: B ×2, C ×2, M ×2, P ×2
             4 points: F ×2, H ×2, V ×2, W ×2, Y ×2
             5 points: K ×1
             8 points: J ×1, X ×1
             10 points: Q ×1, Z ×1
         */

        /// <summary>
        /// Returns the maximum initial number of tiles for the specified letter.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <returns>The maximum initial number of tiles for the specified letter.</returns>
        /// <remarks></remarks>
        public static int Distribution(string letter)
        {
            switch (letter)
            {
                case "E": return 12;
                case "A":
                case "I": return 9;
                case "O": return 8;
                case "N":
                case "R":
                case "T": return 6;
                case "L": 
                case "S": 
                case "U":
                case "D": return 4;
                case "G": return 3;
                case "K":
                case "J":
                case "X":
                case "Q":
                case "Z": return 1;
                default: return 2; // B, C, M, P, F, H, V, W, Y
            }
        }

        /// <summary>
        /// Returns the number of points given for the specified letter.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <returns>The number of points given for the specified letter.</returns>
        /// <remarks></remarks>
        public static int Points(string letter)
        {
            switch (letter)
            {
                case "Q":
                case "Z": return 10;
                case "J":
                case "X": return 8;
                case "K": return 5;
                case "F":
                case "H":
                case "V":
                case "W":
                case "Y": return 4;
                case "B":
                case "C":
                case "M":
                case "P": return 3;
                case "D":
                case "G": return 2;
                case "*": return 0;
                default: return 1;  // E, A, I, O, R, N, T, L, S, U
            }
        }
    }

    /// <summary>
    /// Represents a word, made of cells with tiles in them formed on the board.
    /// </summary>
    /// <remarks></remarks>
    public class Word
    {
        /// <summary>
        /// The player that formed this word.
        /// </summary>
        private Player player;

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>The player.</value>
        /// <remarks></remarks>
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        /// <summary>
        /// The cells which contain the tiles that form this word.
        /// </summary>
        private List<Cell> cells;

        /// <summary>
        /// Gets or sets the cells.
        /// </summary>
        /// <value>The cells.</value>
        /// <remarks></remarks>
        public List<Cell> Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        /// <summary>
        /// Whether this word has been spelling corrected.
        /// </summary>
        private bool corrected;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Word"/> has been corrected.
        /// </summary>
        /// <value><c>true</c> if corrected; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Corrected
        {
            get { return corrected; }
            set { corrected = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Word"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <remarks></remarks>
        public Word(Player player)
        {
            this.player = player;
            cells = new List<Cell>();
            corrected = false;
        }

        /// <summary>
        /// Gets the points that this word scores, including cell multipliers, by Scrabble rules.
        /// </summary>
        /// <remarks></remarks>
        public int Points
        {
            get
            {
                int sc = 0;
                int mul = 1;

                foreach (Cell c in Cells)
                {
                    if (c.Type <= CellType.TripleLetter)
                    {
                        sc += c.contents.Points * (int)c.Type;
                    }
                    else
                    {
                        sc += c.contents.Points;
                        mul *= (int)c.Type - 2;
                    }
                }

                sc *= mul;

                return sc;
            }
        }

        /// <summary>
        /// Sets the placed status all of the tiles that form this word.
        /// </summary>
        /// <param name="placed">if set to <c>true</c> [placed].</param>
        /// <remarks></remarks>
        public void SetPlaced(bool placed)
        {
            foreach (Cell c in Cells) 
                c.contents.IsPlaced = placed;
        }

        /// <summary>
        /// Sets the highlight status all of the tiles that form this word.
        /// </summary>
        /// <param name="highlight">if set to <c>true</c> [highlight].</param>
        /// <remarks></remarks>
        public void SetHighlight(bool highlight)
        {
            foreach (Cell c in Cells) 
                c.contents.IsHighlight = highlight;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            string word = "";
            foreach (Cell c in Cells)
            {
                word += c.contents.Letter;
            }
            return word;
        }
    }

    /// <summary>
    /// Represents a single Scrabble tile.
    /// </summary>
    /// <remarks></remarks>
    public class Tile
    {
        /// <summary>
        /// The letter on this tile.
        /// </summary>
        private string letter;

        /// <summary>
        /// Gets or sets the letter.
        /// </summary>
        /// <value>The letter.</value>
        /// <remarks></remarks>
        public string Letter
        {
            get { return letter; }
            set { letter = value; }
        }
        
        /// <summary>
        /// The number of points this tile is worth.
        /// </summary>
        private int points;

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        /// <remarks></remarks>
        public int Points
        {
            get { return points; }
        }
        
        /// <summary>
        /// The player who placed this tile.
        /// </summary>
        private Player player;

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>The player.</value>
        /// <remarks></remarks>
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        /// <summary>
        /// Whether this tile has been placed or not. If not, it is only temporary.
        /// </summary>
        private bool isPlaced;

        /// <summary>
        /// Gets or sets a value indicating whether this tile is placed.
        /// </summary>
        /// <value><c>true</c> if this instance is placed; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool IsPlaced
        {
            get { return isPlaced; }
            set { isPlaced = value; }
        }

        /// <summary>
        /// Whether this tile is to be highlighted in the graphical output.
        /// </summary>
        private bool isHighlight;

        /// <summary>
        /// Gets or sets a value indicating whether this tile is highlighted.
        /// </summary>
        /// <value><c>true</c> if this instance is highlighted; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool IsHighlight
        {
            get { return isHighlight; }
            set { isHighlight = value; }
        }

        /// <summary>
        /// Stores the blob rectangle. Custom field for the Scrabble referee.
        /// </summary>
        private Rectangle rect;

        /// <summary>
        /// Gets or sets the blob rect.
        /// </summary>
        /// <value>The blob rect.</value>
        /// <remarks></remarks>
        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> struct.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <param name="player">The player.</param>
        /// <remarks></remarks>
        public Tile(string letter, Player player)
        {
            this.letter = letter;
            this.points = TileBag.Points(letter);
            this.player = player;
            this.isPlaced = false;
            this.isHighlight = false;
            this.rect = new Rectangle(0, 0, 0, 0);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Letter;
        }
    }
}