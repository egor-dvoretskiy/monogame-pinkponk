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

        public delegate void GameFinishHandler();

        public event GameFinishHandler OnGameFinish;

        public int LeftScore { get; set; }

        public int RightScore { get; set; }

        public Winner Winner { get; private set; }

        public bool SetScore(Winner winner)
        {
            switch (winner)
            {
                case Winner.LeftSide:
                    this.RightScore++;

                    if (this.RightScore >= MaximumGameScore)
                    {
                        this.Winner = Winner.RightSide;
                        this.OnGameFinish?.Invoke();
                    }

                    return true;
                case Winner.RightSide:
                    this.LeftScore++;

                    if (this.LeftScore >= MaximumGameScore)
                    {
                        this.Winner = Winner.LeftSide;
                        this.OnGameFinish?.Invoke();
                    }

                    return true;
                default:
                    break;
            }

            return false;
        }
    }
}
