 
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Scada.MDSCore.Exceptions;

namespace Scada.MDSCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class GeneralHelper
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="endPoint"> 远程主机</param>
        /// <param name="timeoutMs">链接超时</param>
        /// <returns>返回Sccket</returns>
        public static Socket ConnectToServerWithTimeout(EndPoint endPoint, int timeoutMs)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                socket.Blocking = false;
                socket.Connect(endPoint);
                socket.Blocking = true;
                return socket;
            }
            catch (SocketException socketException)
            {
                if (socketException.ErrorCode != 10035)
                {
                    socket.Close();
                    throw;
                }

                if (!socket.Poll(timeoutMs * 1000, SelectMode.SelectWrite))
                {
                    socket.Close();
                    throw new MDSException("The host failed to connect. Timeout occured.");
                }

                socket.Blocking = true;
                return socket;
            }
        }

        public static byte[] SerializeObject(object obj)
        {
            var memoryStream = new MemoryStream();
            new BinaryFormatter().Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        public static object DeserializeObject(byte[] bytesOfObject)
        {
            return new BinaryFormatter().Deserialize(new MemoryStream(bytesOfObject) { Position = 0 });
        }

        /// <summary>
        /// Gets the current directory of executing assembly.
        /// </summary>
        /// <returns>Directory path</returns>
        public static string GetCurrentDirectory()
        {
            string directory;
            try
            {
                directory = (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.FullName;
            }
            catch (Exception)
            {
                directory = Directory.GetCurrentDirectory();
            }

            var directorySeparatorChar = Path.DirectorySeparatorChar.ToString();
            if (!directory.EndsWith(directorySeparatorChar))
            {
                directory += directorySeparatorChar;
            }

            return directory;
        }
    }
}
