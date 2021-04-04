using System;
namespace Text2TestWebApp.Luis.CodeGenerator
{
    public class PythonGenerator : ICodeGenerator
    {
        int elementCount = 0;
        string driverName = "driver";
        string CRLF = "\r\n";
        public string CurrentElementName { get; set; }

        public PythonGenerator()
        {
        }

        public string ClickElement(string elementName)
        {
            return String.Format("{0}.click()\r\n",elementName);
        }

        public string FindElementByCode(FindElementAttribute findAttribute,string findValalue)
        {
            this.CurrentElementName = String.Format("elem{0}", elementCount++);
            return String.Format("{3} = {0}.{1}(\"{2}\")"+CRLF, driverName, findAttribute.ToString(), findValalue, CurrentElementName);
        }

        public string ImportLib()
        {
            return "from selenium import webdriver" + CRLF 
                    + "from selenium.webdriver.common.keys import Keys"+CRLF;
        }

        public string OpenWebPageCode(BrowserType browserType,string webPageAddress)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    return String.Format("{0} = webdriver.Firefox()\n{0}.get(\"{1}\")" + CRLF, driverName,webPageAddress);
                default:
                    return CRLF;
            }
        }

        public string Submit(string elementName)
        {
            return String.Format("{0}.submit()"+CRLF, elementName);
        }

        public string TypeSomething(string elementName,string typeValue)
        {
            this.CurrentElementName = elementName;
            return String.Format("{0}.send_keys(\"{1}\")"+CRLF,elementName,typeValue);
        }


    }
}
