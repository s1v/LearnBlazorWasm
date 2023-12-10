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
            turnLightColor = Color.FromArgb(turnColor.A, turnColor.R / 2, turnColor.G / 2, turnColor.B / 2);
        }

        /// <summary>
        /// 対象セルの色を変更します
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void ColorChange(int row, int col)
        {
            if (targetPeace.Name is not null && targetPeace.Name != "" && CheckPut(row, col))
            {
                // 対象セルを塗り替え
                foreach (Cell cell in targetPeace.Cells)
                {
                    board[row + cell.Row, col + cell.Col] = players[turn].Color;
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
        /// 対象セルの色を薄く変更します
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private void OnMouseOver(int row, int col)
        {
            try
            {
                if (targetPeace.Name is not null && targetPeace.Name != "" && CheckPut(row, col))
                {
                    // 対象セルを塗り替え
                    foreach (Cell cell in targetPeace.Cells)
                    {
                        if (board[row + cell.Row, col + cell.Col] == Color.White)
                        {
                            board[row + cell.Row, col + cell.Col] = turnLightColor;
                            changedCell.Add(new Cell(row + cell.Row, col + cell.Col));
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine($"OnMouseOver row:{row} col:{col}");
            }
        }

        /// <summary>
        /// 対象セルの色を薄く変更します
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private void OnMouseOut()
        {
            foreach(Cell cell in changedCell)
            {
                board[cell.Row, cell.Col] = Color.White;
            }
            changedCell.Clear();
        }

        /// <summary>
        /// 対象セルにピースを配置可能かチェックします
        /// </summary>
        /// <returns>true:配置可能 false:配置不可</returns>
        private bool CheckPut(int i, int j)
        {
            try
            {
                bool checkPut = false;
                foreach(Cell cell in targetPeace.Cells)
                {
                    // セルの行列を保持
                    int row = i + cell.Row;
                    int col = j + cell.Col;

                    // 各セルの違反チェック
                    // 対象セルが塗られていないかチェック
                    if (board[row, col] != Color.White &&
                        board[row, col] != turnLightColor)
                    {
                        return false;
                    }

                    // 対象セルの上下左右に同色が塗られていないかチェック
                    // 上
                    if (WithinRange(row - 1) &&
                        board[row - 1, col] == players[turn].Color)
                    {
                        return false;
                    }
                    // 下
                    if (WithinRange(row + 1) &&
                        board[row + 1, col] == players[turn].Color)
                    {
                        return false;
                    }
                    // 左
                    if (WithinRange(col - 1) &&
                        board[row, col - 1] == players[turn].Color)
                    {
                        return false;
                    }
                    // 右
                    if (WithinRange(col + 1) && 
                        board[row, col + 1] == players[turn].Color)
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
                            board[row - 1, col - 1] == players[turn].Color) ||
                            (WithinRange(row - 1) && WithinRange(col + 1) &&
                            board[row - 1, col + 1] == players[turn].Color) ||
                            (WithinRange(row + 1) && WithinRange(col - 1) &&
                            board[row + 1, col - 1] == players[turn].Color) ||
                            (WithinRange(row + 1) && WithinRange(col + 1) &&
                            board[row + 1, col + 1] == players[turn].Color))
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
            turnLightColor = Color.FromArgb(turnColor.A, turnColor.R / 2, turnColor.G / 2, turnColor.B / 2);

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

        private void OnHandleRightClick(Peace peace)
        {
            // 右クリックしただけではtargetPeaceが設定されないため指定
            targetPeace = peace;

            // 右クリック時の処理を記述
            // 対象セルを反転させる
            int idx = players[turn].Peaces.FindIndex(peace => peace.Name == targetPeace.Name);
            players[turn].Peaces[idx] = targetPeace.Inversion();
        }
    }

}
