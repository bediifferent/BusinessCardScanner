﻿using System;
using System.IO;
using BusinessCardScanner.Droid;
using BusinessCardScanner.Services.Interfaces;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInfoService))]
namespace BusinessCardScanner.Droid
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public byte[] GetFileStream(MediaFile file)
        {
            if (file != null)
            {
                // provide read access to the file
                using (var fileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read))
                {
                    // Create a byte array of file stream length
                    var imageData = new byte[fileStream.Length];

                    //Read block of bytes from stream into the byte array
                    fileStream.Read(imageData, 0, Convert.ToInt32(fileStream.Length));

                    //Close the File Stream
                    fileStream.Close();

                    return imageData;
                }
            }
            return null;
        }
        public string CreateCommonDatabase()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.SharedSQLiteDBName);
        }

    }
}