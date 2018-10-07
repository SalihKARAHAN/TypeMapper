/* * * * * * * * * * * * * * * * * Copyright © 2018 Salih KARAHAN KARAHAN-LAB® Products * * * * * * * * * * * * * * * * *
 *           Creator: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Created Date: 10/7/2018 4:35:20 AM
 *      Last Changer: Salih KARAHAN <salih.karahan@karahan-lab.com>
 *      Changed Date: 10/7/2018 4:35:20 AM
 *      
 *     Since Version: v1.0.0
 *      		
 *           Summary:
 *     			      What does the TypeMapper.Tests.BenchmarkTests object do?
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
namespace TypeMapper.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TypeMapper.Tests.DummyObjects;

    [TestClass]
    public class Benchmark2Tests
    {
        private readonly Mapper _mapper;
        private readonly BenchmarkEntity _entity;
        private readonly AutoMapper.IMapper _autoMapper;

        public Benchmark2Tests()
        {
            _mapper = new Mapper();
            _entity = new BenchmarkEntity
            {
                Username = "salih.karahan",
                Password = "My$up3RSecRetP@s#w0rD",
                Fullname = "Salih KARAHAN",
                Title = "Principle Software Architech",
                IsActive = true
            };
            _autoMapper = new AutoMapper.MapperConfiguration(autoMapperConfiguration =>
            {
                autoMapperConfiguration.CreateMap<BenchmarkDto, BenchmarkEntity>();
            }).CreateMapper();
        }

        [TestMethod]
        public void RunV1On100KProcess()
        {
            for (int i = 0; i < 100000; i++)
            {
                BenchmarkDto userDto = _mapper.MapTo<BenchmarkDto>(_entity);
            }

        }

        [TestMethod]
        public void RunV2On100KProcess()
        {
            for (int i = 0; i < 100000; i++)
            {
                BenchmarkDto userDto = _mapper.MapTov2<BenchmarkDto>(_entity);
            }

        }

        [TestMethod]
        public void RunAutoMapperOn100KProcess()
        {
            for (int i = 0; i < 100000; i++)
            {
                BenchmarkDto userDto = _autoMapper.Map<BenchmarkDto>(_entity);
            }

        }
    }
}
