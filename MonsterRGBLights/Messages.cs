﻿// Sourced from testing and from .NET libraries included with windows app version of Monster Smart Lighting
using System.Runtime.InteropServices;

namespace MonsterRGBLights;
internal class Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceDiscoveryMsg
    {
        public MessageTypes Type;
        public byte _emptyByte1; // empty padding, unknown why this is needed
        public ushort SessionToken; // Random "token". Also called Machine ID but is randomly generated by the original app
        public ushort MulticastPort; // Multicast port the app is supposedly listening on. The lights tested do not use this and it can be left 0
        [MarshalAs((UnmanagedType)23, SizeConst = 16)]
        public string DeviceSerialNumber;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceControlMsg
    {
        public MessageTypes Type;
        public DevicePower Power;
        public DeviceMode Mode;
        public byte ColorRed; // 0-255 decimal
        public byte ColorGreen; // 0-255 decimal
        public byte ColorBlue; // 0-255 decimal
        public byte ColorBrightness; //0-100 decimal. Higher values appear to wrap around
        public byte Zone; // 0-3 decimal. Only relevant when in Razer mode AFAICT
        [MarshalAs((UnmanagedType)23, SizeConst = 16)]
        public string DeviceSerialNumber;
        public byte IsEphemeral; // Part of the decompiled networking library's spec for the device, however is never set
    }

    public enum MessageTypes : byte
    {
        CONTROL_RESPONSE,
        DEVICE_DISCOVERY,
        DEVICE_CONTROL_SET,
        MULTICAST_COLOR_SET,
        ERROR,
    }

    public enum DevicePower : byte
    {
        OFF,
        ON
    }

    public enum DeviceMode : byte
    {
        RAZER_CHROMA = 0x00,
        UDP_COLOR = 0x01,
        CLOUD_PROFILE = 0x0a
    }
}
