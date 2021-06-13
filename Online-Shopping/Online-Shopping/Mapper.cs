using AutoMapper;
using Online_Shopping.Api.Client.DTOs;
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

            #region ProductMapper
            CreateMap<ProductCreateDto, Product>()
               .ForMember(dest => dest.ProductColors, from => from.MapFrom(x => x.ProductColors));
            CreateMap<ProductColorDto, ProductColor>();
            CreateMap<ProductSizeDto, ProductSize>();
            CreateMap<ProductPhotoDto, ProductPhoto>();
            CreateMap<Product, ProductGetDto>();
            CreateMap<ProductPhoto, PhotoInProductDto>();
            CreateMap<SubCategory, ProductInSubCategoryDto>();
            CreateMap<Brand, ProductInBrandDto>();
            CreateMap<ProductColor, ColorInProductDto>()
               .ForMember(dest => dest.ColorName, from => from.MapFrom(x => x.Color.Name));
            CreateMap<ProductSize, SizeInProductDto>()
                .ForMember(dest => dest.SizeName, from => from.MapFrom(x => x.Size.Name));
            CreateMap<Product, ProductItemDto>()
                .ForMember(dest => dest.SubCategoryName, from => from.MapFrom(x => x.SubCategory.Name))
                .ForMember(dest => dest.BrandName, from => from.MapFrom(x => x.Brand.Name));

            #region SlidersMapper
            CreateMap<SliderCreateDto, Slider>();
            CreateMap<Slider, SliderGetDto>();
            CreateMap<Slider, SliderItemDto>();
            #endregion
            #region OrdersMapper
            CreateMap<OrderCreateDto, Order>();
            #endregion
            #endregion

        }
    }
}
