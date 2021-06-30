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
            CreateMap<Category, CategoryClientGetDto>();
            CreateMap<Category, CategoryClientItemDto>();


            #endregion

            #region SubCategoriesMapper
            CreateMap<SubCategoryCreateDto, SubCategory>();
            CreateMap<Category, CategoryInSubDto>();
            CreateMap<SubCategory, SubCategoryGetDto>();
            CreateMap<SubCategory, SubCategoryItemDto>()
                .ForMember(dest => dest.CategoryName, from => from.MapFrom(x => x.Category.Name));
            #endregion

            #region TagsMapper
            CreateMap<TagCreateDto, Tag>();
            CreateMap<Tag,TagGetDto>();
            CreateMap<Tag,TagItemDto>();
            #endregion

            #region BrandsMapper
            CreateMap<BrandCreateDto, Brand>();
            CreateMap<Brand, BrandGetDto>();
            CreateMap<Brand, BrandItemDto>();
            #endregion

            #region SizesMapper
            CreateMap<SizeCreateDto, Size>();
            CreateMap<Size, SizeGetDto>();
            CreateMap<Size, SizeItemDto>();
            #endregion

            #region ColorsMapper
            CreateMap<ColorCreateDto, Color>();
            CreateMap<Color, ColorGetDto>();
            CreateMap<Color, ColorItemDto>();
            #endregion

            #region ProductsMapper
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
            CreateMap<Product, ProductFilterItemDto>();
            CreateMap<Product, ProductSearchItemDto>()
                .ForMember(dest => dest.BrandName, from => from.MapFrom(x => x.Brand.Name))
                .ForMember(dest => dest.SubCategoryName, from => from.MapFrom(x => x.SubCategory.Name));
            CreateMap<Product, ProductBookmarkDto>();
            CreateMap<Product, ProductListNewDto>();





            #endregion

            #region SlidersMapper
            CreateMap<SliderCreateDto, Slider>();
            CreateMap<Slider, SliderGetDto>();
            CreateMap<Slider, SliderItemDto>();
            #endregion

            #region OrdersMapper
            CreateMap<OrderCreateDto, Order>();
            CreateMap<AppUser, AppUserInOrderDto>();
            CreateMap<Product, ProductInOrderDto>();
            CreateMap<Order, OrderGetDto>();
            CreateMap<Order, OrderItemDto>()
                .ForMember(dest => dest.AppUserFullName, from => from.MapFrom(x => x.AppUser.FullName))
                .ForMember(dest => dest.ProductName, from => from.MapFrom(x => x.Product.Name));

            #endregion

            #region BlogCategoriesMapper
            CreateMap<BlogCategoryCreateDto, BlogCategory>();
            CreateMap<BlogCategory, BlogCategoryGetDto>();
            CreateMap<BlogCategory, BlogCategoryItemDto>();
            #endregion

            #region BlogsMapper
            CreateMap<BlogCreateDto,Blog>();
            CreateMap<BlogTagDto,BlogTag>();
            CreateMap<Blog, BlogGetDto>();
            CreateMap<BlogCategory, BlogInBlogCategoryDto>();
            CreateMap<BlogTag, TagInBlogDto>();
            CreateMap<BlogTag, TagInBlogDto>()
              .ForMember(dest => dest.TagName, from => from.MapFrom(x => x.Tag.Name));
            CreateMap<Blog, BlogItemDto>()
                .ForMember(dest => dest.BlogCategoryName, from => from.MapFrom(x => x.BlogCategory.Name));
            CreateMap<Blog, BlogClientItemDto>();

            #endregion

            #region FeaturesMapper
            CreateMap<FeatureCreateDto, Feature>();
            CreateMap<Feature, FeatureGetDto>();
            CreateMap<Feature, FeatureItemDto>();
            #endregion

            #region DiscountsMapper
            CreateMap<DiscountCreateDto, Discount>();
            CreateMap<Discount, DiscountGetDto>();
            CreateMap<Discount, DiscountItemDto>();
            #endregion

            #region ContactsMapper
            CreateMap<ContactCreateDto, Contact>();
            CreateMap<Contact, ContactGetDto>();
            #endregion

        }
    }
}
