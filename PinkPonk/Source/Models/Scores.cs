using PinkPonk.Source.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkPonk.Source.Models
{
    public class Scores
    {
        public const int MaximumGameScore = 4;

        public delegate void WinnerHandler(Winner winner);

        public event WinnerHandler OnGameFinish;

        public int LeftScore { get; set; }

        public int RightScore { get; set; }

        public bool SetScore(Winner winner)
        {
            switch (winner)
            {
                case Winner.LeftSide:
                    this.LeftScore++;

                    if (this.LeftScore >= MaximumGameScore)
                        this.OnGameFinish?.Invoke(Winner.LeftSide);

                    return true;
                case Winner.RightSide:
                    this.RightScore++;

                    if (this.RightScore >= MaximumGameScore)
                        this.OnGameFinish?.Invoke(Winner.RightSide);

                    return true;
                default:
                    break;
            }

            return false;
        }
    }
}
