using System;

namespace Redirection
{
    public class Redirector
    {
        public static OffsetBackup DetourFunction(IntPtr ptrOriginal, IntPtr ptrModified)
        {
            OffsetBackup Backup = new OffsetBackup(ptrOriginal);
            
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    unsafe
                    {
                        byte* ptrFrom = (byte*) ptrOriginal.ToPointer();

                        *(ptrFrom + 1) = 0xBB;
                        *((uint*) (ptrFrom + 2)) = (uint) ptrModified.ToInt32();
                        *(ptrFrom + 11) = 0xFF;
                        *(ptrFrom + 12) = 0xE3;
                    }
                    break;
                case sizeof(Int64):
                    unsafe
                    {
                        byte* ptrFrom = (byte*) ptrOriginal.ToPointer();

                        *ptrFrom = 0x49;
                        *(ptrFrom + 1) = 0xBB;
                        *((ulong*) (ptrFrom + 2)) = (ulong) ptrModified.ToInt64();
                        *(ptrFrom + 10) = 0x41;
                        *(ptrFrom + 11) = 0xFF;
                        *(ptrFrom + 12) = 0xE3;
                    }
                    break;
            }

            return Backup;
        }

        public static bool RevertDetour(OffsetBackup backup)
        {
            try
            {
                unsafe
                {
                    byte* ptrOriginal = (byte*)backup.Method.ToPointer();

                    *ptrOriginal = backup.A;
                    *(ptrOriginal + 1) = backup.B;
                    *(ptrOriginal + 10) = backup.C;
                    *(ptrOriginal + 11) = backup.D;
                    *(ptrOriginal + 12) = backup.E;
                    if (IntPtr.Size == sizeof(Int32))
                        *((uint*)(ptrOriginal + 2)) = backup.F32;
                    else
                        *((ulong*)(ptrOriginal + 2)) = backup.F64;
                }

                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}