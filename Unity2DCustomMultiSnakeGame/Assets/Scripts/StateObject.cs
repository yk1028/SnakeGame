using System.Net.Sockets;
using System.Text;

// State object for reading client data asynchronously
namespace Com.Yk1028.SnakeGame
{
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Client socket.
        public Socket workSocket = null;
    }
}
