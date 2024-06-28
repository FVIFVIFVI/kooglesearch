using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace parserWords
{
    class ParserWords
    {
        private HtmlDocument _Html;

        public ParserWords()
        {
            _Html = new HtmlDocument();
        }

        private static char[] separators()
        {
            return new char[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '^',
                                '-', '(', ')', '+', '"', '=', '|', '/', '{', '}', '`',
                                '[', ']', '&', '\'', '#', '_', '<', '>', '\\', '%', '$',
                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }

        private static string[] wordsNotSerch()
        {
            return new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                                "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "the", "of", "in", "and", "to", "on", "from", "he", "for", "with", "as", "his" };
        }

        private static void process_words(string[] wordInText, Dictionary<string, int> words)
        {
            foreach (string word in wordInText)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    string lower = word.Trim().ToLower();
                    if (!wordsNotSerch().Contains(lower))
                    {
                        if (words.ContainsKey(lower))
                        {
                            words[lower]++;
                        }
                        else
                        {
                            words[lower] = 1;
                        }
                    }
                }
            }
        }

        public Dictionary<string, int> getWords(string Html)
        {
            //Console.WriteLine(Html);
            var words = new Dictionary<string, int>();
            IWebDriver driver = null;

            try
            {

                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless"); // Run Chrome in headless mode
              
                driver = new ChromeDriver(options);

                if (driver == null)
                {
                    throw new InvalidOperationException("WebDriver initialization failed.");
                }
                Console.WriteLine(Html+"html");
                driver.Navigate().GoToUrl(Html);

                string pageText = driver.FindElement(By.XPath("/html/body")).Text;
                
                Console.WriteLine(12);

                Console.WriteLine( pageText );

                _Html.LoadHtml(pageText);

                //var textNodes = _Html.DocumentNode.SelectNodes("//p|//h1|//h2|//h3|//h4|//h5|//h6");
                var textNodes = pageText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (textNodes != null)
                {
                    foreach (var textNode in textNodes)
                    {
                        string[] wordInText = textNode.Split(separators(), StringSplitOptions.RemoveEmptyEntries);
                        process_words(wordInText, words);
                    }
                }

                Console.WriteLine(String.Format("{0,8}", 979));
            }
            
            catch (Exception ex)
            {
                //Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine("An error occurred: " );
            }
            finally
            {
                if (driver != null)
                {
                    try
                    {
                        driver.Quit();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Error closing WebDriver: " + ex.Message);
                        Console.WriteLine("An error occurred: ");
                    }
                }
            }

            return words;
        }
    }
}
