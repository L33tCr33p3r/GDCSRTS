using Godot;
using System.Net;
using System.Net.Sockets;
using System.Text;

internal partial class Server : Node
{
	//private Level _level = new();
	//private List<Level> _players = new();
	//private TcpListener? _connectionListener;
	//private List<Socket> _clients = new();

	//// Called when the node enters the scene tree for the first time.
	//public override void _Ready()
	//{
	//	IPAddress localIp;
	//	using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
	//	{
	//		socket.Connect("8.8.8.8", 65530);
			
	//		IPEndPoint endPoint = (IPEndPoint) socket.LocalEndPoint!;
	//		localIp = endPoint.Address;
	//	}
	//	for (int i = 4503; i <= 4533; i++)
	//	{
	//		try
	//		{
	//			Console.WriteLine("Trying to open server on: " + localIp.ToString() + ":" + i.ToString());
	//			_connectionListener = new(localIp, i);
	//			break;
	//		}
	//		catch { }
	//	}
	//	if (_connectionListener == null)
	//	{
	//		throw new Exception();
	//	}

	//	_connectionListener.Start();
	//	client = _connectionListener.AcceptSocket();
	//}

	//// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//	byte[] message = Encoding.ASCII.GetBytes(Console.ReadLine()!);
	//	short length = (short)message.Length;
	//	byte[] data = new byte[message.Length + 3];
	//	data[0] = 0;
	//	(data[1], data[2]) = ((byte)(length & 0xff), (byte)(length >> 8));
	//	message.CopyTo(data, 3);
	//	//_level.Update();
	//	client.Send(message);
	//}
}
