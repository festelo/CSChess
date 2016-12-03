using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class User
    {
        public class ID
        {
            private byte group;
            private byte id;
            //public int Group { get group; }
            //public int Id { get id; }

            public ID(byte[] bytes)
            {
                group = bytes[0];
                id = bytes[1];
            }

            override
            public string ToString()
            {
                return group.ToString() + id.ToString();
            }
            public byte[] ToBytes()
            {
                return new byte[] { group, id };
            }
        }
        //Статистика
        //Еще что-то
        public ID id { get; set; }
    }
    public class Internet
    {
        public static class Exceptions
        {
            public class UserLoginError : Exception
            {
                public UserLoginError(string message) : base(message) { }
            }
            public class UserAlreadyJoined : UserLoginError
            {
                public UserAlreadyJoined(string message) : base(message) { }
            }
            public class UserAlreadyRegistred : UserLoginError
            {
                public UserAlreadyRegistred(string message) : base(message) { }
            }
        }
        Socket socket;
        public Internet() { }
        private bool isLogged;
        public bool IsLogged { get { return isLogged; } }
        private bool isConnected;
        public bool IsConnected { get { return isConnected; } }
        public void Connect(string IP)
        {
            IPAddress ipAddr = IPAddress.Parse(IP);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11223);

            socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipEndPoint);
            isConnected = true;
        }

        private byte[] DataToArray(byte type, string login, string password)
        {
            List<byte> data = new List<byte>();
            data.Add(type);
            data.AddRange(Encoding.UTF8.GetBytes(login + "/" + password));
            return data.ToArray();
        }

        public User Login(string login, string password)
        {
            socket.Send(DataToArray(1, login, password));
            byte[] receive = new byte[64];
            int lenght = socket.Receive(receive);
            if (receive[0] == 1)
                throw new Exceptions.UserAlreadyJoined("User has already joined");
            else if (receive[0] == 2)
                throw new Exceptions.UserLoginError("Password or/and login incorrect");
            else
            {
                //string rec = Encoding.UTF8.GetString(receive, 1, lenght);
                //string[] recData = rec.Split('/');  //TODO
                isLogged = true;
                return new User();
            }
        }

        public void Register(string login, string password)
        {
            socket.Send(DataToArray(2, login, password));
            byte[] receive = new byte[1];
            socket.Receive(receive);
            if (receive[0] == 2)
                throw new Exceptions.UserAlreadyRegistred("User has already registered");
            isLogged = true;
            return;
        }

        public User.ID GetID(string login, string password)
        {
            socket.Send(new byte[] {0});
            byte[] Receive = new byte[2];
            socket.Receive(Receive);
            return new User.ID(Receive);
        }
    }
}
