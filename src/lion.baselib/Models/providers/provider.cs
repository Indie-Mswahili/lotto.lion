using LottoLion.BaseLib.Models.Entity;
using Microsoft.Extensions.Configuration;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Cryption;
using System;

namespace LottoLion.BaseLib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LTCX : IDisposable
    {
        private static CLogger __clogger = new CLogger();
        private static CConfig __cconfig = new CConfig();

        private static CCryption __cryptor = null;
        private static CCryption Cryptor
        {
            get
            {
                if (__cryptor == null)
                {
                    var _key = __cconfig.ConfigRoot["aes_key"];
                    var _iv = __cconfig.ConfigRoot["aes_iv"];

                    __cryptor = new CCryption(_key, _iv);
                }

                return __cryptor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LottoLionContext GetNewContext(string connection_name = "DefaultConnection", bool enctyprion = true)
        {
            var _connection_string = __cconfig.ConfigRoot.GetConnectionString(connection_name);
            if (enctyprion == true)
                _connection_string = Cryptor.ChiperToPlain(_connection_string);

            return new LottoLionContext(_connection_string);
        }

        public void Dispose()
        {
        }
    }
}