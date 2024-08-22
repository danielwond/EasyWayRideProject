using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Services.Services.EncryptionService
{
    public interface IEncryptionService
    {
        public string DecryptString(string type, string cipherText);
        public string EncryptString(string type, string plainText);

    }
}
