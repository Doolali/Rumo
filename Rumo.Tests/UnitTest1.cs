using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rumo.Models;

namespace Rumo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PersonalizeParsing()
        {
            string json = "{\r\n    \"id\": \"1\",\r\n    \"content\": [\r\n        {\r\n            \"id\": \"123\",\r\n            \"score\": 4.875551549687884E-5\r\n        },\r\n        {\r\n            \"id\": \"1\",\r\n            \"score\": 0.9901253529737497\r\n        },\r\n        {\r\n            \"id\": \"3\",\r\n            \"score\": 0.997672523375305\r\n        },\r\n        {\r\n            \"id\": \"2\",\r\n            \"score\": 1.0\r\n        }\r\n    ]\r\n}";

            RumoRecommendation rec = JsonConvert.DeserializeObject<RumoRecommendation>(json);

            Assert.AreEqual("1", rec.id);
            Assert.AreEqual(4, rec.content.Count);
            Assert.AreEqual("123", rec.content[0].id);
            Assert.AreEqual(4.875551549687884E-05, rec.content[0].score);
        }
    }
}
