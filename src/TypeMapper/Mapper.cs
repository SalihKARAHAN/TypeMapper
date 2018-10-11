/* * * * * * * * * * * * * * * * * Copyright © 2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/7/2018 3:57:56 AM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 12/9/2018 01:36 AM
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


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class Mapper : IMapper
    {
        private readonly MapTable _mapTable;

        internal Mapper(MapDefinition[] mapDefinitions)
        {
            int mapDefinitionsCount = mapDefinitions.Length;
            this._mapTable = new MapTable(ref mapDefinitionsCount);
            for (int i = 0; i < mapDefinitionsCount; i++)
            {
                MapDefinition mapDefinition = mapDefinitions[i];
                Map map = new Map
                {
                    Hash = this._mapTable.CreateIndex(mapDefinition.TargetType, mapDefinition.SourceType),
                    Specifications = mapDefinition.Specifications.ToArray<MapSpecification>()
                };

                this._mapTable.AddMap(ref i, map);
            }
        }

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

            }

            return targetTypeInstance;
        }
    }
}
