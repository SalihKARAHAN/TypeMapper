/* * * * * * * * * * * * * * * * * Copyright ©2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * * *  
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/9/2018 10:41:16 PM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 12/9/2018 01:36 AM
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

    /// <summary>
    /// 
    /// </summary>
    public sealed class MapperBuilder : IMapperBuilder, IDisposable
    {
        private readonly List<MapDefinition> _definitions;

        /// <summary>
        /// 
        /// </summary>
        public MapperBuilder()
        {
            this._definitions = new List<MapDefinition>();
        }

        public IMapperBuilder DefineMapFor<TTargetType, TSourceType>()
        {
            return this.DefineMapFor<TTargetType, TSourceType>(null);
        }

        public IMapperBuilder DefineMapFor<TTargetType, TSourceType>(Action<MapSpecificationsDefinition<TTargetType, TSourceType>> specifications)
        {
            Type targetType = typeof(TTargetType);
            Type sourceType = typeof(TSourceType);

            // 1. default olarak ismi eşleşen tipleri bul
            List<MapSpecification> mapSpecifications = this.CreateDefaultAssignmentSpecifications(targetType, sourceType);

            // 2. specifications'ı invoke ederek kullanıcı tanımlarını al
            if (specifications != null)
            {
                MapSpecificationsDefinition<TTargetType, TSourceType> specificationsDefinition = new MapSpecificationsDefinition<TTargetType, TSourceType>();
                specifications(specificationsDefinition);
                List<MapSpecification> userDefinedMapSpecifications = specificationsDefinition.Specifications;
                int userDefinedMapSpecificationsCount = userDefinedMapSpecifications.Count;
                for (int i = 0; i < userDefinedMapSpecificationsCount; i++)
                {
                    // 3. kullanıcı tanımları ile default eşleşme sonuçlarını karşılaştır 
                    // 3. 1. target type'ın aynı property'sine kullanıcı tanımında atama yapılmış ise default atama yerine oradaki tanımı dikkate al
                    // 3. 2. eşleşme sonuçlarından farklı bir target property'si için tanım yapılmış ise onu da specification'a ekle
                    MapSpecification userDefinedMapSpecification = userDefinedMapSpecifications[i];
                    int existSpecificationIndex = mapSpecifications.FindIndex(mapSpecification => mapSpecification.TargetPropertyInfo == userDefinedMapSpecification.TargetPropertyInfo);
                    if (existSpecificationIndex != -1)
                    {
                        mapSpecifications.RemoveAt(existSpecificationIndex);
                    }

                    mapSpecifications.Add(userDefinedMapSpecification);
                }
            }

            // 4. Elde edilenlerle bir MapDefinition objesi oluştur ve map definition listesine ekle
            MapDefinition mapDefinition = new MapDefinition
            {
                TargetType = targetType,
                SourceType = sourceType,
                Specifications = mapSpecifications
            };
            this._definitions.Add(mapDefinition);

            // TODO@salih => Reverse mapping tanımlanmamış ise tip değrlerinin yerlerini değiştirerek reverse mapping automation yapılabilir!
            // Yalnız revers'in reverse'ini oluşturmaya çalışmaması için bir kontrol eklenmeli.
            // Bu işlem build olurken yapılmalı!

            return this;
        }

        public IMapper Build()
        {
            IMapper mapper = new Mapper(this._definitions.ToArray());
            return mapper;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void DefineMap(Type targetType, Type sourceType, Action<object, object> specifications)
        {
            throw new NotImplementedException();
        }

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

        private void AssignmentAction(PropertyInfo targetPropertyInfo, object targetObject, PropertyInfo sourcePropertyInfo, object sourceObject)
        {
            // TODO@salih => İki property'nin tipi aynı mı? Birbirine atanabilirler mi? Kontrol edilmeli!!!
            targetPropertyInfo.SetValue(targetObject, sourcePropertyInfo.GetValue(sourceObject));
        }
    }
}