using System;
using System.Text;
using Newtonsoft.Json.Linq;
using Text2TestWebApp.Luis.CodeGenerator;

namespace Text2TestWebApp.Luis
{
    public class CodeConverter
    {
        CodeLanguage? codeLanguage;
        ICodeGenerator codeGenerator;
        public CodeConverter(CodeLanguage language = CodeLanguage.Python)
        {
            codeLanguage = language;
            switch (codeLanguage.Value)
            {
                case CodeLanguage.Python:
                    codeGenerator = new PythonGenerator();
                    break;
                default:
                    break;
            }
        }

        public String ConvertToCode(String luisIntentJson)
        {
            StringBuilder sbCode = new StringBuilder();
           

            JObject jObject = JObject.Parse(luisIntentJson);

            string topIntent = (string)jObject["prediction"]["topIntent"];

            switch (topIntent)
            {
                case "openWebPage":
                    string page = (string)jObject["prediction"]["entities"]["webpage"].First;
                    sbCode.Append(codeGenerator.ImportLib());
                    sbCode.Append(codeGenerator.OpenWebPageCode(BrowserType.Firefox, page));
                    break;
                case "typeSomething":
                    string findAttribute = (string)jObject["prediction"]["entities"]["elementAttribute"].First;
                    string typedText = (string)jObject["prediction"]["entities"]["typedText"].First;
                    typedText = typedText.Replace(@"""", "");
                    if (findAttribute.StartsWith("name"))
                    {
                        sbCode.Append(codeGenerator.FindElementByCode(FindElementAttribute.find_element_by_name, findAttribute.Split("=")[1]));
                        sbCode.Append(codeGenerator.TypeSomething(codeGenerator.CurrentElementName, typedText));
                    } else if (findAttribute.StartsWith("id"))
                    {
                        sbCode.Append(codeGenerator.FindElementByCode(FindElementAttribute.find_element_by_id, findAttribute.Split("=")[1]));
                        sbCode.Append(codeGenerator.TypeSomething(codeGenerator.CurrentElementName, typedText));
                    }
                    else if (findAttribute.StartsWith("xpath"))
                    {
                        sbCode.Append(codeGenerator.FindElementByCode(FindElementAttribute.find_element_by_xpath, findAttribute.Split("=")[1]));
                        sbCode.Append(codeGenerator.TypeSomething(codeGenerator.CurrentElementName, typedText));
                    }
                    break;
                case "submitPage":
                    string currentElement = codeGenerator.CurrentElementName;
                    sbCode.Append(codeGenerator.Submit(currentElement));
                    break;
                default:
                    break;
            }

            return sbCode.ToString();
        }

    }

    public enum CodeLanguage
    {
        Python,
        Java,
        CSharp,
        JavaScript
    }

    public enum BrowserType
    {
       Chrome,
       Firefox,
       Safari,
       Edge
    }

    public enum FindElementAttribute {
        find_element_by_id,
        find_element_by_name,
        find_element_by_xpath,
        find_element_by_link_text,
        find_element_by_partial_link_text,
        find_element_by_tag_name,
        find_element_by_class_name,
        find_element_by_css_selector
    }
}
