using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCourse.Bluetooth
{
    public class BluetoothScanner
    {
        private readonly IBleManager _bluetoothManager;
        private readonly BluetoothPermissions _bluetoothPermissions;

        public BluetoothScanner(IBleManager bluetoothManager, 
            BluetoothPermissions bluetoothPermissions)
        {
            _bluetoothManager = bluetoothManager;
            _bluetoothPermissions = bluetoothPermissions;
        }

        public bool IsScanning => _bluetoothManager.IsScanning;

        public ObservableCollection<BluetoothDevice> Devices { get; private set; } = new ObservableCollection<BluetoothDevice>();

        private IDisposable _scanner;

        public void StartScanning()
        {
            try
            {
                var platform = Microsoft.Maui.Devices.DeviceInfo.Platform;
                
                if (platform == Microsoft.Maui.Devices.DevicePlatform.iOS)
                {
                    _bluetoothPermissions.CheckLocationPermissions().GetAwaiter().GetResult();
                }

                if (platform == Microsoft.Maui.Devices.DevicePlatform.Android)
                {
                    _bluetoothPermissions.CheckLocationPermissions().GetAwaiter().GetResult();
                }
            }
            catch (Exception)
            {

                //TODO: handle exceptions
            }

            if (IsScanning)
                StopScanning();

            _scanner = _bluetoothManager.Scan().Subscribe(_result =>
            {
                if (Devices.Any(x => x.Uuid == _result.Peripheral.Uuid))
                    return;

                Devices.Add(new BluetoothDevice(_result));
            });
        }

        public void StopScanning()
        {
            if (!IsScanning)
                return;

            _scanner?.Dispose();
        }

    }
}
