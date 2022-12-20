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

        public int LeftScore { get; set; }

        public int RightScore { get; set; }

        public Winner IsGameFinished
        {
            get
            {
                if (LeftScore >= MaximumGameScore)
                    return Winner.LeftSide;

                if (RightScore >= MaximumGameScore)
                    return Winner.RightSide;

                return Winner.Nobody;
            }
        }

        public void SetScore(Winner winner)
        {
            switch (winner)
            {
                case Winner.LeftSide:
                    this.LeftScore++;
                    break;
                case Winner.RightSide:
                    this.RightScore++;
                    break;
                default:
                    break;
            }
        }
    }
}
