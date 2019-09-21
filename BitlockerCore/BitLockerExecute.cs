using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitlockerCore
{
    public class BitLockerExecute : IDisposable
    {
        private const string LOCK = "manage-bde -lock ";//f:
        private const string UNLOCK = "manage-bde -unlock ";//f: -password 然后输入密码
        private string _driveLetter;
        public BitLockerExecute(string driveLetter)
        {
            _driveLetter = driveLetter;
        }
        public bool Lock()
        {
            var cmd = new CMDUtils();
            return cmd.Send(LOCK + $"{_driveLetter}:");
        }
        public bool Unlock(string recoveryPassword)
        {
            var cmd = new CMDUtils();
            return cmd.Send(UNLOCK + $"{_driveLetter}: -RecoveryPassword {recoveryPassword}");
        }
        public void Dispose()
        {

        }
    }
}
