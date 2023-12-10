using System.Drawing;

namespace Jcs2BlazorWasmTestField.Client.Pages.Blocks
{
    /// <summary>
    /// 参加者
    /// </summary>
    public class Player
    {
        /// <summary>
        /// プレイヤー名
        /// </summary>
        public string Name { get; set; }

        public string Info { get => 
                "　" + this.Name + 
                "：残数:" + this.RestPeaces.Count.ToString() + 
                "(" + this.RestPeaces.Sum(peace => peace.Cells.Count) + ")"; }

        /// <summary>
        /// プレイヤー色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// ピースリスト
        /// </summary>
        public List<Peace> Peaces { get; set; }

        /// <summary>
        /// 未割付のピースリスト
        /// </summary>
        public List<Peace> RestPeaces { get => Peaces.Where(peace => peace.IsPlaced == false).ToList(); }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="name">プレイヤー名</param>
        /// <param name="color">プレイヤー色</param>
        public Player(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
            this.Peaces = new Peaces().PeaceList;
        }
    }
}
