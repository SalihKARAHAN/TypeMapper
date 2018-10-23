/* * * * * * * * * * * * * * * * * Copyright ©2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/9/2018 10:52:59 PM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 10/9/2018 10:52:59 PM
 *      
 *     Since Version: v1.0.0
 *      		
 *           Summary:
 *     			      What does the TypeMapper.MapSpecificationsDefinition object do?
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTargetType"></typeparam>
    /// <typeparam name="TSourceType"></typeparam>
    [Serializable]
    [DebuggerStepThrough]
    public sealed class MapSpecificationsDefinition<TTargetType, TSourceType> : IMapSpecificationsDefinition<TTargetType, TSourceType>, IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<MapSpecification> _specification;

        internal List<MapSpecification> Specifications => this._specification;

        /// <summary>
        /// 
        /// </summary>
        internal MapSpecificationsDefinition()
        {
            this._specification = new List<MapSpecification>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public IMapSpecificationDefinition<TTargetType, TSourceType, TPropertyType> For<TPropertyType>(Expression<Func<TTargetType, TPropertyType>> target)
        {
            MapSpecification mapSpecification = new MapSpecification();
            MemberExpression targetMemberExpression = (MemberExpression)target.Body;
            mapSpecification.TargetPropertyInfo = (PropertyInfo)targetMemberExpression.Member;
            IMapSpecificationDefinition<TTargetType, TSourceType, TPropertyType> mapSpecificationDefinition = new MapSpecificationDefinition<TTargetType, TSourceType, TPropertyType>(mapSpecification);
            this._specification.Add(mapSpecification);
            return mapSpecificationDefinition;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}