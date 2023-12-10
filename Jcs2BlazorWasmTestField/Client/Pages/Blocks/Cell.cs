namespace Jcs2BlazorWasmTestField.Client.Pages.Blocks
{
    /// <summary>
    /// セル
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        public Cell(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    }
}
