using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

namespace ModbusExample
{
    public class CustomModbusClient
    {
        private ModbusClient modbusClient;
        private string serverIpAddress;
        private int serverPort;
        public CustomModbusClient(string ipAddress, int port)
        {
            serverIpAddress = ipAddress;
            serverPort = port;
            modbusClient = new ModbusClient(ipAddress, port);
        }

        public void WriteInRegisters<T>(int startingAddress, T value)
        {
            int[] values = ConvertToRegister(value);
            modbusClient.Connect();
            modbusClient.WriteMultipleRegisters(startingAddress, values);
            modbusClient.Disconnect();
        }

        private int[] ConvertToRegister<T>(T value)
        {
            int[] retValues = { };
            if (typeof(T) == typeof(float))
            {
                retValues = ModbusClient.ConvertFloatToRegisters(Convert.ToSingle(value));
            }

            return retValues;
        }

        public string ServerIpAddress { get => serverIpAddress; set => serverIpAddress = value; }
        public int Port { get => serverPort; set => serverPort = value; }
    }
}
