using Intranet.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Intranet.Web.UnitTests.Extensions
{
    public class StringHtmlExtensions_Fact
    {
        [Fact]
        public void StripHtmlTest()
        {
            Assert.Null(((string)null).StripHtml());
            Assert.Equal("hello", "hello".StripHtml());
            Assert.Equal("hello", "he<b>ll</b>o".StripHtml());
        }

        [Fact]
        public void TruncateTextTest()
        {
            string test = "1234567890";

            Assert.Null(((string)null).StripHtml());
            Assert.Equal("12345", test.Truncate(5, null));
            Assert.Equal("12345...", test.Truncate(5, "..."));
            Assert.Equal(string.Empty, string.Empty.Truncate(5, null));
            Assert.Equal("12", "12".Truncate(5));
        }

        [Fact]
        public void TruncateHtmlEmptyTest()
        {
            Assert.Null(((string)null).TruncateHtml(100));
            Assert.Equal(string.Empty.TruncateHtml(100), string.Empty);
        }

        [Fact]
        public void TruncateHtmlTextTest()
        {
            // no html test
            Assert.Equal("abc".TruncateHtml(10), "abc");
            Assert.Equal("abc".TruncateHtml(2), "ab");
        }

        [Fact]
        public void TruncateHtmlTextByDelimiterTest()
        {
            const string testHtml = "<b>hello</b><!-- ReadMore --><div>abc</div>";

            // Test for a "read more" delimiter using ordinal comparisons
            Assert.Equal(testHtml.TruncateHtmlByDelimiter("<!-- ReadMore -->"), "<b>hello</b>");
            Assert.NotEqual(testHtml.TruncateHtmlByDelimiter("<!-- READMORE -->"), "<b>hello</b>");

            // Test for a "read more" delimiter using ordinal ignore case comparisons
            Assert.Equal(testHtml.TruncateHtmlByDelimiter("<!-- ReadMore -->", StringComparison.OrdinalIgnoreCase), "<b>hello</b>");
            Assert.Equal(testHtml.TruncateHtmlByDelimiter("<!-- READMORE -->", StringComparison.OrdinalIgnoreCase), "<b>hello</b>");

            // Test for a delimiter that does not exists using ordinal comparison
            Assert.Equal(testHtml.TruncateHtmlByDelimiter("<!-- IDontExist -->"), testHtml);

            // Test truncating with a delimiter that leaves an open tag
            Assert.Equal(testHtml.TruncateHtmlByDelimiter("</b>"), "<b>hello</b>");
        }


        [Fact]
        public void TruncateHtmlTest()
        {
            var html = @"<p>aaa <b>bbb</b>
ccc<br> ddd</p>";

            Assert.Equal(@"<p>aaa <b>bbb</b>
ccc<br> ddd</p>", html.TruncateHtml(100, "no trailing text")); // it ignores unclosed tags

            Assert.Equal(@"<p>aaa <b>bbb</b>
ccc<br>...</br></p>", html.TruncateHtml(14, "..."));

            Assert.Equal(@"<p>aaa <b>bbb</b></p>", html.TruncateHtml(10));

            // self closing test
            html = @"<p>hello<br/>there</p>";
            Assert.Equal(@"<p>hello<br/>th</p>", html.TruncateHtml(7));

            Assert.Equal("<b>i'm</b>", "<b>i'm awesome</b>".TruncateHtml(8));
            Assert.Equal("<b>i'm...</b>", "<b>i'm awesome</b>".TruncateHtml(8, "..."));
        }

        [Fact]
        public void TruncateWordsTest()
        {
            Assert.Null(((string)null).TruncateWords(100));
            Assert.Equal(string.Empty, string.Empty.TruncateWords(100));

            Assert.Equal("big brown", "big brown beaver".TruncateWords(12));
            Assert.Equal("big...", "big brown beaver".TruncateWords(5, "..."));
        }

        [Fact]
        public void TruncateWordsBreakingHtmlTagTest()
        {
            // truncates in the middle of a tag
            Assert.Equal("<b>i'm", "<b>i'm awesome</b>".TruncateWords(16));
        }
    }
}
