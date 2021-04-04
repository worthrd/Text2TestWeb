using System;
namespace Text2TestWebApp.Luis.CodeGenerator
{
    public interface ICodeGenerator
    {
        string ImportLib();
        string OpenWebPageCode(BrowserType browserType, string webPageAddress);
        string FindElementByCode(FindElementAttribute findAttribute,string findValue);
        string ClickElement(string elementName);
        string Submit(string elementName);
        string TypeSomething(string elementName, string typeValue);
        string CurrentElementName { get; set; }
    }
 
}
