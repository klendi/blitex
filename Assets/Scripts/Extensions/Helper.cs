/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.IO;
using System.Xml.Serialization;

public static class Helper
{
    //Serialize
    public static string Serialize<T>(this T toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialize);

        return writer.ToString();
    }

    //De-Serialize
    public static T DeSerialize<T>(this string toDeSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(toDeSerialize);

        return (T)xml.Deserialize(reader);
    }
}
