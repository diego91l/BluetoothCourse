using System;
using System.Collections.ObjectModel;
using Shiny.BluetoothLE;

namespace BluetoothCourse.Bluetooth
{
    public class BluetoothDevice
    {
        public IPeripheral _device;
        public ObservableCollection<BleCharacteristicInfo> Characteristics = new ObservableCollection<BleCharacteristicInfo>();

        public string Name { get; private set; } = "Unknown";

        public int Rssi { get; private set; } = 0;  

        public string Uuid { get; private set; } = string.Empty;

        public string DisplayName => $"{Name} ({Uuid})";
        public BluetoothDevice(IPeripheral device)
        {
            _device = device;
            Name = _device != null ? _device.Name : "Unknown";
            Uuid = _device != null ? _device.Uuid.ToString() : string.Empty;
        }

        public BluetoothDevice(ScanResult scanResult)
        {
            _device = scanResult.Peripheral;
            Name = _device != null ? _device.Name : "Unknown";
            Uuid = _device != null ? _device.Uuid.ToString() : string.Empty;
            Rssi = scanResult.Rssi;
        }

        public async void Connect()
        {
            _device.WhenConnected().Subscribe(async _device => {
                await Discover();
            });

            await _device.ConnectAsync();
        }

        private async Task Discover()
        {
            var services = await _device.GetServicesAsync();

            foreach (var service in services)
            {
                var characteristics = await _device.GetCharacteristicsAsync(service.Uuid);

                foreach (var characteristic in characteristics)
                {
                    Characteristics.Add(characteristic);
                }
            }
        }

        public void Disconnect()
        {
            Characteristics.Clear();
            _device.CancelConnection();
        }

        public async Task<byte[]> Read(Guid characteristicUuid)
        {
            var characteristic = Characteristics.First(a => a.Uuid == characteristicUuid.ToString());

            if (!characteristic.CanRead())
            {
                return null;
            }

            var result = await _device.ReadCharacteristicAsync(characteristic);
            return result.Data;
        }

        public async Task<bool> Write(Guid characteristicUuid, byte[] data)
        {
            var characteristic = Characteristics.First(a => a.Uuid == characteristicUuid.ToString());

            if (!(characteristic.CanWrite() || characteristic.CanWriteWithoutResponse()))
            {
                return false;
            }

            bool wor = characteristic.CanWriteWithoutResponse();
            var result = await _device.WriteCharacteristicAsync(characteristic, data, wor);

            return (result.Event == BleCharacteristicEvent.Write ||
                    result.Event == BleCharacteristicEvent.WriteWithoutResponse);
        }
    }
}
