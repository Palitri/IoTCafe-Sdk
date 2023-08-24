namespace OpenIoT.Lib.Board.Models
{
    public class BoardPropertyFlags
    {
        public static int Read = 1 << 0;
        public static int Write = 1 << 1;
        public static int Subscribed = 1 << 7;
    }
}
