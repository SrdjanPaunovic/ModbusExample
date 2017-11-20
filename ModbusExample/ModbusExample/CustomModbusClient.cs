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
            ServerIpAddress = ipAddress;
            ServerPort = port;
            modbusClient = new ModbusClient(ipAddress, port);
        }


        public void WriteInRegisters<T>(int startingAddress, T value)
        {
            int[] values = ConvertingValueHandler.ConvertToRegister(value);
            modbusClient.Connect();
            modbusClient.WriteMultipleRegisters(startingAddress, values);
            modbusClient.Disconnect();
        }

        public float ReadFloatFromRegisters(int startingAddress)
        {
            modbusClient.Connect();
            int[] byteArray = modbusClient.ReadHoldingRegisters(startingAddress, 2);
            modbusClient.Disconnect();

            return ModbusClient.ConvertRegistersToFloat(byteArray);
        }

        public short ReadShortFromRegisters(int startingAddress)
        {
            modbusClient.Connect();
            int[] byteArray = modbusClient.ReadHoldingRegisters(startingAddress, 1);
            modbusClient.Disconnect();

            return Convert.ToInt16(byteArray[0]);
        }

        public string ServerIpAddress
        {
            get
            {
                return serverIpAddress;
            }

            set
            {
                serverIpAddress = value;
            }
        }

        public int ServerPort
        {
            get
            {
                return serverPort;
            }

            set
            {
                serverPort = value;
            }
        }


    }
}
