using System;
using System.Runtime.InteropServices;

namespace RDPSUTControlAgent
{
    public static class PointerManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        private static readonly uint SPI_SETCURSORS = 87;
        private static readonly uint SPIF_UPDATEINIFILE = 0x1;
        private static readonly uint SPIF_SENDCHANGE = 0x2;

        public static void EffectChange()
        {
            SystemParametersInfo(SPI_SETCURSORS, 0, null, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }
    }

    public enum SPIF
    {
        None = 0x00,
        UPDATEINIFILE = 0x01,
        SENDCHANGE = 0x02
    }
}
