using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecognizeLeafs
{
    class Leaf
    {
        public string Name { get; set; }
        public string HyperLink { get; set;  }
        public string Probability { get; set; }

        public static Leaf Deserialize(string content)
        {
            var serializer = JsonSerializer.Create();
            return JsonConvert.DeserializeObject<Leaf>(content);
        }
    }
}
