using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_Trainer.Services
{
    public class MiBandService
    {
        private readonly IBluetoothLE _bluetoothLE;
        private readonly IAdapter _adapter;
        private IDevice _connectedDevice;

        public MiBandService()
        {
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
        }
        public async Task ConnectToBandAsync(string deviceName)
        {
            try
            {
                _adapter.DeviceDiscovered += OnDeviceDiscovered;
                await _adapter.StartScanningForDevicesAsync();
                var device = _adapter.DiscoveredDevices.FirstOrDefault(d => d.Name == deviceName);
                if (device != null)
                {
                    await _adapter.ConnectToDeviceAsync(device);
                    _connectedDevice = device;
                    Console.WriteLine("Connected to Mi Band 8");
                }
                else
                {
                    Console.WriteLine("Device not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to device: {ex.Message}");
            }
            finally
            {
                _adapter.DeviceDiscovered -= OnDeviceDiscovered;
            }
        }
        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            Console.WriteLine($"Discovered device: {e.Device.Name}");
        }
        public async Task ReadDataAsync(Guid serviceUuid, Guid characteristicUuid, (byte[] data, int resultCode) bytes)
        {
            if (_connectedDevice == null)
            {
                Console.WriteLine("No device connected");
                return;
            }
            try
            {
                var service = await _connectedDevice.GetServiceAsync(serviceUuid);
                if (service != null)
                {
                    var characteristic = await service.GetCharacteristicAsync(characteristicUuid);
                    if (characteristic != null)
                    {
                        var bytess = (await characteristic.ReadAsync()).data;
                        Console.WriteLine($"Data read: {BitConverter.ToString(bytess)}");
                    }
                    else
                    {
                        Console.WriteLine("Characteristic not found");
                    }
                }
                else
                {
                    Console.WriteLine("Service not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data: {ex.Message}");
            }
        }
        public async Task DisconnectAsync()
        {
            if (_connectedDevice != null)
            {
                await _adapter.DisconnectDeviceAsync(_connectedDevice);
                _connectedDevice = null;
                Console.WriteLine("Disconnected from Mi Band 8");
            }
        }
    }
}
