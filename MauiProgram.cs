using Microsoft.Extensions.Logging;
using Shiny;
using Shiny.Infrastructure;

namespace BluetoothCourse;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddShiny();
		builder.Services.AddBluetooth();
		builder.Services.AddViews();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static void AddShiny(this IServiceCollection services)
	{
		services.AddShinyCoreServices();
		services.AddBluetoothLE();
	}

	private static void AddBluetooth(this IServiceCollection services)
	{
		services.AddSingleton<Bluetooth.BluetoothScanner>();
        services.AddSingleton<Bluetooth.BluetoothPermissions>();
    }
	private static void AddViews(this IServiceCollection services)
	{
		services.AddSingleton<BluetoothCourse.Scan.ScanResults>();
        services.AddTransient<BluetoothCourse.Views.Loggin.Loggin>();
    }
}
