// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System;
using System.Net.NetworkInformation;

namespace MACIdentifier
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(GetMacAddress());
            Console.ReadLine();
        }

        private static string GetMacAddress()
        {
            var macAddress = string.Empty;

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (macAddress == string.Empty)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                }
            }

            return macAddress;
        }
    }
}