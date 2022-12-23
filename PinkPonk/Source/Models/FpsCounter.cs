using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PinkPonk.Source.Models
{
    public class FpsCounter
    {
        private readonly Timer _timer;

        private int counter = 0;

        public delegate void FpsCountHandler(int fpsCount);

        public event FpsCountHandler OnFpsCounted;

        public FpsCounter()
        {
            this._timer = new Timer()
            {
                AutoReset = true,
                Enabled = false,
                Interval = 1000,
            };
            this._timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.OnFpsCounted?.Invoke(counter);
            this.counter = 0;
            this._timer.Stop();
            this._timer.Start();
        }

        public void Run()
        {
            if (!this._timer.Enabled)
                this._timer.Start();
        }

        public void Iterate() => this.counter++;
    }
}
