using AutoMapper;
using Online_Shopping.Api.Manage.DTOs;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            #region CategoriesMapper
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryItemDto>();
            #endregion

            #region SubCategoryMapper
            CreateMap<SubCategoryCreateDto, SubCategory>();
            CreateMap<Category, CategoryInSubDto>();
            CreateMap<SubCategory, SubCategoryGetDto>();
            CreateMap<SubCategory, SubCategoryItemDto>()
                .ForMember(dest => dest.CategoryName, from => from.MapFrom(x => x.Category.Name));
            #endregion

            #region TagMapper
            CreateMap<TagCreateDto, Tag>();
            CreateMap<Tag,TagGetDto>();
            CreateMap<Tag,TagItemDto>();
            #endregion

            #region BrandMapper
            CreateMap<BrandCreateDto, Brand>();
            CreateMap<Brand, BrandGetDto>();
            CreateMap<Brand, BrandItemDto>();
            #endregion

            #region SizeMapper
            CreateMap<SizeCreateDto, Size>();
            CreateMap<Size, SizeGetDto>();
            CreateMap<Size, SizeItemDto>();
            #endregion

            #region ColorMapper
            CreateMap<ColorCreateDto, Color>();
            CreateMap<Color, ColorGetDto>();
            CreateMap<Color, ColorItemDto>();
            #endregion

        }
    }
}
