using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCourse.Bluetooth
{
    public class BluetoothPermissions
    {
        public BluetoothPermissions() { }

        public async Task CheckBluetoothPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Bluetooth>();

                if (status != PermissionStatus.Granted)
                {
                    throw new Exception("Bluetooth permissions are required");
                }
            }
        }


        public async Task CheckLocationPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    throw new Exception("Location permissions are required");
                }
            }

        }
    }
}
