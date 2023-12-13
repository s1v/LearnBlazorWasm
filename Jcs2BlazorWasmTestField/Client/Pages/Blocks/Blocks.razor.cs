using System.Drawing;
using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using Jcs2BlazorWasmTestField.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Runtime.ConstrainedExecution;

namespace Jcs2BlazorWasmTestField.Client.Pages.Blocks
{
    public partial class Blocks
    {
        private const int square = 20; // ブロックスのマス数

        protected Color[,] board = new Color[square, square]; // ボード

        public string showInfo = ""; // 情報表示用
        public string showPlayerIngo = ""; // プレイヤー情報用

        public Player[] players = new Player[4]; // 参加者

        public Peace targetPeace = new(""); // 選択中のピース

        //  手番情報
        public int turn = 0; // 手番
        public Color turnColor = Color.Black; // 手番の色
        public Color turnLightColor = Color.Black; // 手番の薄い色

        public List<Cell> changedCell = new(); // 色を薄く変更したセル

        /// <summary>
        /// 表示時に発生するイベント
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetInit();
            InfoUpdate();
        }

        /// <summary>
        /// 表示情報を更新します
        /// </summary>
        private void InfoUpdate()
        {
            showInfo = "現在の手番は" + players[turn].Name + "です。";
            showPlayerIngo = "";
            for (int num = 0; num < players.Length; num++){
                showPlayerIngo += players[num].Info;
            }
        }

        /// <summary>
        /// ゲーム開始前に初期設定を行います
        /// </summary>
        private void SetInit()
        {
            // 全マスを白で初期化
            for (int i = 0; i < square; i++)
            {
                for (int j = 0; j < square; j++)
                {
                    board[i, j] = Color.White;
                }
            }

            // 各プレイヤーの初期設定
            players[0] = new Player("player1", Color.Red);
            players[1] = new Player("player2", Color.Blue);
            players[2] = new Player("player3", Color.Yellow);
            players[3] = new Player("player4", Color.Green);

            // 手番を初期化
            turn = 0;
            turnColor = players[turn].Color;
            turnLightColor = BrightColor(turnColor, 0.5);
            // 対象ピースを初期化
            targetPeace = new("");
        }

        /// <summary>
        /// 対象セルの色を変更します
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void ColorChange(int row, int col)
        {
            if (targetPeace.Name is not null && targetPeace.Name != "" && CheckPut(row, col, targetPeace))
            {
                // 対象セルを塗り替え
                foreach (Cell cell in targetPeace.Cells)
                {
                    board[row + cell.Row, col + cell.Col] = turnColor;
                }
                Console.WriteLine($"Row:{row}, Col:{col}");

                // 更新対象のインデックスを取得
                int idx = players[turn].Peaces.FindIndex(peace => peace.Name == targetPeace.Name);
                // 持ちゴマの配置情報を更新
                Peace peace = players[turn].Peaces[idx];
                peace.IsPlaced = true;
                players[turn].Peaces[idx] = peace;

                // 手番を交代
                TurnChange();
            }
        }

        /// <summary>
        /// マウスを被せた時のイベント
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private void OnMouseOver(int row, int col)
        {
            try
            {
                if (targetPeace.Name is not null && targetPeace.Name != "")
                {
                    Color drawingColor;
                    if (CheckPut(row, col, targetPeace))
                    {
                        drawingColor = turnColor;
                    }
                    else
                    {
                        drawingColor = turnLightColor;
                    }
                    // 対象セルを塗り替え
                    foreach (Cell cell in targetPeace.Cells)
                    {
                        if (board[row + cell.Row, col + cell.Col] == Color.White)
                        {
                            board[row + cell.Row, col + cell.Col] = drawingColor;
                            changedCell.Add(new Cell(row + cell.Row, col + cell.Col));
                        }
                    }
                }
            }
            catch
            {
                ClearChangedCell();
                Console.WriteLine($"OnMouseOver row:{row} col:{col}");
            }
        }

        /// <summary>
        /// マウスを離した時のイベント
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private void OnMouseOut()
        {
            ClearChangedCell();
        }

