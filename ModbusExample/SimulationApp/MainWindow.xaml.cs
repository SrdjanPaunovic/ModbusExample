using ModbusExample;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulationApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<KeyValuePair<long, float>> obsCollection = new ObservableCollection<KeyValuePair<long, float>>();
		private long timer = 0;
		private const long DURATION = 500;
		private CustomModbusClient modbusClient;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			modbusClient = new CustomModbusClient("localhost", 502);
			PopulateSimulationData();

			Task task = new Task(StartSimulation);
			task.Start();
			//StartSimulation();
		}

		private void StartSimulation()
		{
			foreach(KeyValuePair<long,float> pair in obsCollection)
			{
				modbusClient.WriteInRegisters(3,pair.Value);
				Thread.Sleep(1000);
			}
		}

		private void PopulateSimulationData()
		{
			for (int i = 0; i < DURATION; i++)
			{
				float value = (float)SimulationFunction(i);
				ObsCollection.Add(new KeyValuePair<long, float>(i, value));
			}
		}

		private double SimulationFunction(int x)
		{
			return Math.Sin(x) * (Math.Sin(x) - 1) + Math.Cos(x) / 2 + 5;
		}

		public ObservableCollection<KeyValuePair<long, float>> ObsCollection
		{
			get
			{
				return obsCollection;
			}

			set
			{
				obsCollection = value;
			}
		}
	}
}
