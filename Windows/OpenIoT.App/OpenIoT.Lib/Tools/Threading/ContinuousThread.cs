using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Threading
{
    public delegate bool ContinuousThreadAction();

    internal class ContinuousThread
    {
        private volatile bool loop;
        private volatile bool running;

        public volatile int interval;

        private readonly Thread thread;
        private readonly ContinuousThreadAction? action;

        public bool IsRunning => this.running;
        public bool IsClosing => this.running && !this.loop;

        public ContinuousThread(ContinuousThreadAction? action = null, int interval = 50)
        {
            this.thread = new Thread(this.Run);

            this.action = action;
            this.interval = interval;

            this.loop = false;
            this.running = false;
        }

        public void Start(bool continuous = true)
        {
            this.loop = continuous;

            this.thread.Start();
        }
        
        private void Run()
        {
            this.running = true;

            try
            {
                while (this.Action())
                {
                    if (!this.loop)
                        break;

                    Thread.Sleep(this.interval);
                }
            }
            catch
            {
            }
            finally
            {
                this.running = false;
            }
        }

        public void Terminate(bool wait)
        {
            this.loop = false;

            if (wait)
                this.Wait();
        }

        public void Wait()
        {
            while (this.running)
                try
                {
                    Thread.Sleep(this.interval);
                }
                catch (Exception)
                {
                }
        }

        public bool Action()
        {
            if (this.action == null)
                return false;

            return this.action.Invoke();
        }
    }

}
