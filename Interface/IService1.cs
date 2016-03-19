using System;
using System.ServiceModel;

namespace Delver.Interface
{
    [ServiceContract(Namespace = "WcfService1.Services", CallbackContract = typeof (IGameCallback),
        SessionMode = SessionMode.Required)]
    public interface IServerWithCallback
    {
        [OperationContract(IsOneWay = true)]
        void StartNewServer();

        [OperationContract(IsOneWay = true)]
        void SendCommand(int game, int player, string command);
    }

    [ServiceContract]
    public interface IGameCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendDataPacket(int player, string data);
    }

    public class ConsoleCallback : IGameCallback
    {
        public void SendDataPacket(int player, string json)
        {
            Console.WriteLine($"{player}: {json}");
        }
    }
}