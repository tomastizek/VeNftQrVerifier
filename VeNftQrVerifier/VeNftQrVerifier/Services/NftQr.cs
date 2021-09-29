using VEDriversLite.NFT;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace VeNftQrVerifier.Services
{

    public class NftQr
    {
        public string TxId { get; set; }
        public string Signature { get; set; }


        public NftQr()
        {

        }

        /// <summary>
        /// Validate if Qr code contains valid TxId
        /// </summary>
        /// <param name="QrInput"></param>
        /// <returns></returns>
        public static Task<bool> ValidateNftFromQr(string QrInput)
        {
            try
            {
                JsonConvert.DeserializeObject<NftQr>(QrInput);
                return Task.FromResult(true);
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine(ex);
                return Task.FromResult(false);
            }
        }

    }

  



}
