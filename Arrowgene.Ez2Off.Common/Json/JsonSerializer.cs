/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2020 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;

namespace Arrowgene.Ez2Off.Common.Json
{
    public class JsonSerializer
    {
        public static readonly DataContractJsonSerializerSettings Settings =
            new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };

        public static T Deserialize<T>(string json)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            T obj = default(T);
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    DataContractJsonSerializer serializer =
                        new DataContractJsonSerializer(typeof(T), Settings);
                    obj = (T) serializer.ReadObject(stream);
                    stream.Close();
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return obj;
        }

        public static string Serialize<T>(T obj)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            string json = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (XmlDictionaryWriter writer =
                        JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
                    {
                        DataContractJsonSerializer serializer =
                            new DataContractJsonSerializer(typeof(T), Settings);
                        serializer.WriteObject(writer, obj);
                        writer.Flush();
                    }

                    byte[] jsonBytes = stream.ToArray();
                    json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return json;
        }
    }
}