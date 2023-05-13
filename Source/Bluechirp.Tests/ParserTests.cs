using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bluechirp.Parser;
using Bluechirp.Parser.Interfaces;
using Bluechirp.Parser.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public async Task TestHtmlParser()
        {
            TootParser parser = new TootParser();

            string htmlInput = "<p>gonna send this toot to test bluechirp&#39;s new parser :ablobcatbongo: " +
                               "</p><p><a href=\"https://tech.lgbt/tags/hashtags\" class=\"mention hashtag\" rel=\"tag\">#<span>hashtags</span></a> " +
                               "<span class=\"h-card\"><a href=\"https://hachyderm.io/@witchdagger\" class=\"u-url mention\">@<span>witchdagger</span></a></span></p>";
            List<IMastodonContent> expectedOutput = new List<IMastodonContent>
            {
                new MastodonText("gonna send this toot to test bluechirp's new parser :ablobcatbongo: "),
                new MastodonText("\n\n"),
                new MastodonHashtag("hashtags"),
                new MastodonText(" "),
                new MastodonMention("witchdagger")
            };

            List<IMastodonContent> parsedResult = new List<IMastodonContent>();
            try
            {
                parsedResult = await parser.ParseContentAsync(htmlInput);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.IsTrue(expectedOutput.SequenceEqual(parsedResult));
        }

        [TestMethod]
        public async Task TestRawParser()
        {
            TootParser parser = new TootParser();

            string htmlInput = "Why is this toot just raw text?";
            List<IMastodonContent> expectedOutput = new List<IMastodonContent>
            {
                new MastodonText("Why is this toot just raw text?")
            };

            List<IMastodonContent> parsedResult = new List<IMastodonContent>();
            try
            {
                parsedResult = await parser.ParseContentAsync(htmlInput);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.IsTrue(expectedOutput.SequenceEqual(parsedResult));
        }
    }
}