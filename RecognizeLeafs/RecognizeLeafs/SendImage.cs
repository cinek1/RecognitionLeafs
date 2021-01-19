using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecognizeLeafs
{
    class SendImage
    {
        private HttpClient client; 
        
        public SendImage()
        {
            client = new HttpClient(); 
        }

        public async Task<string> SendImageToApi(byte[] content)
        {
            
            MultipartFormDataContent form = new MultipartFormDataContent
            {
                { new ByteArrayContent(content, 0, content.Length), "file", "pic.jpeg" }
            };
            HttpResponseMessage response = await client.PostAsync("http://10.0.2.2:5000/upload", form);
            response.EnsureSuccessStatusCode();
            return  await response.Content.ReadAsStringAsync();
        } 
    }
}
