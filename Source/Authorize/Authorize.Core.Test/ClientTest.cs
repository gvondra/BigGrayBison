using BigGrayBison.Authorize.Core;
using BigGrayBison.Authorize.Data.Models;

namespace Authorize.Core.Test
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        [DataRow("http://unittest.org:8080/path1/path11?y=17", true)]
        [DataRow("https://unittest.org:8080/path1/path11?y=17", false)]
        [DataRow("http://unittest.org/path1/path11?y=17", false)]
        [DataRow("http://unittest.org:8080/path1/", false)]
        [DataRow("http://unittest.org:8080/path1/path11#tag", false)]
        public void IsValidRedirectUrlTest(string url, bool expectedResult)
        {
            ClientData data = new ClientData
            {
                RedirectionUrls = "[ \"\", \"https://example.com\", \"http://example.com\", \"http://unittest.org:8080/path1\", \"http://unittest.org:8080/path1/path11?x=42\", \"http://unittest.org:8080/path1/path12\", \"\" ]"
            };
            Client client = new Client(data);
            Assert.AreEqual(expectedResult, client.IsValidRedirectUrl(url));
        }
    }
}
