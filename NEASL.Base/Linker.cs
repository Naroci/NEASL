using System;
using System.Collections.Generic;
using System.Data;
using NEASL.Base.Linking;
using NEASL.Base.Global.Definitions;

namespace NEASL.Base;

public class Linker
{
    private const string EVENT_LINK_TYPE_KEY = Values.Keywords.Identifier.SECTION_START_IDENTIFIER;
    const string IDENTIFIER_LINK_TYPE_KEY = Values.Keywords.Identifier.SECTION_START_IDENTIFIER;
    
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

        return sectionsResult;
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