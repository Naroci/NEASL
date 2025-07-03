using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using NEASL.Base.Linking;
using NEASL.Base.Global.Definitions;
using NEASL.Base.Reader;

namespace NEASL.Base;

public class Linker
{
    private const string EVENT_LINK_TYPE_KEY = Values.Keywords.Identifier.SECTION_START_IDENTIFIER;
    const string IDENTIFIER_LINK_TYPE_KEY = Values.Keywords.Identifier.SECTION_START_IDENTIFIER;


    public static string GetObjectsName(IBaseLinkedObject linkObject, string scriptContent)
    {
        if (linkObject == null || string.IsNullOrEmpty(scriptContent))
            throw new ArgumentNullException(nameof(linkObject));
        
        Identifier rootIdentifier = AttributeHandler.GetAttributeByObject<Identifier>(linkObject);
        scriptContent.Trim();
        string startSearch = $"{rootIdentifier.Name}(";
        string endSearch = $"):";
        
        if (scriptContent.IndexOf(startSearch, StringComparison.OrdinalIgnoreCase) >= -1
            && scriptContent.IndexOf(endSearch, StringComparison.OrdinalIgnoreCase) >= 1)
        {
            string part = scriptContent.Substring(scriptContent.IndexOf(startSearch) + startSearch.Length, scriptContent.Length - (scriptContent.IndexOf(startSearch) + startSearch.Length));
            string className = part.Substring(0, part.IndexOf(endSearch));
            if (!string.IsNullOrEmpty(className))
                return className;
        }
   
        return null;
    }

    public static string ExtractName(string IdentifierName)
    {
        int start = IdentifierName.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER);
        if (start <= 0)
            return IdentifierName;
        
