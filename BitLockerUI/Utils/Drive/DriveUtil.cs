using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitLockerUI.Utils
{
    class DriveUtil
    {
        public List<DriveEtt> GetDriveList()
        {
            var driveList = new List<DriveEtt>();
            var drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                var d = new DriveEtt();
                d.Number = drive.Name;
                d.IsReady=drive.IsReady;
                if (drive.IsReady)
                {
                    d.Name = drive.VolumeLabel;
                    d.TotalSize = drive.TotalSize;
                    d.FreeSize = drive.AvailableFreeSpace;
                }
                driveList.Add(d);
            }
            return driveList;
        }
    }
}
