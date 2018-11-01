/* * * * * * * * * * * * * * * * * Copyright ©2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/9/2018 10:38:59 PM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 11/1/2018 2:55:00 AM
 *      
 *     Since Version: v1.0.0
 *      		
 *           Summary:
 *     			      What does the TypeMapper.IMapperBuilder object do?
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

    /// <summary>
    /// MapperBuilder helps you about of create a new mapper instance. <seealso cref="IMapperBuilder.Build()"/>
    /// Besides that, this class provides two methods for defining map<see cref="IMapperBuilder"/> of types couple.
    /// </summary>
    public interface IMapperBuilder
    {
        /// <summary>
        /// This method allows you to define a map with using default matching
        /// </summary>
        /// <typeparam name="TTargetType">The target type is what you want to be mapped.</typeparam>
        /// <typeparam name="TSourceType">The source type containing the source of data in the mapping.</typeparam>
        /// <returns>Returns MapperBuilder instance for you can define again another type couple.</returns>
        IMapperBuilder DefineMapFor<TTargetType, TSourceType>();

        /// <summary>
        /// This method allows you to define a map with your specifications
        /// </summary>
        /// <typeparam name="TTargetType">The target type is what you want to be mapped.</typeparam>
        /// <typeparam name="TSourceType">The source type containing the source of data in the mapping.</typeparam>
        /// <param name="specifications"><see cref="IMapSpecificationsDefinition{TTargetType, TSourceType}"/></param>
        /// <returns>Returns MapperBuilder instance for you can define again another type couple.</returns>
        IMapperBuilder DefineMapFor<TTargetType, TSourceType>(Action<IMapSpecificationsDefinition<TTargetType, TSourceType>> specifications);

        /// <summary>
        /// This method provides you create a mapper instance
        /// </summary>
        /// <returns><see cref="IMapper"/></returns>
        IMapper Build();
    }
}