        var result = IdentifierName.Substring(0,start);
        return result;
    }

    public static string ExtractContent(string Base)
    {
        if (string.IsNullOrEmpty(Base))
            return null;
        
        int firstInd = Base.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER) + Values.Keywords.Identifier.SECTION_END_IDENTIFIER.Length;
        if (firstInd <= 0)
            return Base;
        int lastind = Base.LastIndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER);
        if (lastind <= 0)
            return Base;
        
        var trimmed = Base.Substring(firstInd, lastind - firstInd);
        return trimmed;
    }

    public static List<ScriptSection> GetSections(string scriptContent)
    {
        List<ScriptSection> sections = new List<ScriptSection>();
        var matches = Regex.Matches(scriptContent, @"\b(?<name>\w+)(?:\s*\([^)]*\))?\s*:.*?:\k<name>", RegexOptions.Singleline);
        var result = Regex.Matches(scriptContent, @"\b\w+(?:\s*\([^)]*\))?\s*:");

        if (matches.Count >= 0)
        {
            foreach (Match fmatch in matches)
            {
                var res = fmatch.Value;
                var name = ExtractName(res);
                var content = ExtractContent(res);
                sections.Add(new ScriptSection() { KeyNameIdentifier = name, Content = content });
            }
            //Console.WriteLine($"Gefunden: {result.Value}");
            // Gibt z. B. aus: "BUTTON(arg1, arg2):"
        }

        return sections;
    }

    public static Dictionary<string, string> LoadSections(IBaseLinkedObject linkObject, string scriptContent)
    {
        Dictionary<string, string>  sectionsResult = new Dictionary<string, string>();
        if (linkObject == null)
            throw new ArgumentNullException(nameof(linkObject));
        
        if (string.IsNullOrEmpty(scriptContent))
        {
            Console.WriteLine("[WARN] Nothing to link!");
            return new Dictionary<string, string>();
        }

        Identifier rootIdentifier = AttributeHandler.GetAttributeByObject<Identifier>(linkObject);
        List<Signature> defiSignatures = AttributeHandler.GetAttributesByAttributeTypeFromObjectsFields<Signature>(linkObject);
        
        if (rootIdentifier == null)
            throw new NullReferenceException("No Section/Component definition found!");
        if (rootIdentifier != null && string.IsNullOrEmpty(rootIdentifier.Name))
            throw new NoNullAllowedException(rootIdentifier.Name);
        
        string rootName = rootIdentifier.Name;
        string rootContent = fetchScriptPartByKeyword(rootName, scriptContent);
        if (string.IsNullOrEmpty(rootContent))
        {
            return new Dictionary<string, string>();
        }

        var sections = GetSections(rootContent);
        if (sections.Count == 0)
            return sectionsResult;
        
        foreach (var section in sections)
        {
            if (string.IsNullOrEmpty(section.KeyNameIdentifier))
                continue;
            if (sectionsResult.ContainsKey(section.KeyNameIdentifier))
                continue;
            sectionsResult.Add(section.KeyNameIdentifier, section.Content);
        }
        
        return sectionsResult;

        /*


        foreach (var signature in defiSignatures)
        {
            if (signature == null || signature != null && string.IsNullOrEmpty(signature.Name))
                continue;

            if (!SignatureExistsInScriptPart(rootContent, signature.Name))
                continue;
            
            var signatureContent = fetchScriptPartByKeyword(signature.Name, rootContent);
            sectionsResult.Add(signature.Name,signatureContent);
            rootContent = trimByKeyword(signature.Name,rootContent);
        }

        return sectionsResult;*/
    }

    private static bool SignatureExistsInScriptPart(string scriptPart, string keywordName, string key = IDENTIFIER_LINK_TYPE_KEY)
    {
        string KeyWordSTART = $"{keywordName}{key}";
        string KeyWordEND = $"{key}{keywordName}";
        
        return (scriptPart.IndexOf(KeyWordSTART, StringComparison.OrdinalIgnoreCase) >= -1
                && scriptPart.IndexOf(KeyWordEND, StringComparison.OrdinalIgnoreCase) >= 1);
    }

    private static string fetchScriptPartByKeyword(string keywordName, string scriptContent, string key = IDENTIFIER_LINK_TYPE_KEY)
    {
        if (string.IsNullOrEmpty(keywordName) || string.IsNullOrEmpty(scriptContent))
            return string.Empty;

        if (!string.IsNullOrEmpty(keywordName) && !string.IsNullOrEmpty(scriptContent))
        {
            string KeyWordSTART = $"{keywordName}{key}";
            string KeyWordEND = $"{key}{keywordName}";
            if (scriptContent.IndexOf(KeyWordSTART, StringComparison.OrdinalIgnoreCase) >= -1
                && scriptContent.IndexOf(KeyWordEND, StringComparison.OrdinalIgnoreCase) >= 1)
            {
                string part = scriptContent.Substring(scriptContent.IndexOf(KeyWordSTART) + KeyWordSTART.Length, scriptContent.Length - (scriptContent.IndexOf(KeyWordSTART) + KeyWordSTART.Length));
                return part.Substring(0, part.IndexOf(KeyWordEND));
            }
        }

        return scriptContent;
    }

    private static string trimByKeyword(string keywordName, string scriptContent, string key = IDENTIFIER_LINK_TYPE_KEY)
    {
        if (string.IsNullOrEmpty(keywordName) || string.IsNullOrEmpty(scriptContent))
            return string.Empty;

        if (!string.IsNullOrEmpty(keywordName) && !string.IsNullOrEmpty(scriptContent))
        {
            string KeyWordSTART = $"{keywordName}{key}";
            string KeyWordEND = $"{key}{keywordName}";
            int startIndex = scriptContent.IndexOf(KeyWordSTART, StringComparison.OrdinalIgnoreCase);
            int endIndex = scriptContent.IndexOf(KeyWordEND, StringComparison.OrdinalIgnoreCase);
            if (scriptContent.IndexOf(KeyWordSTART, StringComparison.OrdinalIgnoreCase) >= -1
                && scriptContent.IndexOf(KeyWordEND, StringComparison.OrdinalIgnoreCase) >= 1)
            {
                return scriptContent.Remove(startIndex, endIndex - startIndex + KeyWordSTART.Length);
            }
        }

        return scriptContent;
    }
}