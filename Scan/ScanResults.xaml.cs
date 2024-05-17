using System;
using Shiny;
using Shiny.BluetoothLE;
using System.Collections;
using BluetoothCourse.Extensions;

namespace BluetoothCourse.Scan;

public partial class ScanResults : ContentPage
{
    private readonly IBleManager _bleManager;

    public ObservableList<ScanResult> Results { get; } = new ObservableList<ScanResult>();

    public ScanResults(IBleManager bleManager)
    {
        _bleManager = bleManager;
        InitializeComponent();
        this.BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        try { Scan(); }
        catch { }

        lvDevices.ItemSelected += (s, e) => {
            lvDevices.SelectedItem = null;
        };
    }

    public void Scan()
    {
        if (!_bleManager.IsScanning)
        {
            _bleManager.StopScan();
        }

        Results.Clear();

        _bleManager.Scan()
            .Subscribe(_result => {
                System.Diagnostics.Debug.WriteLine($"Scanned for: {_result.Peripheral.Uuid.ToString()}");

                if (!Results.Any(a => a.Peripheral.Uuid.Equals(_result.Peripheral.Uuid)))
                {
                    Results.Add(_result);
                }
            });
    }

    private void btnConectar_Clicked(object sender, EventArgs e)
    {

    }
}
