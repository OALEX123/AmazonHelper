using System;
using System.Net.Http;
using System.Threading.Tasks;
using AmazonHelper.Common;
using HtmlAgilityPack;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Linq;

namespace AmazonHelper.Engine.Parser
{
    public enum ParsePriceResult
    {
        [EnumDisplayName("Error parsing amazon")]
        ParseError,
        [EnumDisplayName("Asin was not found")]
        AsinNotFound,
        [EnumDisplayName("Blocked by captcha")]
        Captcha,
        [EnumDisplayName("Buy price belongs to company")]
        BuyPriceBelongsToCompany,
        [EnumDisplayName("Buy price does not belong to company")]
        BuyPriceNotBelongsToCompany
    }

    public static class AmazonParser
    {
        public static async Task<ParsePriceResult> ParseBuyPriceAsync(string asin, string companyName)
        {
            try
            {
                var amazonPaApiHelper = new AmazonPaApiHelper(new AmazonCredentials
                {
                    AssociateTag = "aggregatorby-20",
                    AWSAccessKey = "AKIAIP42UQY6NP5Q4J6A",
                    AWSSecretKey = "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"
                });

                var response = await amazonPaApiHelper.GetAmazonItemInfoAsync<XmlDocument>(asin);
                var memoryStream = new MemoryStream(StringToUtf8ByteArray(response.InnerXml));
                var serializer = new XmlSerializer(typeof(ItemLookupResponse), "http://webservices.amazon.com/AWSECommerceService/2011-08-01");
                var itemLookupResponse = serializer.Deserialize(memoryStream) as ItemLookupResponse;

                if (itemLookupResponse != null && itemLookupResponse.Items.Any())
                {
                    var item = itemLookupResponse.Items.FirstOrDefault();
                    foreach (var offer in item.Offers)
                    {
                        if (offer.Merchant.Name.Trim().ToLower() == companyName.Trim().ToLower())
                        {
                            return ParsePriceResult.BuyPriceBelongsToCompany;
                        }
                    }
                }

                return ParsePriceResult.BuyPriceNotBelongsToCompany;
            }
            catch (AmazonServiceException awsEx)
            {
                return ParsePriceResult.ParseError;
            }
            catch (Exception ex)
            {
                return ParsePriceResult.ParseError;
            }

        }

        private static byte[] StringToUtf8ByteArray(string xmlString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }
    }
}
