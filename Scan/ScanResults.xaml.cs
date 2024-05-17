using System;
using Shiny;
using Shiny.BluetoothLE;
using System.Collections;
using BluetoothCourse.Extensions;
using BluetoothCourse.Bluetooth;

namespace BluetoothCourse.Scan;

public partial class ScanResults : ContentPage
{
    private readonly BluetoothScanner _bluetoothScanner;

    public ScanResults(BluetoothScanner bluetoothScanner)
    {
        _bluetoothScanner = bluetoothScanner;

        InitializeComponent();
        this.BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        lvDevices.ItemsSource = _bluetoothScanner.Devices;
        _bluetoothScanner.StartScanning();

        lvDevices.ItemSelected += (s, e) => {
            lvDevices.SelectedItem = null;
        };
    }



    private void btnConectar_Clicked(object sender, EventArgs e)
    {

    }
}
