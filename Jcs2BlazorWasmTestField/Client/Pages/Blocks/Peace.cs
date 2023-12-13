using System.Drawing;

namespace Jcs2BlazorWasmTestField.Client.Pages.Blocks
{
    /// <summary>
    /// コマ
    /// </summary>
    public class Peace
    {
        /// <summary>
        /// ピース名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ピースを構成するセル
        /// </summary>
        public List<Cell> Cells { get; set; }

        /// <summary>
        /// 割付済み
        /// </summary>
        public bool IsPlaced { get; set; }

        public Color PeaceColor { get; set; }

        /// <summary>
        /// セル行の最大値
        /// </summary>
        public int MaxRow { get => Cells.Max(cell => cell.Row); }

        /// <summary>
        /// セル行の最小値
        /// </summary>
        public int MinRow { get => Cells.Min(cell => cell.Row); }

        /// <summary>
        /// セル列の最大値
        /// </summary>
        public int MaxCol { get => Cells.Max(cell => cell.Col); }

        /// <summary>
        /// セル列の最小値
        /// </summary>
        public int MinCol { get => Cells.Min(cell => cell.Col); }

        /// <summary>
        /// nameに応じてピースを構成する
        /// </summary>
        /// <param name="name">構成するピースの種類</param>
        public Peace(string name)
        {
            this.Name = name;
            this.IsPlaced = false;
            Cells = new List<Cell>();
            switch (name)
            {
                case "a":
                    // ■
                    Cells.Add(new Cell(0, 0));
                    break;

                case "b":
                    // ■
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "c":
                    // □
                    // ■
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "d":
                    // □
                    // ■□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, 1));
                    break;

                case "e":
                    // □
                    // ■
                    // □
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(2, 0));
                    break;

                case "f":
                    // 　□
                    // 　■
                    // □□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, -1));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "g":
                    // □
                    // ■□
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "h":
                    // ■□
                    // □□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(1, 1));
                    break;

                case "i":
                    // □■
                    // 　□□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(0, -1));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(1, 1));
                    break;

                case "j":
                    // □
                    // □
                    // ■
                    // □
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-2, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(2, 0));
                    break;

                case "k":
                    // 　□
                    // 　■
                    // 　□
                    // □□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(2, -1));
                    Cells.Add(new Cell(2, 0));
                    break;

                case "l":
                    // 　□
                    // 　□
                    // □■
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-2, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, -1));
                    Cells.Add(new Cell(1, -1));
                    break;

                case "m":
                    // 　□
                    // □■
                    // □□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, -1));
                    Cells.Add(new Cell(1, -1));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "n":
                    // □□
                    // 　■
                    // □□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, -1));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, -1));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "o":
                    // □
                    // ■□
                    // □
                    // □
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(2, 0));
                    break;

                case "p":
                    // 　□
                    // 　■
                    // □□□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(1, -1));
                    Cells.Add(new Cell(1, 0));
                    Cells.Add(new Cell(1, 1));
                    break;

                case "q":
                    // □
                    // □
                    // ■□□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-2, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(0, 2));
                    break;

                case "r":
                    // □□
                    // 　■□
                    // 　　□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, -1));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 1));
                    break;

                case "s":
                    // □
                    // □■□
                    // 　　□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, -1));
                    Cells.Add(new Cell(0, -1));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 1));
                    break;

                case "t":
                    // □
                    // □■□
                    // 　□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, -1));
                    Cells.Add(new Cell(0, -1));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 0));
                    break;

                case "u":
                    // 　□
                    // □■□
                    // 　□
                    Cells.Add(new Cell(0, 0));
                    Cells.Add(new Cell(-1, 0));
                    Cells.Add(new Cell(0, -1));
                    Cells.Add(new Cell(0, 1));
                    Cells.Add(new Cell(1, 0));
                    break;
            }
        }


        /// <summary>
        /// 右90度回転
        /// </summary>
        public Peace RotationRight()
        {
            // セル構成の組み換え
            List<Cell> cells = new();
            for (int len = 0; len < Cells.Count; len++)
            {
                cells.Add(new Cell(Cells[len].Col, Cells[len].Row * -1));
            }
            this.Cells = cells;
            return this;
        }

        /// <summary>
        /// 左右反転
        /// </summary>
        public Peace Inversion()
        {
            // セル構成の組み換え
            List<Cell> cells = new();
            for (int len = 0; len < Cells.Count; len++)
            {
                cells.Add(new Cell(Cells[len].Row, Cells[len].Col * -1));
            }
            this.Cells = cells;
            return this;
        }

        /// <summary>
        /// 座標移動
        /// </summary>
        public Peace MovePoint(int num = 0)
        {
            // セル構成の組み換え
            List<Cell> myCells = new Peace(this.Name).Cells; // 初期配置を設定
            List<Cell> cells = new();
            Cell startCell = myCells[num];
            foreach (Cell cell in myCells)
            {
                cells.Add(new Cell(cell.Row - startCell.Row, cell.Col - startCell.Col));
            }
            this.Cells = cells;
            return this;
        }
    }
}
