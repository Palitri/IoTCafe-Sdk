using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Scanner
{
    public class BoardPortEventArgs : EventArgs
    {
        public string Port { get; private set; }
        public bool JustAppeared { get; private set; }

        public BoardPortEventArgs(string port, bool justAppeared)
        {
            this.Port = port;
            this.JustAppeared = justAppeared;
        }
    }

    public class BordInfoEventArgs : EventArgs
    {
        public BoardScanInfo Info { get; private set; }

        public BordInfoEventArgs(BoardScanInfo info)
        {
            this.Info = info;
        }
    }
}
