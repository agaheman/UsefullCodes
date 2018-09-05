using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgahClassLibrary
{
    public class FileAndFolder
    {
        public List<DrivesInfoViewModel> getDrivesInfos()
        {
           return DriveInfo.GetDrives().Select(
               d => new DrivesInfoViewModel()
               {
                   Name = d.Name,
                   VolumeLabel = d.VolumeLabel,
                   AvailableFreeSpace = UtitlityCodes.ByteToHumanReadableSize(d.AvailableFreeSpace),
                   TotalSize = UtitlityCodes.ByteToHumanReadableSize(d.TotalSize),
                   DriveType = d.DriveType.ToString(),
                   DriveFormat = d.DriveFormat

               }).ToList();

        }


        public class DrivesInfoViewModel
        {
            public string Name;
            public string VolumeLabel;
            public string TotalSize;
            public string AvailableFreeSpace;
            public string DriveFormat;
            public string DriveType;
        }

    }
}
