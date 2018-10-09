/* * * * * * * * * * * * * * * * * Copyright © 2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/7/2018 3:57:56 AM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 10/7/2018 3:57:56 AM
 *      
 *     Since Version: v1.0.0-alpha
 *      		
 *           Summary:
 *     			      What does the TypeMapper.Mapper object do?
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

/// <summary>
/// 
/// </summary>
namespace TypeMapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class Mapper : IMapper
    {
        private static readonly Dictionary<string, Dictionary<string, PropertyInfo>> _typeTreasures = new Dictionary<string, Dictionary<string, PropertyInfo>>();

        internal Mapper(MapDefinition[] mapDefinitions)
        {

        }

        public TTargetType MapTo<TTargetType>(object sourceObject)
           where TTargetType : new()
        {
            TTargetType targetInstance = new TTargetType();
            Type targetType = typeof(TTargetType);
            PropertyInfo[] targetProperties = targetType.GetProperties();

            Type sourceType = sourceObject.GetType();
            //PropertyInfo[] sourceProperties = sourceType.GetProperty(;

            foreach (PropertyInfo targetPropertyInfo in targetProperties)
            {
                PropertyInfo sourcePropertyInfo = sourceType.GetProperty(targetPropertyInfo.Name);
                if (sourcePropertyInfo != null)
                {
                    targetPropertyInfo.SetValue(targetInstance, sourcePropertyInfo.GetValue(sourceObject));
                }
            }

            return targetInstance;
        }

        public TTargetType MapTov2<TTargetType>(object sourceObject)
          where TTargetType : new()
        {
            TTargetType targetInstance = new TTargetType();
            Type targetType = typeof(TTargetType);
            if (!Mapper._typeTreasures.ContainsKey(targetType.FullName))
            {
                PropertyInfo[] targetProperties = targetType.GetProperties();
                Dictionary<string, PropertyInfo> propertyMapOfType = new Dictionary<string, PropertyInfo>(targetProperties.Length);
                foreach (PropertyInfo targetPropertyInfo in targetProperties)
                {
                    propertyMapOfType.Add(targetPropertyInfo.Name, targetPropertyInfo);
                }

                Mapper._typeTreasures.Add(targetType.FullName, propertyMapOfType);
            }

            Type sourceType = sourceObject.GetType();
            if (!Mapper._typeTreasures.ContainsKey(sourceType.FullName))
            {
                PropertyInfo[] sourceProperties = sourceType.GetProperties();
                Dictionary<string, PropertyInfo> propertyMapOfType = new Dictionary<string, PropertyInfo>(sourceProperties.Length);
                foreach (PropertyInfo sourcePropertyInfo in sourceProperties)
                {
                    propertyMapOfType.Add(sourcePropertyInfo.Name, sourcePropertyInfo);
                }

                Mapper._typeTreasures.Add(sourceType.FullName, propertyMapOfType);
            }

            Dictionary<string, PropertyInfo> targetNamePropertyPair = Mapper._typeTreasures[targetType.FullName];
            Dictionary<string, PropertyInfo> sourceNamePropertyPair = Mapper._typeTreasures[sourceType.FullName];
            string[] matchedKeys = targetNamePropertyPair.Keys.Where(targetKey => sourceNamePropertyPair.Keys.Any(sourceKey => targetKey.Equals(sourceKey))).ToArray();

            for (int i = 0; i < matchedKeys.Length; i++)
            {
                string matchedKey = matchedKeys[i];
                PropertyInfo sourcePropertyInfo = sourceNamePropertyPair[matchedKey];
                PropertyInfo targetPropertyInfo = targetNamePropertyPair[matchedKey];
                targetPropertyInfo.SetValue(targetInstance, sourcePropertyInfo.GetValue(sourceObject));
            }

            return targetInstance;
        }
    }
}
