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
 
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static short Limit(short valMin, short val, short valMax) {
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ushort Limit(ushort valMin, ushort val, ushort valMax) {
            return Math.Max(valMin, Math.Min(val, valMax));
        }


        //
        // Summary:
        //     Returns the smaller of two 32-bit signed integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 32-bit signed integers to compare.
        //
        //   val2:
        //     The second of two 32-bit signed integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Limit(short valMin, short val, short valMax){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two decimal numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two decimal numbers to compare.
        //
        //   val2:
        //     The second of two decimal numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static decimal Limit(decimal val1, decimal val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two double-precision floating-point numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two double-precision floating-point numbers to compare.
        //
        //   val2:
        //     The second of two double-precision floating-point numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller. If val1, val2, or both val1 and
        //     val2 are equal to System.Double.NaN, System.Double.NaN is returned.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static double Limit(double val1, double val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two single-precision floating-point numbers.
        //
        // Parameters:
        //   val1:
        //     The first of two single-precision floating-point numbers to compare.
        //
        //   val2:
        //     The second of two single-precision floating-point numbers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller. If val1, val2, or both val1 and
        //     val2 are equal to System.Single.NaN, System.Single.NaN is returned.
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static float Limit(float val1, float val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 64-bit unsigned integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 64-bit unsigned integers to compare.
        //
        //   val2:
        //     The second of two 64-bit unsigned integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [CLSCompliant(false)]
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ulong Limit(ulong val1, ulong val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 64-bit signed integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 64-bit signed integers to compare.
        //
        //   val2:
        //     The second of two 64-bit signed integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Limit(long val1, long val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 32-bit unsigned integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 32-bit unsigned integers to compare.
        //
        //   val2:
        //     The second of two 32-bit unsigned integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [CLSCompliant(false)]
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static uint Limit(uint val1, uint val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 16-bit unsigned integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 16-bit unsigned integers to compare.
        //
        //   val2:
        //     The second of two 16-bit unsigned integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [CLSCompliant(false)]
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ushort Limit(ushort val1, ushort val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 16-bit signed integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 16-bit signed integers to compare.
        //
        //   val2:
        //     The second of two 16-bit signed integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static short Limit(short val1, short val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 8-bit signed integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 8-bit signed integers to compare.
        //
        //   val2:
        //     The second of two 8-bit signed integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [CLSCompliant(false)]
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static sbyte Limit(sbyte val1, sbyte val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
        //
        // Summary:
        //     Returns the smaller of two 8-bit unsigned integers.
        //
        // Parameters:
        //   val1:
        //     The first of two 8-bit unsigned integers to compare.
        //
        //   val2:
        //     The second of two 8-bit unsigned integers to compare.
        //
        // Returns:
        //     Parameter val1 or val2, whichever is smaller.
        [NonVersionableAttribute]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static byte Limit(byte val1, byte val2){
            return Math.Max(valMin, Math.Min(val, valMax));
        }
    }
}