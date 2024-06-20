namespace downloadHtml
{

    class GetHtml
    {
        public string getHtmlFromUrl(string Url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(Url).Result;

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
