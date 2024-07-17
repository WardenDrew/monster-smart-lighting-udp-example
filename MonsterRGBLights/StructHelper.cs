// Sourced from https://www.dotnetcodegeeks.com/2015/09/converting-between-structs-and-byte-arrays.html
using System.Runtime.InteropServices;

namespace MonsterRGBLights;
internal static class StructHelper
{
    public static byte[] Serialize<T>(T s)
    where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        byte[] bytes = new byte[size];
        nint pointer = Marshal.AllocHGlobal(size);

        try
        {
            Marshal.StructureToPtr(s, pointer, true);
            Marshal.Copy(pointer, bytes, 0, size);
        }
        finally
        {
            Marshal.FreeHGlobal(pointer);
        }

        return bytes;
    }

    public static T? Deserialize<T>(byte[] array)
        where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        nint pointer = Marshal.AllocHGlobal(size);

        T? resultStruct = null;
        try
        {
            Marshal.Copy(array, 0, pointer, size);
            resultStruct = (T?)Marshal.PtrToStructure(pointer, typeof(T));
        }
        finally
        {
            Marshal.FreeHGlobal(pointer);
        }

        return resultStruct;
    }
}
