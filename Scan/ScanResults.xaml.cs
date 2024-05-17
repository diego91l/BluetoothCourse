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
    private BluetoothDevice _selectedDevice;

    public ScanResults(BluetoothScanner bluetoothScanner)
    {
        _bluetoothScanner = bluetoothScanner;

        InitializeComponent();
        this.BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        pickerDevices.ItemsSource = _bluetoothScanner.Devices;
        _bluetoothScanner.StartScanning();

        pickerDevices.SelectedIndexChanged += (s, e) =>
        {
            if (pickerDevices.SelectedIndex != -1)
            {
                _selectedDevice = (BluetoothDevice)pickerDevices.SelectedItem;
            }
        };
    }



    private void btnConectar_Clicked(object sender, EventArgs e)
    {
        if (_selectedDevice != null)
        {
            _selectedDevice.Connect();
            DisplayAlert("Connection", $"Connecting to {_selectedDevice.Name}", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please select a device to connect", "OK");
        }

    }

    private void btnDesconectar_Clicked(object sender, EventArgs e)
    {
        if (_selectedDevice != null)
        {
            _selectedDevice.Disconnect();
            DisplayAlert("Disconnection", $"Disconnected from {_selectedDevice.Name}", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please select a device to disconnect", "OK");
        }

    }
}
