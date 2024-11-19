using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace 電車でGO
{
    sealed class 電車でGODriver : System.IDisposable
    {
        public static bool logDebug = false;

        private struct DeviceInformation
        {
            public string path;
            public LibUsbDotNet.UsbDevice usbDevice;
            public 新幹線専用コントローライージィ controller;
            public 電車でGO新幹線Device inputDevice;
        };
        private Dictionary<string, DeviceInformation> _addedInputDevicesByPath = new Dictionary<string, DeviceInformation>();

        public void Update()
        {
            CheckForAdded();
            CheckForRemoved();

            ProcessEventQueue();
        }

        private void CheckForAdded()
        {
            var usbDevice = DeviceFinder.FindDevice(DeviceFinder.SupportedDevice.TCPP20011);

            if (usbDevice == null)
            {
                return;
            }

            var path = usbDevice.DevicePath;
            if (_addedInputDevicesByPath.ContainsKey(path))
            {
                if (logDebug)
                {
                    UnityEngine.Debug.Log($"電車でGO: Device already added: {path}");
                }

                return;
            }

            var description = new InputDeviceDescription
            {
                interfaceName = "電車でGO新幹線",
                deviceClass = "Gamepad",
                product = "電車でGO新幹線",
                // capabilities = "{\"channel\":" + channel + "}"
                // version,
                // serial,
                manufacturer = "Taito Corp."
            };

            var inputDevice = (電車でGO新幹線Device)InputSystem.AddDevice(description);

            if (inputDevice == null)
            {
                UnityEngine.Debug.LogError($"電車でGO: Failed to add device: {path}");

                return;
            }

            var controller = new 新幹線専用コントローライージィ(usbDevice);
            inputDevice.SetController(controller);

            var deviceInformation = new DeviceInformation
            {
                path = path,
                usbDevice = usbDevice,
                controller = controller,
                inputDevice = inputDevice
            };
            _addedInputDevicesByPath.Add(path, deviceInformation);

            if (logDebug)
            {
                UnityEngine.Debug.Log($"電車でGO: added usb device Layout: {inputDevice}");
            }
        }

        private void CheckForRemoved()
        {
            var index = 0;

            while (index < _addedInputDevicesByPath.Count)
            {
                var pair = _addedInputDevicesByPath.ElementAt(index);

                var path = pair.Key;
                var deviceInformation = pair.Value;

                var usbDevice = deviceInformation.usbDevice;

                if (usbDevice == null)
                {
                    _addedInputDevicesByPath.Remove(path);

                }
                else if (!usbDevice.UsbRegistryInfo.IsAlive)
                {
                    var inputDevice = deviceInformation.inputDevice;

                    Remove(pair);

                    if (logDebug)
                    {
                        UnityEngine.Debug.Log($"電車でGO: removed usb device Layout: {inputDevice}");
                    }
                }
                else
                {
                    index++;
                }
            }
        }

        private void ProcessEventQueue()
        {
            foreach (var pair in _addedInputDevicesByPath)
            {
                var deviceInformation = pair.Value;
                deviceInformation.inputDevice.ProcessEventQueue();
            }
        }

        public void Dispose()
        {
            while (_addedInputDevicesByPath.Count > 0)
            {
                var pair = _addedInputDevicesByPath.ElementAt(0);

                Remove(pair);
            }
        }

        private void Remove(KeyValuePair<string, DeviceInformation> pair)
        {
            var path = pair.Key;
            var deviceInformation = pair.Value;

            var usbDevice = deviceInformation.usbDevice;
            usbDevice.Close();

            var inputDevice = deviceInformation.inputDevice;
            InputSystem.RemoveDevice(inputDevice);

            _addedInputDevicesByPath.Remove(path);
        }
    }
}