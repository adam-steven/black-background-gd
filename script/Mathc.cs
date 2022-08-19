using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System
{
    //
    // Summary:
    //     Provides constants and static methods for trigonometric, logarithmic, and other
    //     common mathematical functions.To browse the .NET Framework source code for this
    //     type, see the Reference Source.
    public static class Mathc
    {
        //
        // Summary:
        //     Limits a given 32-bit signed integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Limit(int valMin, int val, int valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given decimal between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static decimal Limit(decimal valMin, decimal val, decimal valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given double-precision floating-point between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax. If valMin, val, and/or valMax,
        //     are equal to System.Double.NaN, System.Double.NaN is returned.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static double Limit(double valMin, double val, double valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given single-precision floating-point between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax. If valMin, val, and/or valMax,
        //     are equal to System.Single.NaN, System.Single.NaN is returned.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static float Limit(float valMin, float val, float valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 64-bit unsigned integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ulong Limit(ulong valMin, ulong val, ulong valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 64-bit signed integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Limit(long valMin, long val, long valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 32-bit unsigned integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static uint Limit(uint valMin, uint val, uint valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 16-bit unsigned integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ushort Limit(ushort valMin, ushort val, ushort valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 16-bit signed integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static short Limit(short valMin, short val, short valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 8-bit signed integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static sbyte Limit(sbyte valMin, sbyte val, sbyte valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Limits a given 8-bit unsigned integer between to values.
        //
        // Parameters:
        //   valMin:
        //     The limit minimum.
        //
        //   val:
        //     The number to compare/crop.
        // 
        //   valMax:
        //     The limit Maximum.
        //
        // Returns:
        //     Parameter val unchanged or val cropped to never exceed valMin or valMax.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static byte Limit(byte valMin, byte val, byte valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
    }
}