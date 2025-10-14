using Riok.Mapperly.Abstractions;
namespace Service.Common.Mappings;

[Mapper(AllowNullPropertyAssignment = false)]
public partial class MapperlyMapper
{
    #region Generic Methods

    public static partial TDestination Map<TSource, TDestination>(TSource source);

    public static partial IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceCollection);

    #endregion Generic Methods

    #region 

    #endregion


}