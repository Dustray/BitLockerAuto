using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitLockerUI.Useless
{
    public class DriveEtt
    {
        /// <summary>
        /// 驱动器号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 驱动器名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 总空间
        /// </summary>
        public long TotalSize { get; set; }
        /// <summary>
        /// 可用空间
        /// </summary>
        public long FreeSize { get; set; }
        /// <summary>
        /// 驱动器可用
        /// </summary>
        public bool IsReady { get; set; }
    }
}
