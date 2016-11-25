using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;

namespace RobotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var usbDevices = GetUSBDevices();

            //foreach (var usbDevice in usbDevices)
            //{
            //    Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
            //        usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            //}
            getNameFlashisAlive();
            Console.Read();
        }

        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
           
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }

        /// <summary>
        /// проверяем флешку на файл RobotKernel.bin и его содержимое 
        /// </summary>
        /// <returns></returns>
        static int? getNameFlashisAlive()
        {
            string fileNameKernel = "RobotKernel.bin";

            try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        foreach (string dir in dirs)
                        {
                            if (Path.GetFileName(dir) == fileNameKernel)
                            {
                                string line;
                                StreamReader file = new StreamReader(dir);

                                while ((line = file.ReadLine()) != null)
                                {
                                    return Convert.ToInt32(line);
                                }
                                file.Close();
                            }
                        }                   
                    }               
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }

   

    class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }
}



