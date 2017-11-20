using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ModbusExample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		//private ObservableCollection<KeyValuePair<long, short>> obsCollection = new ObservableCollection<KeyValuePair<long, short>>();
		//public ObservableCollection<KeyValuePair<long, short>> ObsCollection
		//{
		//	get
		//	{
		//		return obsCollection;
		//	}

		//	set
		//	{
		//		obsCollection = value;
		//	}
		//}

		private ObservableCollection<KeyValuePair<long, float>> obsCollection = new ObservableCollection<KeyValuePair<long, float>>();
		private CustomModbusClient modbusClient;
		private long timer = 0;

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

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			modbusClient = new CustomModbusClient("localhost", 502);
			//   modbusClient.WriteInRegisters(5, s);
			Task t = new Task(Loop);
			t.Start();

		}

		public void Loop()
		{
			while (true)
			{
				CollectData();
				timer++;
				Thread.Sleep(1000); // sleep 1s
			}
		}

		public void CollectData()
		{
			//short value = modbusClient.ReadShortFromRegisters(5);

			float value = modbusClient.ReadFloatFromRegisters(3);

			this.Dispatcher.Invoke(() =>
			{
				ObsCollection.Add(new KeyValuePair<long, float>(timer, value));
				while(ObsCollection.Count > 15)
				{
					ObsCollection.RemoveAt(0);
				}
			});

			// fixedQueue.Enqueue(new KeyValuePair<long, short>(timer,sValue));
			// OnPropertyChanged(nameof(FixedQueue));
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

	}
}
