using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusExample
{
    public static class ConvertingValueHandler
    {

        private static Dictionary<Type, Func<object, int[]>> typeToFuncDictionary = new Dictionary<Type, Func<object, int[]>>
        {
            {typeof(int),   (value) => { return ModbusClient.ConvertIntToRegisters(Convert.ToInt32(value));} },
            {typeof(float), (value) => { return ModbusClient.ConvertFloatToRegisters(Convert.ToSingle(value));} },
            {typeof(short), (value) => { return new int[] { Convert.ToInt32(value)}; } }
        };

        public static int[] ConvertToRegister<T>(T value)
        {
            int[] retValues = typeToFuncDictionary[typeof(T)].Invoke(value);

            return retValues;
        }


    }
}
