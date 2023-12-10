using System.Drawing;

namespace Jcs2BlazorWasmTestField.Client.Pages.Blocks
{
    /// <summary>
    /// 持ちゴマ
    /// </summary>
    public class Peaces
    {
        /// <summary>
        /// ピースリスト
        /// </summary>
        public List<Peace> PeaceList { get; set; }

        /// <summary>
        /// 全ピースを構成するインスタンス
        /// </summary>
        public Peaces()
        {
            this.PeaceList = new List<Peace>
            {
                new Peace("a"),
                new Peace("b"),
                new Peace("c"),
                new Peace("d"),
                new Peace("e"),
                new Peace("f"),
                new Peace("g"),
                new Peace("h"),
                new Peace("i"),
                new Peace("j"),
                new Peace("k"),
                new Peace("l"),
                new Peace("m"),
                new Peace("n"),
                new Peace("o"),
                new Peace("p"),
                new Peace("q"),
                new Peace("r"),
                new Peace("s"),
                new Peace("t"),
                new Peace("u")
            };
        }
    }
}
