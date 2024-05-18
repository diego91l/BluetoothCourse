using Shiny.BluetoothLE;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Shiny;
using BluetoothCourse.Bluetooth;
using BluetoothCourse.Scan;

namespace BluetoothCourse;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();


        MainPage = new NavigationPage(serviceProvider.GetRequiredService<Views.Loggin.Loggin>());
    }
}



