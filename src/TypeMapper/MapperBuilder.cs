/* * * * * * * * * * * * * * * * * Copyright ©2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * * *  
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/9/2018 10:41:16 PM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 11/1/2018 2:55:00 AM
 *      
 *     Since Version: v1.0.0
 *      		
 *           Summary:
 *     			      What does the TypeMapper.MapperBuilder object do?
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
    using System.Reflection;
#if RELEASE
    using System.Diagnostics;
#endif

    /// <summary>
    /// MapperBuilder helps you about of create a new mapper instance. <seealso cref="MapperBuilder.Build()"/>
    /// Besides that, this class provides two methods for defining map<see cref="MapDefinition"/> of types couple.
    /// </summary>
    [Serializable]
#if RELEASE
    [DebuggerStepThrough]
    [DebuggerDisplay("MapperBuilder{}")]
#endif
    public sealed class MapperBuilder : IMapperBuilder
    {
#if RELEASE 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
#endif
        private readonly List<MapDefinition> _definitions;

        /// <summary>
        /// Create a new MapperBuilder class instance.
        /// </summary>
        public MapperBuilder()
        {
            this._definitions = new List<MapDefinition>();
        }

        /// <summary>
        /// This method allows you to define a map with using default matching
        /// </summary>
        /// <typeparam name="TTargetType">The target type is what you want to be mapped.</typeparam>
        /// <typeparam name="TSourceType">The source type containing the source of data in the mapping.</typeparam>
        /// <returns>Returns MapperBuilder instance for you can define again another type couple.</returns>
        public IMapperBuilder DefineMapFor<TTargetType, TSourceType>()
        {
            return this.DefineMapFor<TTargetType, TSourceType>(null);
        }

        /// <summary>
        /// This method allows you to define a map with your specifications
        /// </summary>
        /// <typeparam name="TTargetType">The target type is what you want to be mapped.</typeparam>
        /// <typeparam name="TSourceType">The source type containing the source of data in the mapping.</typeparam>
        /// <param name="specifications"><see cref="IMapSpecificationsDefinition{TTargetType, TSourceType}"/></param>
        /// <returns>Returns MapperBuilder instance for you can define again another type couple.</returns>
        public IMapperBuilder DefineMapFor<TTargetType, TSourceType>(Action<IMapSpecificationsDefinition<TTargetType, TSourceType>> specifications)
        {
            Type targetType = typeof(TTargetType);
            Type sourceType = typeof(TSourceType);
            List<MapSpecification> mapSpecifications = this.CreateDefaultAssignmentSpecifications(targetType, sourceType);
            if (specifications != null)
            {
                this.HandleUserDefinedSpecifications(specifications, mapSpecifications);
            }

            MapDefinition mapDefinition = new MapDefinition(targetType, sourceType, mapSpecifications);
            this._definitions.Add(mapDefinition);
            return this;
        }

        /// <summary>
        /// This method provides you create a mapper instance
        /// </summary>
        /// <returns><see cref="IMapper"/></returns>
        public IMapper Build()
        {
            this.DefineUndefinedReverseMaps();
            IMapper mapper = new Mapper(this._definitions.ToArray());
            return mapper;
        }

        /// <summary>
        /// This method creates default specifications for defined type couple according to name conversion
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        private List<MapSpecification> CreateDefaultAssignmentSpecifications(Type targetType, Type sourceType)
        {
            List<MapSpecification> mapSpecifications = new List<MapSpecification>();
            PropertyInfo[] targetPropertyInfos = targetType.GetProperties();
            foreach (PropertyInfo targetPropertyInfo in targetPropertyInfos)
            {
                PropertyInfo sourcePropertyInfo = sourceType.GetProperty(targetPropertyInfo.Name);
                if (sourcePropertyInfo != null)
                {
                    MapSpecification defaultSpecification = new MapSpecification
                    {
                        TargetPropertyInfo = targetPropertyInfo,
                        SourcePropertyInfo = sourcePropertyInfo,
                        AssignmentAction = this.AssignmentAction
                    };

                    mapSpecifications.Add(defaultSpecification);
                }
            }

            return mapSpecifications;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTargetType"></typeparam>
        /// <typeparam name="TSourceType"></typeparam>
        /// <param name="specifications"></param>
        /// <param name="mapSpecifications"></param>
        private void HandleUserDefinedSpecifications<TTargetType, TSourceType>(Action<MapSpecificationsDefinition<TTargetType, TSourceType>> specifications, List<MapSpecification> mapSpecifications)
        {
            MapSpecificationsDefinition<TTargetType, TSourceType> specificationsDefinition = new MapSpecificationsDefinition<TTargetType, TSourceType>();
            specifications(specificationsDefinition);
            List<MapSpecification> userDefinedMapSpecifications = specificationsDefinition.Specifications;
            int userDefinedMapSpecificationsCount = userDefinedMapSpecifications.Count;
            for (int i = 0; i < userDefinedMapSpecificationsCount; i++)
            {
                MapSpecification userDefinedMapSpecification = userDefinedMapSpecifications[i];
                int existSpecificationIndex = mapSpecifications.FindIndex(mapSpecification
                    => mapSpecification.TargetPropertyInfo == userDefinedMapSpecification.TargetPropertyInfo);
                if (existSpecificationIndex != -1)
                {
                    mapSpecifications.RemoveAt(existSpecificationIndex);
                }

                mapSpecifications.Add(userDefinedMapSpecification);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DefineUndefinedReverseMaps()
        {
            List<MapDefinition> reverseMapDefinitionList = new List<MapDefinition>();
            int definedMapCount = this._definitions.Count;
            for (int i = 0; i < definedMapCount; i++)
            {
                MapDefinition mapDefinition = this._definitions[i];
                MapDefinition reverseMapDefinition = this._definitions.Find(definition
                    => definition.TargetType == mapDefinition.SourceType
                    && definition.SourceType == mapDefinition.TargetType
                );

                if (reverseMapDefinition == null)
                {
                    List<MapSpecification> defaultSpecificationsOfReverseMap = this.CreateDefaultAssignmentSpecifications(mapDefinition.SourceType, mapDefinition.TargetType);
                    reverseMapDefinition = new MapDefinition(mapDefinition.SourceType, mapDefinition.TargetType, defaultSpecificationsOfReverseMap);
                    reverseMapDefinitionList.Add(reverseMapDefinition);
                }
            }

            this._definitions.AddRange(reverseMapDefinitionList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPropertyInfo"></param>
        /// <param name="targetObject"></param>
        /// <param name="sourcePropertyInfo"></param>
        /// <param name="sourceObject"></param>
        private void AssignmentAction(PropertyInfo targetPropertyInfo, object targetObject, PropertyInfo sourcePropertyInfo, object sourceObject)
        {
            // TODO@salih => İki property'nin tipi aynı mı? Birbirine atanabilirler mi? Kontrol edilmeli!!!
            targetPropertyInfo.SetValue(targetObject, sourcePropertyInfo.GetValue(sourceObject));
        }
    }
}