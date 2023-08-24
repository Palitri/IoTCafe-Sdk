using OpenIoT.Lib.Board.Api;
using OpenIoT.Lib.Board.Protocol;
using OpenIoT.Lib.Board.Protocol.Events;
using OpenIoT.Lib.Board.Transmission.Com;
using OpenIoT.Lib.Composite;
using OpenIoT.Lib.Tools.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Scanner
{


    public delegate void OnPortAvailable(object sender, BoardPortEventArgs args);
    public delegate void OnPortUnavailable(object sender, BoardPortEventArgs args);
    public delegate void OnBoardAvailable(object sender, BordInfoEventArgs args);
    public delegate void OnBoardUnavailable(object sender, BordInfoEventArgs args);


    public class BoardScanner : IDisposable
    {
        public OnPortAvailable OnPortAvailable;
        public OnPortUnavailable OnPortUnavailable;
        public OnBoardAvailable OnBoardAvailable;
        public OnBoardUnavailable OnBoardUnavailable;

        private IEnumerable<string> availablePorts;
        private IEnumerable<BoardScanInfo> connectedBoards;

        private List<BoardScanInfo> foundBoards;

        private ContinuousThread? scanThread;
        
        public bool IsScanning { get { return (this.scanThread != null) && this.scanThread.IsRunning; } }

        public IEnumerable<BoardScanInfo> ConnectedBoards => this.connectedBoards.ToList();

        public TimeSpan ScanInterval { get; set; }

        public long Iteration { get; private set; }

        public BoardScanner()
        {
            this.Iteration = 0;

            this.scanThread = null;
            
            this.availablePorts = Array.Empty<string>();
            this.connectedBoards = new List<BoardScanInfo>();

            this.ScanInterval = TimeSpan.FromMilliseconds(2000);
        }

        public void ScanOnce()
        {
            if (this.scanThread != null)
                this.scanThread.Terminate(true);

            this.scanThread = new ContinuousThread(ScanTask);
            this.scanThread.Start(false);
        }

        public void ScanContinuously()
        {
            if (this.scanThread != null)
                this.scanThread.Wait();

            this.scanThread = new ContinuousThread(ScanTask);
            this.scanThread.Start();
        }

        public void StopScan(bool wait = true)
        {
            this.scanThread.Terminate(wait);
        }

        public void ScanForConnectedBoards(out IEnumerable<string> availablePorts, out IEnumerable<BoardScanInfo> connectedBoards, int timeoutMillis = 1000)
        {
            IEnumerable<string> allPorts = ComTransmissionChannel.GetAvailablePorts();
            List<OpenIoTBoard> candidateBoards = allPorts.Select(port => new OpenIoTBoard(new ComTransmissionChannel(port))).ToList();

            this.foundBoards = new List<BoardScanInfo>();
            List<string> freePorts = allPorts.ToList();

            foreach (OpenIoTBoard candidateBoard in candidateBoards)
            {
                try
                {
                    candidateBoard.EventHandlers.Add(new ScanBoardsEventHandler(this));
                    candidateBoard.Open();
                    candidateBoard.requestDeviceProperties(
                        OpenIoTProtocol.DevicePropertyId_BoardName,
                        OpenIoTProtocol.DevicePropertyId_Name,
                        OpenIoTProtocol.DevicePropertyId_ProjectName);

                    if (!this.availablePorts.Contains(candidateBoard.transmissionChannel.Name))
                        this.OnPortAvailable?.Invoke(this, new BoardPortEventArgs(candidateBoard.transmissionChannel.Name, this.Iteration != 0));
                }
                catch (Exception ex)
                {
                    freePorts.Remove(candidateBoard.transmissionChannel.Name);
                }
            }

            while (timeoutMillis > 0 && (this.foundBoards.Count < freePorts.Count))
            {
                Thread.Sleep(100);
                timeoutMillis -= 100;
            }

            foreach (OpenIoTBoard candidateBoard in candidateBoards)
            {
                candidateBoard.Close();
            }

            availablePorts = allPorts;
            connectedBoards = this.foundBoards;

            this.Iteration++;
        }

        internal void onSearchDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
            OpenIoTBoard board = (OpenIoTBoard)sender;
            BoardScanInfo boardInfo = new BoardScanInfo(board.transmissionChannel.Name, properties);

            // Close transmission channel, so that eventually in the OnBoardAvailable handler, a new cannel can be opened
            board.transmissionChannel.Close();
            
            this.foundBoards.Add(boardInfo);

            if (!this.connectedBoards.Contains(boardInfo))
                this.OnBoardAvailable?.Invoke(this, new BordInfoEventArgs(boardInfo));
        }

        public void ScanPorts()
        {
            IEnumerable<string> availablePorts;
            IEnumerable<BoardScanInfo> connectedBoards;

            this.ScanForConnectedBoards(out availablePorts, out connectedBoards, 1000);
            this.availablePorts.Except(availablePorts).ToList().ForEach(p => this.OnPortUnavailable?.Invoke(this, new BoardPortEventArgs(p, this.Iteration != 0)));
            this.connectedBoards.Except(connectedBoards).ToList().ForEach(b => this.OnBoardUnavailable?.Invoke(this, new BordInfoEventArgs(b)));

            this.availablePorts = availablePorts;
            this.connectedBoards = connectedBoards;
        }

        private bool ScanTask()
        {
            try
            {
                this.ScanPorts();

                this.Iteration++;

                Thread.Sleep((int)Math.Max(this.ScanInterval.TotalMilliseconds, 100));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Dictionary<string, BoardScanInfo> GetPorts(int timeoutMillis = 1000)
        {
            Dictionary<string, BoardScanInfo> result = new Dictionary<string, BoardScanInfo>();

            IEnumerable<string> availablePorts;
            IEnumerable<BoardScanInfo> connectedBoards;
            this.ScanForConnectedBoards(out availablePorts, out connectedBoards, 1000);

            availablePorts.ToList().ForEach(p => result[p] = null);
            connectedBoards.ToList().ForEach(b => result[b.Port] = b);

            return result;
        }

        public void Dispose()
        {
            this.StopScan();
        }
    }

    internal class ScanBoardsEventHandler : OpenIoTProtocolEventsHandler
    {
        private BoardScanner scanner;

        public ScanBoardsEventHandler(BoardScanner scanner)
        {
            this.scanner = scanner;
        }

        public override void onDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
            this.scanner.onSearchDevicePropertiesReceived(sender, properties);
        }
    }
}
