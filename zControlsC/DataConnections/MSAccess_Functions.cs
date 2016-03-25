using System;
using System.Collections.Generic;
using System.Text;

namespace zControlsC.DataConnection
{
    public static class MSAccess_Functions
    {

        public static string RunAccessMacro(string db, string MacroName, string msAccessEXE)
        {
            try
            {
                System.Diagnostics.Process p;
                db = (char)34 + db + (char)34;
                MacroName = (char)34 + MacroName + (char)34;
                msAccessEXE = (char)34 + msAccessEXE + (char)34;
                p = System.Diagnostics.Process.Start(msAccessEXE, db + " /x " + MacroName);
                p.WaitForExit();
                p.Dispose();
                p = null;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


    }

}
