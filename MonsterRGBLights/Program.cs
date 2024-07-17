/* Monster Smart Lighting "Smart RGB+IC Color Flow Light Bar Pair" UDP Client
 * Written by Andrew Haskell (c) 2024
 * 
 * Specifically, testing was done with a MNW7-2024-ICM (FCCID: 2AHAS-MLW71003)
 * 
 * The device tested uses the Ayla Network Communications IoT libraries to function.
 * For further digging, the devices do expose an HTTP server, however the API for it is unknown.
 * Hitting /status.json yields more response details than other endpoints, and a http 405 when hitting it with non GET methods.
 * There may be more robust communciation that can be done with these cheap devices
*/

using MonsterRGBLights;
using System.Net;
using System.Net.Sockets;

// Get from the app under the device info as "DSN" This is baked into the ESP chip inside the light but not printed anywhere on it
// There doesn't seem to be a way to discover this as the device is silent until a message with the DSN is sent.
// Presumably this is exchanged during initial device setup over bluetooth,
// and is then stored in the user's online "Monster Smart Lighting" account (Which is different from a "Monster Smart" account)
const string DEVICE_SERIAL_NUMBER = "AC000W032989949";

// You can find this in the device app, or via many other means.
// I recommend giving setting DHCP reservation for the device so its address does'nt change.
const string IP_ADDRESS = "10.100.0.27";

// This is the port the device listens on
const int UDP_PORT = 64000;

// After the device has been powered on, it won't respond to control messages until a discovery message is sent.
// Perhaps there is a timeout for commands where a fresh discovery message must be sent? Will update if that looks like the case.
byte[] discovery = StructHelper.Serialize(new Messages.DeviceDiscoveryMsg()
{
    Type = Messages.MessageTypes.DEVICE_DISCOVERY,
    SessionToken = (ushort)Random.Shared.Next(ushort.MaxValue),
    MulticastPort = 0,
    DeviceSerialNumber = DEVICE_SERIAL_NUMBER
});

byte[] onRed = StructHelper.Serialize(new Messages.DeviceControlMsg()
{
    Type = Messages.MessageTypes.DEVICE_CONTROL_SET,
    Power = Messages.DevicePower.ON,
    Mode = Messages.DeviceMode.UDP_COLOR,
    Zone = 0,
    ColorRed = 255,
    ColorGreen = 0,
    ColorBlue = 0,
    ColorBrightness = 100,
    DeviceSerialNumber = DEVICE_SERIAL_NUMBER,
    IsEphemeral = 0
});
byte[] off = StructHelper.Serialize(new Messages.DeviceControlMsg()
{
    Type = Messages.MessageTypes.DEVICE_CONTROL_SET,
    Power = Messages.DevicePower.OFF,
    Mode = Messages.DeviceMode.UDP_COLOR,
    Zone = 0,
    ColorRed = 0,
    ColorGreen = 0,
    ColorBlue = 0,
    ColorBrightness = 100,
    DeviceSerialNumber = DEVICE_SERIAL_NUMBER,
    IsEphemeral = 0
});

IPEndPoint endpoint = IPEndPoint.Parse($"{IP_ADDRESS}:{UDP_PORT}");

using UdpClient udp = new();
udp.Send(discovery, discovery.Length, endpoint);

while (true)
{
    udp.Send(off, off.Length, endpoint);
    Thread.Sleep(1000);
    udp.Send(onRed, onRed.Length, endpoint);
    Thread.Sleep(1000);
}