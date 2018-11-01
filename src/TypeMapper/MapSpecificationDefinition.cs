/* * * * * * * * * * * * * * * * * Copyright ©2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/9/2018 10:54:29 PM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 11/1/2018 2:55:00 AM
 *      
 *     Since Version: v1.0.0
 *      		
 *           Summary:
 *     			      What does the TypeMapper.MapSpecificationDefinition object do?
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
#if RELEASE
    using System.Diagnostics;
#endif
    /// <summary>
    /// This interface includes the <see cref="IMapSpecificationsDefinition{TTargetType, TSourceType}.For{TPropertyType}(System.Linq.Expressions.Expression{System.Func{TTargetType, TPropertyType}})"/> method
    /// </summary>
    [Serializable]
#if RELEASE
    [DebuggerStepThrough]
    [DebuggerDisplay("MapSpecificationDefinition{}")]
#endif
    internal sealed class MapSpecificationDefinition<TTargetType, TSourceType, TPropertyType> : IMapSpecificationDefinition<TTargetType, TSourceType, TPropertyType>
    {
#if RELEASE 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
#endif
        private readonly MapSpecification _mapSpecification;

        /// <summary>
        /// 
        /// </summary>
        public MapSpecificationDefinition(MapSpecification mapSpecification)
        {
            this._mapSpecification = mapSpecification;
        }

        /// <summary>
        /// This method allows you to define a function, that function uses in mapping process when getting value on a
        /// source object of the <typeparamref name="TSourceType"/>
        /// </summary>
        /// <param name="source">The <seealso cref="Func{TSourceType, TPropertyType}"/></param>
        public void Map(Func<TSourceType, TPropertyType> source)
        {
            this._mapSpecification.AssignmentAction = (targetPropertyInfo, targetObject, sourcePropertyInfo, sourceObject) =>
            {
                TSourceType typedSourceObject = (TSourceType)sourceObject;
                TPropertyType value = source.Invoke(typedSourceObject);
                targetPropertyInfo.SetValue(targetObject, value);
            };
        }
    }
}