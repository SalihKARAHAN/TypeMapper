/* * * * * * * * * * * * * * * * * Copyright © 2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/7/2018 3:57:56 AM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 11/1/2018 2:55:00 AM
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
    using System.Linq;
    using System.Reflection;
#if RELEASE
    using System.Diagnostics;
#endif


    /// <summary>
    /// This class provides you can access to an implementation of the <see cref="IMapper.MapTo{TTargetType}(object)"/> method
    /// </summary>
    [Serializable]
#if RELEASE
    [DebuggerStepThrough]
    [DebuggerDisplay("Mapper")]
#endif
    public sealed class Mapper : IMapper
    {
#if RELEASE
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        private readonly MapTable _mapTable;

        internal Mapper(MapDefinition[] mapDefinitions)
        {
            int mapDefinitionsCount = mapDefinitions.Length;
            this._mapTable = new MapTable(ref mapDefinitionsCount);
            for (int i = 0; i < mapDefinitionsCount; i++)
            {
                MapDefinition mapDefinition = mapDefinitions[i];
                string hash = this._mapTable.CreateIndex(mapDefinition.TargetType, mapDefinition.SourceType);
                MapSpecification[] mapSpecifications = mapDefinition.Specifications.ToArray<MapSpecification>();
                Map map = new Map(hash, mapSpecifications);
                this._mapTable.AddMap(ref i, map);
            }
        }

        /// <summary>
        /// This method converts your <paramref name="sourceObject"/> to a new instance of <typeparamref name="TTargetType"/>
        /// according to defined by you or default mapping specifications. The default mapping specifications  are created 
        /// by the name similarity of properties in these types.
        /// </summary>
        /// <typeparam name="TTargetType"></typeparam>
        /// <param name="sourceObject"></param>
        /// <returns>An instance of <typeparamref name="TTargetType"/></returns>
        public TTargetType MapTo<TTargetType>(object sourceObject)
           where TTargetType : new()
        {
            TTargetType targetTypeInstance = new TTargetType();
            Map map = this._mapTable.FindMap(typeof(TTargetType), sourceObject.GetType());
            if (map != null)
            {
                int specCount = map.Specifications.Length;
                for (int i = 0; i < specCount; i++)
                {
                    MapSpecification mapSpecification = map.Specifications[i];
                    mapSpecification.AssignmentAction(
                        mapSpecification.TargetPropertyInfo
                        , targetTypeInstance
                        , mapSpecification.SourcePropertyInfo
                        , sourceObject);
                }
            }
            else
            {
                PropertyInfo[] targetProperties = typeof(TTargetType).GetProperties();
                Type sourceType = sourceObject.GetType();
                foreach (PropertyInfo targetPropertyInfo in targetProperties)
                {
                    PropertyInfo sourcePropertyInfo = sourceType.GetProperty(targetPropertyInfo.Name);
                    if (sourcePropertyInfo != null)
                    {
                        targetPropertyInfo.SetValue(targetTypeInstance, sourcePropertyInfo.GetValue(sourceObject));
                    }
                }
            }

            return targetTypeInstance;
        }
    }
}
