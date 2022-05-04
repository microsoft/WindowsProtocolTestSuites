using System;
using System.Runtime.InteropServices;

namespace RDPSUTControlAgent
{
    public static class PointerManager
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
        private static readonly uint SPI_SETCURSORS = 0x0057;
        private static readonly uint SPIF_UPDATEINIFILE = 0x01;
        private static readonly uint SPIF_SENDCHANGE = 0x02;

        public static void EffectChange()
        {
            SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }
    }

}