        /// <summary>
        /// 一時的に変更していた色を白に戻す
        /// </summary>
        public void ClearChangedCell()
        {
            foreach (Cell cell in changedCell)
            {
                board[cell.Row, cell.Col] = Color.White;
            }
            changedCell.Clear();
        }

        /// <summary>
        /// 対象セルにピースを向きを変えずに配置可能かチェックします
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="j">列</param>
        /// <param name="peace">チェック対象のピース</param>
        /// <returns>true:配置可能 false:配置不可</returns>
        private bool CheckPut(int i, int j, Peace peace)
        {
            try
            {
                OnMouseOut();

                bool checkPut = false;
                foreach(Cell cell in peace.Cells)
                {
                    // セルの行列を保持
                    int row = i + cell.Row;
                    int col = j + cell.Col;

                    // 各セルの違反チェック
                    // 対象セルが塗られていないかチェック
                    if (board[row, col] != Color.White &&
                        board[row, col] != turnColor &&
                        board[row, col] != turnLightColor)
                    {
                        return false;
                    }

                    // 対象セルの上下左右に同色が塗られていないかチェック
                    // 上
                    if (WithinRange(row - 1) &&
                        board[row - 1, col] == turnColor)
                    {
                        return false;
                    }
                    // 下
                    if (WithinRange(row + 1) &&
                        board[row + 1, col] == turnColor)
                    {
                        return false;
                    }
                    // 左
                    if (WithinRange(col - 1) &&
                        board[row, col - 1] == turnColor)
                    {
                        return false;
                    }
                    // 右
                    if (WithinRange(col + 1) && 
                        board[row, col + 1] == turnColor)
                    {
                        return false;
                    }

                    // ピース全体が条件を満たしているかチェック
                    if(checkPut)
                    {
                        // すでに条件を満たしている場合は次のセルの違反チェックに進む
                        continue;
                    }
                    // 初手かどうか
                    if (players[turn].RestPeaces.Count == 21)
                    {
                        // 初手の場合、四隅のいずれかを塗るかチェック
                        if ((row == 0 && col == 0) ||
                            (row == 0 && col == square - 1) ||
                            (row == square - 1 && col == 0) ||
                            (row == square - 1 && col == square - 1))
                        {
                            checkPut = true;
                        }
                    }
                    else
                    {
                        // 初手以外の場合、対象セルの斜めに同色が塗ってあるかチェック
                        if ((WithinRange(row - 1) && WithinRange(col - 1) &&
                            board[row - 1, col - 1] == turnColor) ||
                            (WithinRange(row - 1) && WithinRange(col + 1) &&
                            board[row - 1, col + 1] == turnColor) ||
                            (WithinRange(row + 1) && WithinRange(col - 1) &&
                            board[row + 1, col - 1] == turnColor) ||
                            (WithinRange(row + 1) && WithinRange(col + 1) &&
                            board[row + 1, col + 1] == turnColor))
                        {
                            checkPut = true;
                        }
                    }
                }

                return checkPut;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 対象セルにピースを配置可能かチェックします
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="j">列</param>
        /// <param name="peace">チェック対象のピース</param>
        /// <returns>true:配置可能 false:配置不可</returns>
        private bool CheckPutAllDirection(int i, int j, Peace peace)
        {
            try
            {
                Peace originalPeace = peace;

                for (int k = 0; k < peace.Cells.Count; k++)
                {
                    // 座標を移動してチェック
                    originalPeace = originalPeace.MovePoint(k);

                    for(int l = 0; l < 2; l++)
                    {
                        // 左右反転してチェック
                        originalPeace = originalPeace.Inversion();
                        
                        for (int m = 0; m < 4; m++)
                        {
                            // 右90度回転してチェック
                            originalPeace = originalPeace.RotationRight();

                            if (CheckPut(i, j, originalPeace))
                            {
                                return true;
                            }

                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 対象ピースを配置可能な場所があるかどうかをチェックします
        /// </summary>
        /// <param name="peace">チェック対象のピース（newで宣言）</param>
        /// <returns>true:配置可能 false:配置不可</returns>
        public bool ExistCanPutPlace(Peace peace)
        {
            List<Cell> cells = CanPutCellls();

            foreach (Cell cell in cells)
            {
                if ((board[cell.Row, cell.Col] == Color.White || board[cell.Row, cell.Col] == turnLightColor) &&
                CheckPutAllDirection(cell.Row, cell.Col, peace))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 手番を変更します
        /// </summary>
        private void TurnChange()
        {
            if (turn == players.Length - 1)
            {
                turn = 0;
            }
            else
            {
                turn += 1;
            }

            turnColor = players[turn].Color;
            turnLightColor = BrightColor(turnColor, 0.5);

            targetPeace = new Peace("");
            changedCell.Clear();

            // 表示情報更新
            InfoUpdate();
        }

        /// <summary>
        /// コンテナの要素選択時のイベント
        /// </summary>
        /// <param name="name"></param>
        private void PeaceSelected(Peace peace)
        {
            // 選択中ピース名を更新
            targetPeace = peace;
            Console.WriteLine($"Peace:{peace.Name}");
        }

        /// <summary>
        /// 数値が配列の範囲内かどうかを判定します
        /// </summary>
        /// <param name="index">判定対象の数値</param>
        /// <returns>true:範囲内 false:範囲外</returns>
        static private bool WithinRange(int index)
        {
            return 0 <= index && index < square;
        }

        /// <summary>
        /// ダブルクリック時のイベント
        /// </summary>
        private void OnDblClick(Peace peace)
        {
            // ダブルクリックしただけではtargetPeaceが設定されないため指定
            targetPeace = peace;

            // 対象セルを右回転させる
            int idx = players[turn].Peaces.FindIndex(peace => peace.Name == targetPeace.Name);
            players[turn].Peaces[idx] = targetPeace.RotationRight();
        }

        /// <summary>
        ///  右クリック時のイベント
        /// </summary>
        /// <param name="peace"></param>
        private void OnHandleRightClick(Peace peace)
        {
            // 右クリックしただけではtargetPeaceが設定されないため指定
            targetPeace = peace;

            // 右クリック時の処理を記述
            // 対象セルを反転させる
            int idx = players[turn].Peaces.FindIndex(peace => peace.Name == targetPeace.Name);
            players[turn].Peaces[idx] = targetPeace.Inversion();
        }

        /// <summary>
        /// 対象ピースとして選ばれている場合、文字色を赤くする
        /// </summary>
        /// <param name="peace"></param>
        /// <returns></returns>
        public string GetColorStyle(Peace peace)
        {
            return peace.Name == targetPeace.Name ? "color: red;" : "color: black;";
        }

        /// <summary>
        /// ピースの背景色を設定する
        /// </summary>
        /// <param name="peace">配置可能な場合：プレイヤー色　配置不可な場合：グレー</param>
        public void SetPeaceColor(Peace peace)
        {
            players[turn].Peaces.Where(item => item.Name == peace.Name).First().PeaceColor = ExistCanPutPlace(peace) ? turnColor : Color.Gray;
            Console.WriteLine($"Peace.Color:{peace.PeaceColor}");
        }

        /// <summary>
        /// ピースを配置可能な可能性があるマスのリストを作成
        /// </summary>
        /// <returns></returns>
        public List<Cell> CanPutCellls()
        {
            List<Cell> cells = new();

            // 1マスのピースでチェック
            Peace peace = new("a");

            for(int i = 0; i < square; i++){
                for(int j = 0; j < square; j++)
                {
                    if(CheckPut(i, j, peace)){
                        cells.Add(new Cell(i, j));
                    }
                }
            }

            return cells;
        }

        /// <summary>
        /// 色を薄くします。
        /// </summary>
        /// <param name="color"></param>
        /// <param name="lightenAmount">0から1を指定。1に近いほど薄くなります。</param>
        /// <returns></returns>
        public static Color BrightColor(Color color, double lightenAmount)
        {
            return Color.FromArgb(
                (int)((1 - lightenAmount) * color.R + lightenAmount * 255),
                (int)((1 - lightenAmount) * color.G + lightenAmount * 255),
                (int)((1 - lightenAmount) * color.B + lightenAmount * 255)
            );
        }
    }

}
