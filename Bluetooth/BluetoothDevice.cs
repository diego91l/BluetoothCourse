using System;
using Shiny.BluetoothLE;

namespace BluetoothCourse.Bluetooth
{
    public class BluetoothDevice
    {
        public IPeripheral _device;
        public List<BleCharacteristicInfo> _characteristics = new List<BleCharacteristicInfo>();

        public BluetoothDevice(IPeripheral device)
        {
            _device = device;
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
                    _characteristics.Add(characteristic);
                }
            }
        }

        public void Disconnect()
        {
            _characteristics.Clear();
            _device.CancelConnection();
        }

        public async Task<byte[]> Read(Guid characteristicUuid)
        {
            var characteristic = _characteristics.First(a => a.Uuid == characteristicUuid.ToString());

            if (!characteristic.CanRead())
            {
                return null;
            }

            var result = await _device.ReadCharacteristicAsync(characteristic);
            return result.Data;
        }

        public async Task<bool> Write(Guid characteristicUuid, byte[] data)
        {
            var characteristic = _characteristics.First(a => a.Uuid == characteristicUuid.ToString());

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
