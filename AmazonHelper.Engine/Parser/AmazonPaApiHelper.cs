using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace AmazonHelper.Engine.Parser
{
    [Serializable]
    [XmlRoot("Merchant")]
    public class Merchant
    {
        public string Name { get; set; }
    }

    [Serializable]
    [XmlRoot("Offer")]
    public class Offer
    {
        public Merchant Merchant { get; set; }
    }

    [Serializable]
    public class Item
    {
        [XmlArrayItem("Offer", typeof(Offer))]
        public List<Offer> Offers { get; set; }
    }

    [Serializable]
    [XmlRoot("ItemLookupResponse")]
    public class ItemLookupResponse
    {
        [XmlArrayItem("Item", typeof(Item))]
        public List<Item> Items { get; set; }
    }

    public class AmazonServiceException : Exception
    {
        public AmazonServiceException(string message):base(message)
        {

        }
    }

    public class AmazonCredentials
    {
        public string AssociateTag { get; set; }

        public string AWSAccessKey { get; set; }

        public string AWSSecretKey { get; set; }
    }

    public class AmazonPaApiHelper
    {
        private const string baseServiceUrl = "webservices.amazon.com";
        private byte[] strSecret;
        private const string REQUEST_URI = "/onca/xml";
        private const string REQUEST_METHOD = "GET";
        private HMAC signer;

        private AmazonCredentials _amazonCredentials;

        public AmazonPaApiHelper(AmazonCredentials amazonCredentials)
        {
            _amazonCredentials = amazonCredentials;
            strSecret = Encoding.UTF8.GetBytes(_amazonCredentials.AWSSecretKey);
            signer = new HMACSHA256(strSecret);
        }

        public async Task<T> GetAmazonItemInfoAsync<T>(string asin)
            where T : class
        {
            try
            {
                var requestParams = new Dictionary<string, string>();
                requestParams["Service"] = "AWSECommerceService";
                requestParams["Operation"] = "ItemLookup";
                requestParams["ItemId"] = asin;
                requestParams["ItemType"] = "ASIN";
                requestParams["ResponseGroup"] = "Offers,OfferFull";

                string url = GenerateRequestUrl(requestParams);
                string response = await MakeRequestAsync(url);

                var doc = new XmlDocument();
                doc.LoadXml(response);

                return doc as T;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateRequestUrl(IDictionary<string, string> requestParams)
        {
            var pc = new ParamComparer();
            var sortedMap = new SortedDictionary<string, string>(requestParams, pc);

            sortedMap["AWSAccessKeyId"] = _amazonCredentials.AWSAccessKey;
            sortedMap["AssociateTag"] = _amazonCredentials.AssociateTag;
            sortedMap["Timestamp"] = GetTimestamp();

            string canonicalQS = this.ConstructCanonicalQueryString(sortedMap);

            var builder = new StringBuilder();
            builder.Append(REQUEST_METHOD)
                .Append("\n")
                .Append(baseServiceUrl)
                .Append("\n")
                .Append(REQUEST_URI)
                .Append("\n")
                .Append(canonicalQS);

            string stringToSign = builder.ToString();
            byte[] toSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] sigBytes = signer.ComputeHash(toSign);
            string signature = Convert.ToBase64String(sigBytes);

            var qsBuilder = new StringBuilder();
            qsBuilder.Append("http://")
                .Append(baseServiceUrl)
                .Append(REQUEST_URI)
                .Append("?")
                .Append(canonicalQS)
                .Append("&Signature=")
                .Append(PercentEncodeRfc3986(signature));

            return qsBuilder.ToString();
        }

        private async Task<string> MakeRequestAsync(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();

                    //var doc = new XmlDocument();
                    //doc.Load(content);

                    //var errorMessageNodes = doc.GetElementsByTagName("Message");
                    //if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                    //{
                    //    string message = errorMessageNodes.Item(0).InnerText;
                    //    throw new AmazonServiceException(message);
                    //}

                    return content;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SignRequest()
        {

        }

        private void SignString(string stringToSign)
        {

        }

        private string GetTimestamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private string PercentEncodeRfc3986(string str)
        {
            str = HttpUtility.UrlEncode(str, Encoding.UTF8);
            str = str.Replace("'", "%27").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21").Replace("%7e", "~").Replace("+", "%20");

            var sbuilder = new StringBuilder(str);
            for (int i = 0; i < sbuilder.Length; i++)
            {
                if (sbuilder[i] == '%')
                {
                    if (char.IsLetter(sbuilder[i + 1]) || char.IsLetter(sbuilder[i + 2]))
                    {
                        sbuilder[i + 1] = char.ToUpper(sbuilder[i + 1]);
                        sbuilder[i + 2] = char.ToUpper(sbuilder[i + 2]);
                    }
                }
            }

            return sbuilder.ToString();
        }

        // Consttuct the canonical query string from the sorted parameter map.
        private string ConstructCanonicalQueryString(SortedDictionary<string, string> sortedParamMap)
        {
            if (!sortedParamMap.Any())
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var kvp in sortedParamMap)
            {
                builder.Append(PercentEncodeRfc3986(kvp.Key));
                builder.Append("=");
                builder.Append(PercentEncodeRfc3986(kvp.Value));
                builder.Append("&");
            }

            return builder.ToString().Substring(0, builder.ToString().Length - 1);
        }

        private class ParamComparer : IComparer<string>
        {
            public int Compare(string p1, string p2)
            {
                return string.CompareOrdinal(p1, p2);
            }
        }
    }
}
