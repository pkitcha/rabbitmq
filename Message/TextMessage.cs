using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message
{
    public class TextMessage
    {
        public string Text { get; set; }

        public InnerText innerText;

        public TextMessage(string body , int seq)
        {
            innerText = new InnerText();
            innerText.ID = Guid.NewGuid().ToString();
            innerText.CreateDate = DateTime.Now;
            innerText.bodyMsg = body;
            innerText.Sequence = seq;
        }

    }

    public class InnerText
    {
        public string ID { get; set; }
        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public string bodyMsg { get; set; }
    }
}
