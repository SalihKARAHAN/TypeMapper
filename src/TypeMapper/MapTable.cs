﻿/* * * * * * * * * * * * * * * * * Copyright ©2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/9/2018 10:54:53 PM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 11/1/2018 2:55:00 AM
 *      
 *     Since Version: v1.0.0
 *      		
 *           Summary:
 *     			      What does the TypeMapper.MapTable object do?
 *                    Which was created on demand? 
 *           License:
 *                   MIT License
 *                   
 *                   Copyright (c) 2018 Salih KARAHAN
 *                   
 *                   Permission is hereby granted, free of charge, to any person obtaining a copy
 *                   of this software and associated documentation files (the "Software"), to deal
 *                   in the Software without restriction, including without limitation the rights
 *                   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *                   copies of the Software, and to permit persons to whom the Software is
 *                   furnished to do so, subject to the following conditions:
 *                   
 *                   The above copyright notice and this permission notice shall be included in all
 *                   copies or substantial portions of the Software.
 *                   
 *                   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *                   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *                   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *                   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *                   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *                   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *                   SOFTWARE.
 *       Description:
 *                   _
 *           Changes: 
 *                    yyyy.mm.dd: <mail.address@provider.com>
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace TypeMapper
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
#if RELEASE
    using System.Diagnostics;
#endif

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
#if RELEASE
    [DebuggerStepThrough]
    [DebuggerDisplay("Size: {this._index.Length}")]
#endif
    internal sealed class MapTable
    {
#if RELEASE 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
#endif
        private readonly string[] _index;


#if RELEASE
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        private readonly Map[] _maps;

#if RELEASE
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
#endif
        private readonly Encoding _utf8Encoding;

#if RELEASE
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
#endif
        private readonly HashAlgorithm _md5HashAlgorithm;

        /// <summary>
        /// 
        /// </summary>
        internal MapTable(ref int mapSize)
        {
            this._md5HashAlgorithm = new MD5CryptoServiceProvider();
            this._utf8Encoding = Encoding.UTF8;
            this._index = new string[mapSize];
            this._maps = new Map[mapSize];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTargetType"></typeparam>
        /// <typeparam name="TSourceType"></typeparam>
        /// <returns></returns>
        internal string CreateIndex<TTargetType, TSourceType>()
        {
            return this.CreateIndex(typeof(TTargetType), typeof(TSourceType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        internal string CreateIndex(Type targetType, Type sourceType)
        {
            string rawIndexContent = string.Format("{0}{1}{2}|{3}{4}{5}"
                , targetType.Assembly.FullName, targetType.Assembly.ImageRuntimeVersion, targetType.FullName
                , sourceType.Assembly.FullName, sourceType.Assembly.ImageRuntimeVersion, sourceType.FullName);

            string hash = this.CreateMD5Hash(rawIndexContent);
            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="map"></param>
        internal void AddMap(ref int index, Map map)
        {
            this._index[index] = map.Hash;
            this._maps[index] = map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTargetType"></typeparam>
        /// <typeparam name="TSourceType"></typeparam>
        /// <returns></returns>
        internal Map FindMap<TTargetType, TSourceType>()
        {
            return this.FindMap(typeof(TTargetType), typeof(TSourceType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        internal Map FindMap(Type targetType, Type sourceType)
        {
            string hash = this.CreateIndex(targetType, sourceType);
            int index = Array.IndexOf<string>(this._index, hash);
            Map map = this._maps[index];
            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CreateMD5Hash(string text)
        {
            byte[] textBytes = this._utf8Encoding.GetBytes(text);
            byte[] hashBytes = this._md5HashAlgorithm.ComputeHash(textBytes);
            int hashBytesCount = hashBytes.Length;
            StringBuilder hashBuilder = new StringBuilder();
            for (int i = 0; i < hashBytesCount; i++)
            {
                hashBuilder.AppendFormat("{0:x2}", hashBytes[i]);
            }

            return hashBuilder.ToString();
        }
    }
}